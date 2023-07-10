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

namespace Ensage.Data.Game.Types;

using System.Numerics;
using Ensage.Data.Game.Types.Spells;
using Ensage.Data.Utils;
using JetBrains.Annotations;

/// <summary>
/// Type for missiles. Inherited by <see cref="AIBaseClient"/>.
/// </summary>
public class AIMissileClient : AIBaseClient
{
    #region Properties

    [PublicAPI] public Vector3 StartPosition => GetProperty<Vector3>(Offsets.MissileStartPosition);
    [PublicAPI] public Vector3 EndPosition => GetProperty<Vector3>(Offsets.MissileEndPosition);
    [PublicAPI] public string MissileName => GetSpellInfo().GetSpellData().MissileName;
    [PublicAPI] public string CasterName => GetSpellInfo().GetSpellDetails().CasterName;
    [PublicAPI] public long CasterNameHash => GetSpellInfo().GetSpellDetails().CasterNameHash;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AIMissileClient"/> class.
    /// </summary>
    /// <param name="address">The address of the unit.</param>
    public AIMissileClient(long address)
        : base(address)
    {
    }

    #endregion

    #region Methods

    /// <summary>
    /// Access the SpellInfo structure.
    /// </summary>
    /// <returns>SpellInfo instance.</returns>
    internal SpellInfo GetSpellInfo()
    {
        long ptr = GetProperty<long>(Offsets.MissileSpellInfo);

        return new SpellInfo(ptr);
    }

    #endregion
}