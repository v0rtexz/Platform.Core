#region Copyright (c) Ensage GmbH

// ////////////////////////////////////////////////////////////////////////////////
//
//        Ensage GmbH Source Code
//        Copyright (c) 2020-2023 Ensage GmbH
//        ALL RIGHTS RESERVED.
//
//    The entire contents of this file is protected by German and
//    International Copyright Laws. Unauthorized reproduction,
//    reverse-engineering, and distribution of all or any portion of
//    the code contained in this file is strictly prohibited and may
//    result in severe civil and criminal penalties and will be
//    prosecuted to the maximum extent possible under the law.
//
//    RESTRICTIONS
//
//    THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES
//    ARE CONFIDENTIAL AND PROPRIETARY TRADE SECRETS OF
//    Ensage GMBH.
//
//    THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED
//    FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE
//    COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE
//    AVAILABLE TO OTHER INDIVIDUALS WITHOUT WRITTEN CONSENT
//    AND PERMISSION FROM Ensage GMBH.
// 
// ////////////////////////////////////////////////////////////////////////////////

#endregion

using Ensage.Data.Game.Types;
using Ensage.Data.Game.Types.Spells;
using Ensage.Data.Rendering;

namespace Ensage.Data.Events;

using Ensage.Data.Events.Args;
using Ensage.Data.Game;
using JetBrains.Annotations;
using Serilog;

/// <summary>
/// The OnProcessSpell callback is being triggered on every spell or missile.
/// </summary>
public class OnProcessSpell : ICallback
{
    [NotNull] private ILogger logger;
    [NotNull] private World world;
    [NotNull] private Engine engine;

    private Dictionary<long, AIHeroClient> casterMap = new Dictionary<long, AIHeroClient>();

    public OnProcessSpell([NotNull] ILogger logger, [NotNull] World world, [NotNull] Engine engine)
    {
        this.logger = logger;
        this.world = world;
        this.engine = engine;

        this.InstantiateCallback();
    }

    /// <summary>
    /// Triggered if a spell has been casted.
    /// If a spell is not recognized as missile, I try to recognize it as ActiveSpell.
    /// </summary>
    public void TriggerIfConditionMet()
    {
        foreach (var missile in world.Missiles)
        {
            OnProcessSpellArgs args = new OnProcessSpellArgs(
                missile.Value.MissileName,
                missile.Value.CasterName,
                missile.Value.CasterNameHash,
                missile.Value.NetworkID,
                missile.Value.StartPosition,
                missile.Value.EndPosition,
                engine.Clock.GetGameTime(),
                false,
                false
            );

            // The hero caster
            AIHeroClient heroCaster = null;

            // The minion caster. If no hero casting was found, it will look for a minion.
            AIMinionClient minionCaster = null;

            // Try to find the hero in the map first.
            // If the hash of the casting unit has been found in the map,
            // we return it instead of looping through heroes again for performance.
            if (!casterMap.TryGetValue(args.CasterHash, out heroCaster))
            {
                foreach (var hero in world.Heroes)
                {
                    if (hero.HashedName == args.CasterHash)
                    {
                        heroCaster = hero;
                        casterMap.Add(args.CasterHash, hero);
                        break;
                    }
                }
            }

            // if no hero was found with hash, it must be a minion.
            if (heroCaster == null)
            {
                foreach (var minion in world.Minions)
                {
                    if (!minion.IsLaneMinion())
                        continue;

                    if (args.CasterHash == 1888323292288 || // SRU_ChaosMinionRanged
                        args.CasterHash == 1888323292288) // SRU_OrderMinionRanged
                    {
                        minionCaster = minion;

                        break;
                    }
                }
            }


            // If no hero caster has been stored, it's a minion so we store minionCaster in the args.
            args.Caster = heroCaster != null ? heroCaster : minionCaster;

            if (args.Caster != null)
            {
                EventManager.InvokeCallback<EventDelegate.EvtOnProcessSpell, OnProcessSpellArgs>(args);
                return;
            }
        }

        foreach (var hero in world.Heroes)
        {
            if (hero.IsCasting)
            {
                ActiveSpell spell = hero.GetActiveSpell();
                SpellInfo spellInfo = spell.GetSpellInfo();

                OnProcessSpellArgs args = new OnProcessSpellArgs(
                    spellInfo.SpellName,
                    hero.ChampionName,
                    spellInfo.SpellNameHash,
                    0,
                    spell.StartPosition,
                    spell.EndPositiion,
                    spell.StartTime,
                    spell.IsAutoAttack,
                    spell.IsSpell
                );

                args.Caster = hero;
                EventManager.InvokeCallback<EventDelegate.EvtOnProcessSpell, OnProcessSpellArgs>(args);
            }
        }
    }


    private void Test(OnProcessSpellArgs args)
    {
    }

    public void InstantiateCallback()
    {
        EventManager.RegisterCallback<EventDelegate.EvtOnUpdate>(() => TriggerIfConditionMet());
        EventManager.RegisterCallback<EventDelegate.EvtOnProcessSpell>((args) => Test(args));
    }
}