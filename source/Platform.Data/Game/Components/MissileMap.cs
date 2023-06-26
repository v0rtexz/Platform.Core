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

using System.Collections;
using Ensage.Data.Game.Types;
using Ensage.Data.Memory;
using JetBrains.Annotations;
using ProcessMemoryUtilities.Managed;
using Serilog;

namespace Ensage.Data.Game.Components;

public class MissileMap : IEnumerable<AIMissileClient>
{
    #region Properties

    /// <summary>
    /// Injected logger instance
    /// </summary>
    private ILogger logger;

    private long basePtr;

    #endregion

    #region Constructors and Destructors

    public MissileMap([NotNull] ILogger logger)
    {
        this.logger = logger;

        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
            (IntPtr)(MemoryAccessor.BaseAddress + Offsets.MissileMap),
            ref this.basePtr);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Returns the size of the MissileList
    /// </summary>
    /// <returns>Size of the MissileList</returns>
    private int GetSize()
    {
        int size = 0;
        NativeWrapper.ReadProcessMemory<int>(MemoryAccessor.Handle, (IntPtr)(basePtr + 0x10), ref size);
        return size;
    }

    /// <summary>
    /// Returns the ManagerTemplate Unit Pointer which contains all Units if accessed.
    /// </summary>
    /// <returns>Address of the unit pointer</returns>
    private long GetMissileMap()
    {
        long missileMap = 0;
        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)(basePtr + 0x8), ref missileMap);

        return missileMap;
    }

    private long GetKey(long entryPtr)
    {
        long networkID = 0;
        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)(entryPtr + 0x20), ref networkID);

        return networkID;
    }

    private bool GetValue(long entryPtr, ref long result)
    {
        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)(entryPtr + 0x28), ref result);
        return result != 0;
    }

    public Dictionary<long, AIMissileClient> GetMap()
    {
        Dictionary<long, AIMissileClient> missileMap = new Dictionary<long, AIMissileClient>();

        for (int i = 0; i < GetSize(); i++)
        {
            long entryPtr = 0;

            //Get the missile map entrys
            NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)(GetMissileMap() + i * 0x8),
                ref entryPtr);

            long key = GetKey(entryPtr);
            AIMissileClient possibleDuplicate;


            // See if key already exists, if it does don't add into map
            if (missileMap.TryGetValue(key, out possibleDuplicate))
                continue;

            long value = 0;
            GetValue(entryPtr, ref value);

            AIMissileClient missile = new AIMissileClient(value);

            missileMap.Add(key, missile);
        }

        return missileMap;
    }

    public IEnumerator<AIMissileClient> GetEnumerator()
    {
        for (int i = 0; i < GetSize(); i++)
        {
            long entryPtr = 0;

            //Get the missile map entrys
            NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)(GetMissileMap() + 0x8),
                ref entryPtr);

            long entry = 0;

            // Access the Entry (map with <networkID,MissileObject>)
            NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)(entryPtr + i * 0x8),
                ref entry);

            long value = 0;

            GetValue(entryPtr, ref value);

            yield return new AIMissileClient(value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}