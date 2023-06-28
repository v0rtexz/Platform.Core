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

namespace Ensage.Data.Game.Types.Buff;

using JetBrains.Annotations;

/// <summary>
/// Contains information about all spellslots.
/// </summary>
public class BuffEntry : MemoryObject
{
    #region Properties

    [PublicAPI] public float StartTime => GetBuffStartTime();
    [PublicAPI] public int BuffType => GetBuffType();
    [PublicAPI] public string Name => GetBuffInfo().Name;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BuffManager"/> class.
    /// </summary>
    /// <param name="addr">The address of the BuffManager.</param>
    internal BuffEntry(long addr)
    {
        base.address = addr;
    }

    #endregion

    #region Methods

    /// <summary>
    /// BuffInfo is more detailed and deeper structure to gain more information about the buff.
    /// </summary>
    /// <returns>The BuffInfo instance.</returns>
    private BuffInfo GetBuffInfo()
    {
        long ptr = GetProperty<long>(0x10);

        BuffInfo buffInfoInstance = new BuffInfo(ptr);

        return buffInfoInstance;
    }

    /// <summary>
    /// Get the start time of the buff.
    /// </summary>
    /// <returns>The start time of the buff.</returns>
    private float GetBuffStartTime()
    {
        return GetProperty<float>(0x18);
    }

    /// <summary>
    /// Get the type of the buff.
    /// </summary>
    /// <returns>The buff type.</returns>
    private int GetBuffType()
    {
        return GetProperty<int>(0x8);
    }

    #endregion
}