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

using Autofac;
using Ensage.Data.Container;
using Ensage.Data.Utils;

namespace Ensage.Data.Game.Components;

/// <summary>
/// Accessor for all <see cref="IGameComponent"/>.
/// </summary>
public static class ComponentController
{

    /// <summary>
    /// Registers a new component.
    /// </summary>
    /// <typeparam name="T">The component type to register.</typeparam>
    /// <param name="singleton">True if it should be registered as SingleInstance.</param>
    /// <param name="externallyAllowed">True if it should be registered under ExternallyAllowed.</param>
    /// /// <param name="autoActivate">True if the type should be automatically activated.s</param>
    /// <returns></returns>
    internal static OperationResult RegisterComponent<T>(bool singleton = true, bool externallyAllowed = false, bool autoActivate = false)
        where T : notnull
    {
        var type = CoreContainer.GetBuilder().RegisterType<T>().As<T>();

        if (singleton)
        {
            type.SingleInstance();
        }

        if (externallyAllowed)
        {
            type.ExternallyOwned();
        }

        if (autoActivate)
        {
            type.AutoActivate();
        }


        return type != null ? OperationResult.SUCCESS : OperationResult.FAILURE;
    }

    /// <summary>
    /// Get the instance of the given class.
    /// </summary>
    /// <typeparam name="T">The instance type to get.</typeparam>
    /// <returns>The instance of the component.</returns>
    internal static T GetComponent<T>()
        where T : notnull
    {
        return (CoreContainer.Get() ?? throw new InvalidOperationException()).Resolve<T>();
    }
}