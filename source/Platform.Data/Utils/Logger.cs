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

using Platform.Data.Container;

namespace Platform.Data.Utils;

using Autofac;
using Platform.Data.Container;
using Serilog;
using Serilog.Events;

/// <summary>
/// Logger class to implement the functionality of the logger.
/// </summary>
public class Logger : ILogger
{
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Logger"/> class.
    /// </summary>
    public Logger()
    {
        this.logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();
    }

    #endregion

    #region Properties

    private ILogger logger;

    #endregion

    #region Methods

    public void Write(LogEvent logEvent)
    {
        this.logger.Write(logEvent.Level, logEvent.MessageTemplate.Text);
    }

    public void Write(LogEventLevel level, string msg)
    {
        this.logger.Write(level, msg);
    }

    public void Write<T0>(LogEventLevel level, string msg, T0 propertyValue)
    {
        this.logger.Write(level, msg, propertyValue);
    }

    public void Write<T0, T1>(LogEventLevel level, string msg, T0 propertyValue, T1 propertyValue2)
    {
        this.logger.Write(level, msg, propertyValue, propertyValue2);
    }

    public void Write<T0, T1, T3>(LogEventLevel level, string msg, T0 propertyValue, T1 propertyValue2, T3 propertyValue3)
    {
        this.logger.Write(level, msg, propertyValue, propertyValue2, propertyValue3);
    }

    /// <summary>
    /// Get the logger instance.
    /// </summary>
    /// <returns>Returns the <see cref="Logger"/> instance.</returns>
    public static ILogger Get()
    {
        return CoreContainer.Get().Resolve<ILogger>();
    }

    #endregion
}
