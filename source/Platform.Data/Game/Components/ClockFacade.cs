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

using System.Diagnostics.CodeAnalysis;
using Ensage.Data.Memory;
using Ensage.Data.Utils;
using ProcessMemoryUtilities.Managed;

namespace Ensage.Data.Game.Components;

/// <summary>
/// Represents the clock of the game.
/// </summary>
public class ClockFacade : IGameComponent
{
    #region Properties

    /// <summary>
    /// The module manager instance
    /// </summary>
    [NotNull] private ModuleManager moduleManager;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ClockFacade"/> class.
    /// </summary>
    /// <param name="memoryAccessor">The memory reading instance.</param>
    /// <param name="moduleManager">The module manager instance.</param>
    public ClockFacade([NotNull] ModuleManager moduleManager)
    {
        this.moduleManager = moduleManager;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get the current gametime.
    /// </summary>
    /// <returns>The gametime.</returns>
    public float GetGameTime()
    {
        float gametime = 0f;
        NativeWrapper.ReadProcessMemory<float>(MemoryAccessor.Handle, MemoryAccessor.BaseAddress + Offsets.GameTime,
            ref gametime);

        return gametime;
    }

    public int GetFPS()
    {
        long offset = moduleManager.GetModule("OWClient.dll") - 0x1A112510E34;
        Console.WriteLine(offset);
        int fps = 0;
        NativeWrapper.ReadProcessMemory<int>(MemoryAccessor.Handle,
            (IntPtr)(moduleManager.GetModule("OWClient.dll") + 0x3CC920), ref fps);
        return fps;
    }

    public OperationResult Construct()
    {
        return OperationResult.SUCCESS;
    }

    #endregion
}