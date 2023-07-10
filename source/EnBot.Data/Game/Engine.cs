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

using Ensage.Data.Game.Components;
using JetBrains.Annotations;
using Serilog;

namespace Ensage.Data.Game;

/// <summary>
/// Contains components that are not directly attached to the game or games objects.
/// </summary>
public class Engine
{
    #region Properties

    public ClockFacade Clock { get; set; }
    public Renderer Renderer { get; set; }

    private ILogger logger;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Engine"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="clock">The ingame clock.</param>
    /// <param name="renderer">The renderer.</param>
    /// <exception cref="ArgumentNullException">Throws an Exception if one of the parameters is NULL.</exception>
    public Engine(
        [NotNull] ILogger logger,
        [NotNull] ClockFacade clock,
        [NotNull] Renderer renderer
    )
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.Clock = clock ?? throw new ArgumentNullException(nameof(clock));
        this.Renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    #endregion

    #region Methods

    #endregion
}