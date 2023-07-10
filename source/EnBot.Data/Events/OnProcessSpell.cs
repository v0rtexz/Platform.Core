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

using System.Numerics;
using Ensage.Data.Game.Components;
using Ensage.Data.Rendering;
using GameOverlay.Drawing;
using Color = System.Drawing.Color;

namespace Ensage.Data.Events;

using Ensage.Data.Game.Types;
using Ensage.Data.Game.Types.Spells;
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
        bool triggered = false;

        foreach (var missile in world.Missiles)
        {
            OnProcessSpellArgs args = default(OnProcessSpellArgs); // Create a new instance for each missile
            args.Name = missile.Value.MissileName;
            args.CasterName = missile.Value.CasterName;
            args.CasterHash = missile.Value.CasterNameHash;
            args.NetworkID = 0;
            args.StartPosition = missile.Value.StartPosition;
            args.EndPosition = missile.Value.EndPosition;
            args.StartTime = engine.Clock.GetGameTime();
            args.IsSpell = true;
            args.IsAutoAttack = true;

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
                EventManager.ExecuteEvent<OnProcessSpellArgs>((short)EventID.OnProcessSpell, args);
                triggered = true;
            }
        }

        if (triggered)
            return;

        foreach (var hero in world.Heroes)
        {
            if (hero.IsCasting)
            {
                OnProcessSpellArgs args = default(OnProcessSpellArgs); // Create a new instance for each missile
                ActiveSpell spell = hero.GetActiveSpell();
                SpellInfo spellInfo = spell.GetSpellInfo();

                args.Name = spellInfo.SpellName;
                args.CasterName = hero.ChampionName;
                args.CasterHash = hero.HashedName;
                args.NetworkID = 0;
                args.StartPosition = spell.StartPosition;
                args.EndPosition = spell.EndPositiion;
                args.StartTime = spell.StartTime;
                args.IsSpell = spell.IsSpell;
                args.IsAutoAttack = spell.IsAutoAttack;

                args.Caster = hero;
                EventManager.ExecuteEvent<OnProcessSpellArgs>((short)EventID.OnProcessSpell, args);
            }
        }
    }


    private void Test(OnProcessSpellArgs args)
    {
    }

    public void InstantiateCallback()
    {
        EventManager.RegisterEvent<OnProcessSpellArgs>((short)EventID.OnProcessSpell, Test);
        EventManager.RegisterEvent((short)EventID.OnUpdate, TriggerIfConditionMet);
    }
}