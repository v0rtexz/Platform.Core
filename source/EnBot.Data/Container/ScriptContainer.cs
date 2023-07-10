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
using Autofac;
using Ensage.Data.Utils;
using Serilog;
using Serilog.Events;

namespace Ensage.Data.Container;

/// <summary>
/// Class which contains the functionality of container and script building.
/// </summary>
public static class ScriptContainer
{
    #region Properties

    /// <summary>
    /// The built container
    /// </summary>
    [NotNull]
    private static IContainer? container;

    /// <summary>
    /// The lifetime scope of the container
    /// </summary>
    [NotNull]
    private static ILifetimeScope? scope;

    /// <summary>
    /// The builder of the container.
    /// </summary>
    [NotNull]
    private static ContainerBuilder? containerBuilder;

    #endregion

    #region Methods

    /// <summary>
    /// Build and run the Container and initialize the scope.
    /// </summary>
    public static OperationResult Build()
    {
        var logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();

        logger.Write(LogEventLevel.Information, "--------------------------------------Script Container--------------------------------------");

        logger.Write(LogEventLevel.Information, "Loading {0}", "Plugins");
        using PluginLoader pluginLoader = new PluginLoader();

        pluginLoader.LoadPlugins();

        containerBuilder = new ContainerBuilder();

        // Register the Logger
        containerBuilder.RegisterType<Logger>().As<ILogger>().SingleInstance().ExternallyOwned();

        logger.Write(LogEventLevel.Information, "Registering {0}", "Plugins");
        // Register types for external plugins
        foreach (var plugin in pluginLoader.Plugins)
        {
            logger.Write(LogEventLevel.Information, "Loading Plugin: {0}", plugin.Name);
            containerBuilder.RegisterType(plugin.GetType()).SingleInstance().ExternallyOwned();
        }

        // Build the containers
        logger.Write(LogEventLevel.Debug, "Building Container...");
        container = containerBuilder.Build();

        logger.Write(LogEventLevel.Debug, "Initiating Lifetime Scope...");
        scope = container.BeginLifetimeScope();

        // Construct every Plugin
        foreach (var plugin in pluginLoader.Plugins)
        {
            scope.Resolve(plugin.GetType());
        }

        return scope != null ? OperationResult.SUCCESS : OperationResult.FAILURE;
    }

    /// <summary>
    /// Get the Instance of the initialized <see cref="ILifetimeScope"/> Instance.
    /// </summary>
    /// <returns>The Instance.</returns>
    public static ILifetimeScope Get()
    {
        return scope;
    }

    /// <summary>
    /// Registers a Module.
    /// </summary>
    /// <typeparam name="T">The object type to Initialize.</typeparam>
    internal static void RegisterModule<T>()
        where T : notnull
    {
        containerBuilder.RegisterType<T>();
    }

    #endregion
}
