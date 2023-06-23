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

using Platform.Data.Game.Components;

namespace Platform.Data.Game.Engine;

using Platform.Data.Game.Components;

using System.Text;

using ProcessMemoryUtilities.Managed;

/// <summary>
/// Utility class to read Riots Strings.
/// </summary>
internal class RiotString
{
    internal static string Get(long address, int stringSize = 512)
    {
        byte[] dataBuffer = new byte[stringSize];
        Memory.Memory memory = ComponentController.GetComponent<Memory.Memory>();
        NativeWrapper.ReadProcessMemoryArray(memory.Handle, (IntPtr)address, dataBuffer, 0, dataBuffer.Length);

        return Encoding.UTF8.GetString(dataBuffer).Split('\0')[0];
    }
}