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

namespace Ensage.Data.Game.Components.ManagerTemplates;

using JetBrains.Annotations;
using ProcessMemoryUtilities.Managed;

/// <summary>
/// Base class for all ManagerTemplates. That includes the so called HeroList, MinionList, TurretList etc.
/// </summary>
public abstract class ManagerTemplate
{
    //basePtr, listPtr
    protected static long[,] entityListPtrs;


    protected ManagerTemplate()
    {
        entityListPtrs = new long[3, 2];

        this.InitializePointers(ManagerTemplateType.Heroes);
        this.InitializePointers(ManagerTemplateType.Minions);
        this.InitializePointers(ManagerTemplateType.Turrets);
    }

    private void InitializePointers(ManagerTemplateType type)
    {
        switch (type)
        {
            case ManagerTemplateType.Heroes:

                NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
                    MemoryAccessor.BaseAddress + Offsets.HeroList, ref entityListPtrs[0, 0]);

                NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
                    (IntPtr)(GetBasePtr(ManagerTemplateType.Heroes) + Offsets.ManagerTemplateListPtr),
                    ref entityListPtrs[0, 1]);
                break;

            case ManagerTemplateType.Minions:

                // I only sit base pointer here because the list pointer is dynamic.
                NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
                    MemoryAccessor.BaseAddress + Offsets.MinionList, ref entityListPtrs[1, 0]);
                break;

            case ManagerTemplateType.Turrets:

                NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
                    MemoryAccessor.BaseAddress + Offsets.TurretList, ref entityListPtrs[2, 0]);

                NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
                    (IntPtr)(GetBasePtr(ManagerTemplateType.Turrets) + Offsets.ManagerTemplateListPtr),
                    ref entityListPtrs[2, 1]);
                break;
        }
    }

    /// <summary>
    /// Get the base pointer for the specified entity type.
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns>Base pointer for the specified entity type.</returns>
    protected IntPtr GetBasePtr(ManagerTemplateType entityType)
    {
        switch (entityType)
        {
            case ManagerTemplateType.Heroes:
                return (IntPtr)entityListPtrs[0, 0];
            case ManagerTemplateType.Minions:
                return (IntPtr)entityListPtrs[1, 0];
            case ManagerTemplateType.Turrets:
                return (IntPtr)entityListPtrs[2, 0];
            default:
                return IntPtr.Zero;
        }
    }

    /// <summary>
    /// Get the list pointer for the specified entity type.
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns>List pointer for the specified entity type.</returns>
    protected IntPtr GetListPtr(ManagerTemplateType entityType)
    {
        switch (entityType)
        {
            case ManagerTemplateType.Heroes:
                return (IntPtr)entityListPtrs[0, 1];

            case ManagerTemplateType.Minions:
                NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
                    (IntPtr)(GetBasePtr(ManagerTemplateType.Minions) + Offsets.ManagerTemplateListPtr),
                    ref entityListPtrs[1, 1]);

                return (IntPtr)entityListPtrs[1, 1];

            case ManagerTemplateType.Turrets:
                return (IntPtr)entityListPtrs[2, 1];

            default:
                return IntPtr.Zero;
        }
    }

    /// <summary>
    /// Get the size of the list for the specified entity type.
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns>Size of the list for the specified entity type.</returns>
    public int GetListSize(ManagerTemplateType entityType)
    {
        int size = 0;
        switch (entityType)
        {
            case ManagerTemplateType.Heroes:
                NativeWrapper.ReadProcessMemory<int>(MemoryAccessor.Handle,
                    (IntPtr)(GetBasePtr(ManagerTemplateType.Heroes) + Offsets.ManagerTemplateSize), ref size);
                break;
            case ManagerTemplateType.Minions:
                NativeWrapper.ReadProcessMemory<int>(MemoryAccessor.Handle,
                    (IntPtr)(GetBasePtr(ManagerTemplateType.Minions) + Offsets.ManagerTemplateSize), ref size);
                break;
            case ManagerTemplateType.Turrets:
                NativeWrapper.ReadProcessMemory<int>(MemoryAccessor.Handle,
                    (IntPtr)(GetBasePtr(ManagerTemplateType.Turrets) + Offsets.ManagerTemplateSize), ref size);
                break;
            default:
                return 0;
        }

        return size;
    }
}