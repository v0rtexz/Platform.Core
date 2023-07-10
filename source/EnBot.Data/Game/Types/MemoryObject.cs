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

using Ensage.Data.Memory;
using ProcessMemoryUtilities.Managed;

/// <summary>
/// Contains functions to check the validity of memory objects and to make them more easily accessible.
/// </summary>
public abstract class MemoryObject
{
    protected long address;

    /// <summary>
    /// Read a property out of the objects memory space.
    /// </summary>
    /// <param name="address">The address to read from..</param>
    /// <param name="offset">The offset to read from.</param>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <returns>The property value.</returns>
    protected TType GetProperty<TType>(long address, int offset)
        where TType : unmanaged
    {
        TType buffer = default(TType);
        NativeWrapper.ReadProcessMemory<TType>(MemoryAccessor.Handle, (IntPtr)(address + offset), ref buffer);

        return buffer;
    }
    
    /// <summary>
    /// Read a property out of the objects memory space.
    /// </summary>
    /// <param name="offset">The offset to read from.</param>
    /// <typeparam name="TType">The type of the property.</typeparam>
    /// <returns>The property value.</returns>
    protected TType GetProperty<TType>(int offset)
        where TType : unmanaged
    {
        TType buffer = default(TType);
        NativeWrapper.ReadProcessMemory<TType>(MemoryAccessor.Handle, (IntPtr)(this.address + offset), ref buffer);

        return buffer;
    }
}