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

using Ensage.Data.Utils;

namespace Ensage.Data.Game.Types.Spells;

/// <summary>
/// SpellInfo class
/// </summary>
internal class SpellInfo : MemoryObject
{
    #region Properties
    internal string SpellName => RiotString.Get(this.address + Offsets.SpellName);
    internal long SpellNameHash => GetProperty<long>(Offsets.SpellName);
    
    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SpellInfo"/> class.
    /// </summary>
    /// <param name="address">The address of the structure.</param>
    internal SpellInfo(long address)
    {
        this.address = address;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Access the <see cref="SpellData"/> structure.
    /// </summary>
    /// <returns></returns>
    internal SpellData GetSpellData()
    {
        long ptr = GetProperty<long>(Offsets.SpellData);

        return new SpellData(ptr);
    }

    internal SpellDetails GetSpellDetails()
    {
        long ptr = GetProperty<long>(Offsets.DetailedMissileInfo);

        return new SpellDetails(ptr);
    }

    #endregion
}