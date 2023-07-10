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

using Ensage.Data.Memory;
using ProcessMemoryUtilities.Managed;

namespace Ensage.Data.Game.Types.Buff;

using System.Collections;

/// <summary>
/// Contains information about all spellslots.
/// </summary>
public class BuffManagerEntriesArray : MemoryObject, IEnumerable<BuffEntry>
{
    #region Properties

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="BuffManager"/> class.
    /// </summary>
    /// <param name="addr">The address of the BuffManager.</param>
    internal BuffManagerEntriesArray(long addr)
    {
        base.address = addr;
    }

    #endregion

    #region Methods

    #endregion

    private float GetGameTime()
    {
        float gameTime = 0f;
        NativeWrapper.ReadProcessMemory<float>(MemoryAccessor.Handle, MemoryAccessor.BaseAddress + Offsets.GameTime,
            ref gameTime);
        return gameTime;
    }

    public IEnumerator<BuffEntry> GetEnumerator()
    {
        long buffManagerEntriesArrayStart = GetProperty<long>(this.address + Offsets.BuffManagerInstance + 0x18, 0x0);
        long buffManagerEntriesArrayEnd = GetProperty<long>(this.address + Offsets.BuffManagerInstance + 0x20, 0x0);
        float gameTime = GetGameTime(); // Cache the game time outside the loop

        for (long i = buffManagerEntriesArrayStart; i != buffManagerEntriesArrayEnd; i += 0x10)
        {
            long buffEntryPtr = GetProperty<long>(i, 0x0);
            long buffEntryInfo = GetProperty<long>(buffEntryPtr, 0x10);

            if (buffEntryPtr > 0 && buffEntryInfo > 0)
            {
                float buffEndTime = GetProperty<float>(buffEntryPtr, Offsets.BuffEndTime);

                if (buffEndTime >= gameTime) // Compare directly with the cached game time
                {
                    BuffEntry buffEntry = new BuffEntry(buffEntryPtr);
                    yield return buffEntry;
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}