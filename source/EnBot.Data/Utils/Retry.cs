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

namespace Ensage.Data.Utils;

/// <summary>
/// Provides the functionality to retry a given functionality.
/// </summary>
public static class Retry
{
    /// <summary>
    /// Retries a given action.
    /// </summary>
    /// <param name="action">The action to retry.</param>
    /// <param name="retryInterval">The interval if retrying.</param>
    /// <param name="maxAttemptCount">The maximum attempts.</param>
    public static void Do(
        Action action,
        TimeSpan retryInterval,
        int maxAttemptCount = 3) => Do<object>(
            () =>
        {
            action();
            return null;
        }, retryInterval, maxAttemptCount);

    public static T Do<T>(
        Func<T> action,
        TimeSpan retryInterval,
        int maxAttemptCount = 3)
    {
        var exceptions = new List<Exception>();

        for (int attempted = 0; attempted < maxAttemptCount; attempted++)
        {
            try
            {
                if (attempted > 0)
                {
                    Thread.Sleep(retryInterval);
                }
                return action();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        throw new AggregateException(exceptions);
    }

    public static OperationResult Do(Func<OperationResult> function, TimeSpan retryInterval, int retryCount = 3)
    {
        int currentRetry = 0;
        OperationResult result = OperationResult.FAILURE;

        while (currentRetry < retryCount && result == OperationResult.FAILURE)
        {
            result = function.Invoke();
            if (result == OperationResult.FAILURE)
            {
                Thread.Sleep(retryInterval);
                currentRetry++;
            }
        }

        return result;
    }
}