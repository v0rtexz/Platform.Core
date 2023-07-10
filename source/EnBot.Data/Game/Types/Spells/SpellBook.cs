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

using JetBrains.Annotations;

namespace Ensage.Data.Game.Types.Spells;

/// <summary>
/// Contains information about all spellslots.
/// </summary>
public class SpellBook : MemoryObject
{
    #region Properties

    [PublicAPI] public int Level => GetProperty<int>(Offsets.SpellSlotLevel);
    [PublicAPI] public int Charges => GetProperty<int>(Offsets.SpellSlotCharges);
    [PublicAPI] public float ReadTime => GetProperty<float>(Offsets.SpellSlotTime);
    [PublicAPI] public float TimeCharge => GetProperty<float>(Offsets.SpellSlotTimeCharge);

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SpellBook"/> class.
    /// </summary>
    /// <param name="addr">The address of the spellbook.</param>
    internal SpellBook(long addr)
    {
        base.address = addr;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Access the SpellInfo structure.
    /// </summary>
    /// <returns>SpellInfo instance.</returns>
    internal SpellInfo GetSpellInfo()
    {
        long ptr = GetProperty<long>(Offsets.SpellSlotSpellInfo);

        return new SpellInfo(ptr);
    }

    #endregion
}