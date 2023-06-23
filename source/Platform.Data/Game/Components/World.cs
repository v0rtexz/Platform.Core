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

using Platform.Data.Game.Types;
using Platform.Data.Utils;
using ProcessMemoryUtilities.Managed;

namespace Platform.Data.Game.Components;

using Platform.Data.Utils;
using JetBrains.Annotations;
using Serilog;

/// <summary>
/// World component.
/// </summary>
public class World : IGameComponent
{
    [NotNull] private ILogger logger;

    private Memory.Memory memory;

    public World(ILogger logger, Memory.Memory memory)
    {
        this.logger = logger;
        this.memory = memory;

        logger.Debug("World created!");
    }

    public OperationResult Construct()
    {
        return OperationResult.SUCCESS;
    }

    public AIHeroClient GetLocalPlayer()
    {
        long player = 0;
        NativeWrapper.ReadProcessMemory<long>(memory.Handle, (IntPtr)(memory.BaseAddress + Offsets.LocalPlayer),
            ref player);

        return new AIHeroClient(player);
    }
}