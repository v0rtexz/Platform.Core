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

namespace Platform.Data.Exceptions;

using Platform.Data.Container;

using Autofac;

using Serilog;

internal class MemoryException : Exception
{
    public MemoryException(string message)
      : base(message)
    {
        if (CoreContainer.Get() == null)
            Console.WriteLine(message);
        else
            CoreContainer.Get().Resolve<ILogger>().Fatal(message);
    }

    public MemoryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}