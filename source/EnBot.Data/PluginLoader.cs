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

using System.Reflection;
using Autofac;
using Ensage.Data.Container;
using Ensage.Data.Modules.Interfaces;
using JetBrains.Annotations;
using Serilog;

namespace Ensage.Data;

/// <summary>
/// Responsible for loading all external plugins.
/// </summary>
internal class PluginLoader : IDisposable
{
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginLoader"/> class.
    /// </summary>
    public PluginLoader()
    {
        this.Plugins = new List<IScript>();
    }

    #endregion

    #region Properties

    [NotNull] [ItemNotNull] internal List<IScript> Plugins { get; }

    #endregion

    #region Methods

    internal void LoadPlugins(bool isDeveloper = false)
    {
        if (!this.IsPluginDirectoryPresent())
            return;

        this.LoadAssembliesIntoDomain();

        Type interfaceType = typeof(IScript);
        Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
            .ToArray();

        foreach (Type type in types)
        {
            IScript? toLoad = null;
            // Create Plugin Instance out of the DLL.
            try
            {
                toLoad = Activator.CreateInstance(type) as IScript;
            }
            catch (NullReferenceException ex)
            {
                CoreContainer.Get().Resolve<ILogger>().Error("Failed to load Assembly.\n", ex);
                continue;
            }

            this.Plugins.Add(toLoad);
        }
    }

    /// <summary>
    /// Load all assemblys from the directory into the AppDomain.
    /// </summary>
    /// <exception cref="Exception">Exception is raised when the assembly loading fails.</exception>
    private void LoadAssembliesIntoDomain()
    {
        string[] files = Directory.GetFiles("Plugins");

        try
        {
            foreach (string file in files)
            {
                if (file.EndsWith(".dll"))
                {
                    var assembly = Assembly.LoadFrom(Path.GetFullPath(file));

                    // Change the working directory of the loaded assembly
                    Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("Failed to load the plugins: " + e.Message);
        }
    }

    /// <summary>
    /// Checks if the Plugins directory is present.
    /// </summary>
    /// <returns>True if the directory is present.</returns>
    /// <exception cref="DirectoryNotFoundException">Exception is thrown, if there was an issue with the IO.</exception>
    private bool IsPluginDirectoryPresent()
    {
        try
        {
            return Directory.Exists("Plugins");
        }
        catch (IOException e)
        {
            throw new IOException("Exception while checking for the plugins directory: " + e.Message);
        }
    }

    /// <summary>
    /// Clear all plugins on dispose.
    /// </summary>
    public void Dispose()
    {
        this.Plugins.Clear();
    }

    #endregion
}