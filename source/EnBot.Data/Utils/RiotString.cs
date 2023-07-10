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

using System.Text;
using Ensage.Data.Game.Components;
using Ensage.Data.Memory;
using ProcessMemoryUtilities.Managed;

namespace Ensage.Data.Utils;

/// <summary>
/// Utility class to read Riots Strings.
/// </summary>
internal static class RiotString
{
    private static Dictionary<long, string> stringMap = new Dictionary<long, string>();

    /// <summary>
    /// Read a string.
    /// This should be used for clear UTF8 strings which are not stored inside a additional memory/ behind a pointer.
    /// </summary>
    /// <param name="address">The address to read from.</param>
    /// <param name="stringSize">The amount to read.</param>
    /// <returns></returns>
    internal static string Get(long address, int stringSize = 50)
    {
        long hash = 0;
        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)address, ref hash);

        string result;
        if (stringMap.TryGetValue(hash, out result))
            return result;

        // Read the string from memory into a byte array
        byte[] dataBuffer = new byte[stringSize];
        NativeWrapper.ReadProcessMemoryArray(MemoryAccessor.Handle, (IntPtr)address, dataBuffer, 0, dataBuffer.Length);

        // Find the null terminator to determine the actual length of the string
        int nullTerminatorIndex = Array.IndexOf(dataBuffer, (byte)0);
        int actualLength = nullTerminatorIndex >= 0 ? nullTerminatorIndex : dataBuffer.Length;

        // Convert the byte array to a string using the actual length
        result = Encoding.UTF8.GetString(dataBuffer, 0, actualLength);

        stringMap.Add(hash, result);

        return result;
    }

    /// <summary>
    /// Read a string.
    /// This should be used for strings stored inside a additional memory space / behind a pointer.
    /// </summary>
    /// <param name="address">The address to read from.</param>
    /// <param name="stringSize">The amount to read.</param>
    /// <returns></returns>
    internal static string GetByPtr(long address, int maxStringLength = 50)
    {
        long hash = 0;
        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)address, ref hash);

        string result;
        if (stringMap.TryGetValue(hash, out result))
            return result;

        byte[] dataBuffer = new byte[maxStringLength];
        List<byte> tempBuffer = new List<byte>(maxStringLength);

        long ptr = 0;
        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle, (IntPtr)address, ref ptr);

        byte currentByte = 0;
        int bytesRead = 0;

        do
        {
            NativeWrapper.ReadProcessMemory<byte>(MemoryAccessor.Handle, (IntPtr)(ptr + bytesRead), ref currentByte);
            tempBuffer.Add(currentByte);
            bytesRead++;
        } while (currentByte != 0 && bytesRead < maxStringLength);

        int actualLength = tempBuffer.Count;
        if (actualLength > maxStringLength)
            actualLength = maxStringLength;

        Array.Copy(tempBuffer.ToArray(), dataBuffer, actualLength);
        result = Encoding.UTF8.GetString(dataBuffer, 0, actualLength);

        stringMap.Add(hash, result);

        return result;
    }
}