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

using Ensage.Data.Events.Args;
using JetBrains.Annotations;

namespace Ensage.Data.Events;

/// <summary>
/// Responsible for the registering and erasing of events.
/// </summary>
public static class EventManager
{
    private static Dictionary<short, Delegate> eventDictionary;

    public enum EventId : short
    {
        OnTick = 1
    }

    static EventManager()
    {
        eventDictionary = new Dictionary<short, Delegate>();
    }

    public static void RegisterEvent(short eventId, Action action)
    {
        if (eventDictionary.ContainsKey(eventId))
        {
            eventDictionary[eventId] = (Action)eventDictionary[eventId] + action;
        }
        else
        {
            eventDictionary[eventId] = action;
        }
    }

    public static void RegisterEvent<T>(short eventId, Action<T> action)
    {
        if (eventDictionary.ContainsKey(eventId))
        {
            eventDictionary[eventId] = (Action<T>)eventDictionary[eventId] + action;
        }
        else
        {
            eventDictionary[eventId] = action;
        }
    }

    public static void UnregisterEvent(short eventId, Action action)
    {
        if (eventDictionary.ContainsKey(eventId))
        {
            eventDictionary[eventId] = (Action)eventDictionary[eventId] - action;
        }
    }

    public static void UnregisterEvent<T>(short eventId, Action<T> action)
    {
        if (eventDictionary.ContainsKey(eventId))
        {
            eventDictionary[eventId] = (Action<T>)eventDictionary[eventId] - action;
        }
    }

    public static void ExecuteEvent(short eventId)
    {
        if (eventDictionary.TryGetValue(eventId, out var eventDelegate) && eventDelegate is Action action)
        {
            action();
        }
    }

    public static void ExecuteEvent<T>(short eventId, T parameter)
    {
        if (eventDictionary.TryGetValue(eventId, out var eventDelegate) && eventDelegate is Action<T> action)
        {
            action(parameter);
        }
    }
}