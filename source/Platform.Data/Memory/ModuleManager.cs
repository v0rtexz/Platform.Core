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
using System.Diagnostics.CodeAnalysis;
using Serilog;

namespace Platform.Data.Memory;

public class ModuleManager
{
    #region Properties

    /// <summary>
    /// Module name, base address
    /// </summary>
    private Dictionary<string, IntPtr> moduleList;

    [NotNull] private ILogger logger;

    [NotNull] private Memory memory;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleManager"/> class.
    /// </summary>
    /// <param name="logger">Injected logger instance.</param>
    /// <param name="memory">Injected Memory instance.</param>
    public ModuleManager([NotNull] ILogger logger, [NotNull] Memory memory)
    {
        this.logger = logger;
        this.memory = memory;

        this.moduleList = new Dictionary<string, IntPtr>();

        // Initialize all modules

        this.InitModule("OWClient.dll");
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get the base address of a certain module.
    /// </summary>
    /// <param name="name">The module to get the base address from.</param>
    /// <returns>The base address of the module.</returns>
    public IntPtr GetModule(string name)
    {
        try
        {
            return moduleList[name];
        }
        catch (KeyNotFoundException e)
        {
            logger.Fatal($"Couldn't get the module: {name}");
        }

        return IntPtr.Zero;
    }

    /// <summary>
    /// Init the given module.
    /// </summary>
    /// <param name="moduleName">The module name to init.</param>
    private bool InitModule(string moduleName)
    {
        foreach (ProcessModule module in Process.GetProcessById(memory.ProcessID).Modules)
        {
            //   Console.WriteLine(module.ModuleName);
            if (module.ModuleName.Equals(moduleName))
            {
                moduleList[module.ModuleName] = module.BaseAddress;
                return true;
            }
        }

        logger.Warning($"Couldn't Init the module: {moduleName}");
        return false;
    }

    #endregion
}