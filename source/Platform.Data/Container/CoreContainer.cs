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

namespace Platform.Data.Container;

using Platform.Data.Utils;
using Platform.Data.Rendering;
using Platform.Data.Game.Components;
using Autofac;
using Serilog;
using Serilog.Events;
using JetBrains.Annotations;
using Platform.Data.Events;

/// <summary>
/// Container which containts instances for the core functionality.
/// </summary>
public class CoreContainer
{
    #region Properties

    /// <summary>
    /// The built container
    /// </summary>
    [NotNull] private static IContainer? container;

    /// <summary>
    /// The lifetime scope of the container
    /// </summary>
    [NotNull] private static ILifetimeScope? scope;

    /// <summary>
    /// The builder of the container.
    /// </summary>
    [NotNull] private static ContainerBuilder? containerBuilder;

    [NotNull] private static Serilog.Core.Logger logger;

    #endregion

    #region Methods

    /// <summary>
    /// Build and run the Container and initialize the scope.
    /// </summary>
    /// <returns>Returns a <see cref="OperationResult"/> that indicates if the build succeeded.</returns>
    public static OperationResult Initialize()
    {
        logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();

        logger.Write(LogEventLevel.Information,
            "---------------------------------------Core Container---------------------------------------");

        containerBuilder = new ContainerBuilder();

        // Register the Logger
        containerBuilder.RegisterType<Utils.Logger>().As<ILogger>().SingleInstance();
        containerBuilder.RegisterType<Memory.Memory>().As<Memory.Memory>().SingleInstance().AutoActivate();
        containerBuilder.RegisterType<Memory.ModuleManager>().As<Memory.ModuleManager>().SingleInstance().AutoActivate();

        // Register callback types to the builder
        RegisterCallbacks();

        // Register game component types to the builder
        RegisterGameComponents();

        containerBuilder.RegisterType<Present>().SingleInstance();

        OperationResult result = Build();

        // Run present after building
        ComponentController.GetComponent<Present>();

        return result;
    }

    /// <summary>
    /// Get the Instance of the initialized <see cref="ILifetimeScope"/> Instance.
    /// </summary>
    /// <returns>The Instance.</returns>
    internal static ILifetimeScope Get()
    {
        return scope;
    }

    /// <summary>
    /// Get the container builder.
    /// </summary>
    /// <returns>The container builder.</returns>
    [CanBeNull]
    internal static ContainerBuilder GetBuilder()
    {
        return containerBuilder;
    }

    /// <summary>
    /// Build the container.
    /// </summary>
    /// <returns>Returns the result of the operation.</returns>
    private static OperationResult Build()
    {
        // Build the containers
        logger.Write(LogEventLevel.Debug, "Building core container...");
        container = containerBuilder.Build();

        if (container == null)
        {
            logger.Write(LogEventLevel.Fatal, "Failed to build core container!");
            return OperationResult.FAILURE;
        }

        logger.Write(LogEventLevel.Debug, "Initiating core lifetime scope...");
        scope = container?.BeginLifetimeScope();

        return scope == null ? OperationResult.FAILURE : OperationResult.SUCCESS;
    }

    /// <summary>
    /// Register all game components to the core builder.
    /// </summary>
    private static void RegisterGameComponents()
    {
        logger.Debug("Registering Game Components...");
        ComponentController.RegisterComponent<World>();
        ComponentController.RegisterComponent<Renderer>();
        ComponentController.RegisterComponent<ClockFacade>();
    }

    /// <summary>
    /// Register all required callbacks to the core builder.
    /// </summary>
    private static void RegisterCallbacks()
    {
        logger.Debug("Registering Callbacks...");
        ComponentController.RegisterComponent<OnUpdate>();
    }

    #endregion
}