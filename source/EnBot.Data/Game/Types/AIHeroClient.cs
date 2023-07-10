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

using Ensage.Data.Game.Components;
using Ensage.Data.Game.Types.Spells;
using Ensage.Data.Utils;
using JetBrains.Annotations;

namespace Ensage.Data.Game.Types;

/// <summary>
/// Type for heroes. Inherited by <see cref="AIBaseClient"/>.
/// </summary>
public class AIHeroClient : AIBaseClient
{
    #region Properties

    [PublicAPI] public short LevelUpPoints => GetProperty<short>(Offsets.LevelPoints);
    [PublicAPI] public float TotalEXP => GetProperty<float>(Offsets.TotalEXP);
    [PublicAPI] public string ChampionName => GetChampionName();
    [PublicAPI] public SpellBook Q => GetSpellSlot(SpellSlot.Q);
    [PublicAPI] public SpellBook W => GetSpellSlot(SpellSlot.W);
    [PublicAPI] public SpellBook E => GetSpellSlot(SpellSlot.E);
    [PublicAPI] public SpellBook R => GetSpellSlot(SpellSlot.R);
    [PublicAPI] public bool IsCasting => GetProperty<long>(Offsets.SpellCast) != 0;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AIHeroClient"/> class.
    /// </summary>
    /// <param name="address">The address of the unit.</param>
    public AIHeroClient(long address)
        : base(address)
    {
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get the name of the champion. This usually differs to the ObjectName.
    /// </summary>
    /// <returns>The name of the object.</returns>
    private string GetChampionName()
    {
        return RiotString.Get(this.address + Offsets.ChampionName);
    }

    /// <summary>
    /// Get the <see cref="SpellBook"/> instance for the given <see cref="SpellSlot"/>.
    /// </summary>
    /// <param name="slot">The slot to get.</param>
    /// <returns>The SpellBook instance.</returns>
    private SpellBook GetSpellSlot(SpellSlot slot)
    {
        long ptr = GetProperty<long>(Offsets.SpellBook + (short)slot * 0x8);

        return new SpellBook(ptr);
    }

    /// <summary>
    /// The <see cref="ActiveSpell"/> instance.
    /// </summary>
    /// <returns>The instance.</returns>
    internal ActiveSpell GetActiveSpell()
    {
        long ptr = GetProperty<long>(Offsets.SpellCast);

        return new ActiveSpell(ptr);
    }

    #endregion
}