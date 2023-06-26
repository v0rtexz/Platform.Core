#region Copyright (c) Platform

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

namespace Ensage.Data.Events;

/// <summary>
/// Responsible for the registering and erasing of events.
/// </summary>
public class EventManager
{
    #region Properties

    /// <summary>
    /// OnUpdate event
    /// </summary>
    private static event EventDelegate.EvtOnUpdate EvtOnUpdate;
    
    #endregion

    #region Methods

    /// <summary>
    /// Register a callback to a given event.
    /// </summary>
    /// <param name="subscriber">The function that will be executed when the event is raised.</param>
    /// <typeparam name="T">The type of the callback.</typeparam>
    public static void RegisterCallback<T>(Action subscriber)
    {
        switch (typeof(T))
        {
            case Type t when t == typeof(EventDelegate.EvtOnUpdate):
                EventDelegate.EvtOnUpdate handler = () => subscriber.Invoke();
                EvtOnUpdate += handler;
                break;
        }
    }

    /// <summary>
    /// Invoke the given callback type.
    /// </summary>
    /// <typeparam name="TCallback">The callback type to invoke.</typeparam>
    public static void InvokeCallback<TCallback>()
    {
        switch (typeof(TCallback))
        {
            case Type t when t == typeof(EventDelegate.EvtOnUpdate):
                EvtOnUpdate?.Invoke();
                break;
        }
    }

    #endregion

}