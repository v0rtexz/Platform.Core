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

using Platform.Data.Exceptions;
using Platform.Data.Utils;

namespace Platform.Data.Memory;

using Platform.Data.Exceptions;
using Platform.Data.Utils;
using System.Diagnostics;
using JetBrains.Annotations;
using ProcessMemoryUtilities.Managed;
using ProcessMemoryUtilities.Native;
using Serilog;

/// <summary>
/// Holds data for the memory.
/// </summary>
public class Memory
{
    #region Properties

    [NotNull] private readonly ILogger logger;

    /// <summary>
    /// The process id of the game process.
    /// </summary>
    internal int ProcessID { get; set; }

    /// <summary>
    /// The handle to the game process.
    /// </summary>
    [NotNull]
    internal IntPtr Handle { get; set; }

    /// <summary>
    /// The base address of the game process.
    [NotNull]
    internal IntPtr BaseAddress { get; set; }

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Memory"/> class.
    /// </summary>
    /// <param name="logger">The injected logger</param>
    public Memory([NotNull] ILogger logger)
    {
        this.logger = logger;

        logger.Debug("Initializing memory");

        if (this.InitMemory() < OperationResult.SUCCESS)
        {
            this.logger.Error("Failed to initiate memory! Retrying in 10 seconds...");
            OperationResult finalResult = Retry.Do(this.InitMemory, TimeSpan.FromSeconds(10), 1);

            if (finalResult != OperationResult.SUCCESS)
            {
                throw new MemoryException("Failed to initialize memory after multiple attempts!");
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Initiates the memory.
    /// </summary>
    /// <returns>Returns if the operation was successful or failed.</returns>
    internal OperationResult InitMemory()
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

        this.ProcessID = gameProc.Id;
        this.logger.Debug("ProcessID: {ProcessID} ", this.ProcessID);

        this.Handle = NativeWrapper.OpenProcess(ProcessAccessFlags.ReadWrite, this.ProcessID);
        this.logger.Debug("Handle: {Handle}", this.Handle);

        this.BaseAddress = Process.GetProcessById(this.ProcessID).Modules[0].BaseAddress;
        this.logger.Debug("BaseAddress: {BaseAddr}", this.BaseAddress);

        return this.BaseAddress != IntPtr.Zero ? OperationResult.SUCCESS : OperationResult.FAILURE;
    }

    #endregion
}