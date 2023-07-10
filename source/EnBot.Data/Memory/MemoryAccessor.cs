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

using System.Diagnostics;
using Ensage.Data.Exceptions;
using Ensage.Data.Utils;
using JetBrains.Annotations;
using ProcessMemoryUtilities.Managed;
using ProcessMemoryUtilities.Native;
using Serilog;

namespace Ensage.Data.Memory;

/// <summary>
/// Holds data for the memory.
/// </summary>
public static class MemoryAccessor
{
    #region Properties

    /// <summary>
    /// The process id of the game process.
    /// </summary>
    internal static int ProcessID { get; set; }

    /// <summary>
    /// The handle to the game process.
    /// </summary>
    [NotNull]
    internal static IntPtr Handle { get; set; }

    /// <summary>
    /// The base address of the game process.
    [NotNull]
    internal static IntPtr BaseAddress { get; set; }

    #endregion

    #region Methods

    internal static void Init()
    {
        if (InitMemory() < OperationResult.SUCCESS)
        {
            Console.WriteLine("Failed to initiate memory! Retrying in 10 seconds...");
            OperationResult finalResult = Retry.Do(InitMemory, TimeSpan.FromSeconds(10), 1);

            if (finalResult != OperationResult.SUCCESS)
            {
                throw new MemoryException("Failed to initialize memory after multiple attempts!");
            }
        }
    }

    /// <summary>
    /// Initiates the memory.
    /// </summary>
    /// <returns>Returns if the operation was successful or failed.</returns>
    private static OperationResult InitMemory()
    {
        Process[] processes = Process.GetProcessesByName("League of Legends");

        if (processes.Length == 0)
        {
            return OperationResult.FAILURE;
        }

        Process gameProc;

        try
        {
            gameProc = Process.GetProcessesByName("League of Legends")[0];
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return OperationResult.FAILURE;
        }

        ProcessID = gameProc.Id;
        Console.WriteLine("ProcessID: " + ProcessID);

        Handle = NativeWrapper.OpenProcess(ProcessAccessFlags.ReadWrite, ProcessID);
        Console.WriteLine("Handle: " + Handle);

        BaseAddress = Process.GetProcessById(ProcessID).Modules[0].BaseAddress;
        Console.WriteLine("BaseAddr: " + BaseAddress);

        return BaseAddress != IntPtr.Zero ? OperationResult.SUCCESS : OperationResult.FAILURE;
    }

    #endregion
}