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

namespace Platform.Data.Events;

using JetBrains.Annotations;

using Serilog;

/// <summary>
/// The OnUpdate callback should be triggered every tick.
/// </summary>
public class OnUpdate : ICallback
{
    [NotNull]
    private ILogger logger;
    
    public OnUpdate(ILogger logger)
    {
        this.logger = logger;
       this.InstantiateCallback();
    }

    /// <summary>
    /// Triggered every tick.
    /// </summary>
    public void TriggerIfConditionMet()
    {
       // logger.Debug("OnUpdate Triggered!");
    }

    public void InstantiateCallback()
    {
        EventManager.RegisterCallback<EventDelegate.EvtOnUpdate>(() => TriggerIfConditionMet());
    }
}