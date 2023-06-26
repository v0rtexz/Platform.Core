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

namespace Ensage.Data.Game;

using Ensage.Data.Game.Components;
using Ensage.Data.Game.Components.ManagerTemplates;
using Ensage.Data.Game.Types;
using Ensage.Data.Memory;
using Ensage.Data.Utils;
using JetBrains.Annotations;
using ProcessMemoryUtilities.Managed;
using Serilog;

/// <summary>
/// World component.
/// </summary>
public class World : IGameComponent
{
    #region Properties

    /// <summary>
    /// Gets all <see cref="AIHeroClient"/>.
    /// </summary>
    public HeroManager Heroes { get; }

    /// <summary>
    /// Gets all <see cref="AIMinionClient"/>.
    /// </summary>
    public MinionManager Minions { get; }

    /// <summary>
    /// Gets all <see cref="AITurretClient"/>.
    /// </summary>
    public TurretManager Turrets { get; }

    /// <summary>
    /// Gets all active <see cref="AIMissileClient"/>.
    /// </summary>
    public MissileMap Missiles { get; }

    /// <summary>
    /// Injected logger instance.
    /// </summary>
    [NotNull] private readonly ILogger logger;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="World"/> class.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="memoryAccessor">MemoryAccessor instance.</param>
    /// <param name="heroManager">The <see cref="ManagerTemplate"/> for heroes.</param>
    /// <param name="minionManager">The <see cref="ManagerTemplate"/> for minions.</param>
    /// <param name="turretManager">The <see cref="ManagerTemplate"/> for turrets.</param>
    /// <param name="missiles">Map including all Missiles.</param>
    public World(
        [NotNull] ILogger logger,
        [NotNull] HeroManager heroManager,
        [NotNull] MinionManager minionManager,
        [NotNull] TurretManager turretManager,
        [NotNull] MissileMap missiles
    )
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.Heroes = heroManager ?? throw new ArgumentNullException(nameof(heroManager));
        this.Minions = minionManager ?? throw new ArgumentNullException(nameof(minionManager));
        this.Turrets = turretManager ?? throw new ArgumentNullException(nameof(turretManager));
        this.Missiles = missiles ?? throw new ArgumentNullException(nameof(missiles));

        this.logger.Debug("World created!");
    }

    #endregion

    #region Methods

    public OperationResult Construct()
    {
        return OperationResult.SUCCESS;
    }

    /// <summary>
    /// Get the LocalPlayer.
    /// </summary>
    /// <returns>The local player (you).</returns>
    public AIHeroClient GetLocalPlayer()
    {
        long player = 0;
        NativeWrapper.ReadProcessMemory<long>(MemoryAccessor.Handle,
            (IntPtr)(MemoryAccessor.BaseAddress + Offsets.LocalPlayer),
            ref player);

        return new AIHeroClient(player);
    }

    #endregion
}