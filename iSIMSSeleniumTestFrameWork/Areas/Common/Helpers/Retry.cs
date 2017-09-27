using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedComponents.Helpers
{
    /// <summary>
    /// A contraining helpers for "Retry" implementation.
    /// </summary>
    public static class Retry
    {
        public static void Do(
            Action action,
            int retryInterval = 100,
            int retryCount = 20,
            Action catchAction = null)
        {
            if (catchAction == null)
            {
                Do<object>(() =>
                {
                    action();
                    return null;
                }, retryInterval, retryCount);
            }
            else
            {
                Do<object>(
                    () =>
                    {
                        action();
                        return null;
                    }, 
                    retryInterval, 
                    retryCount,
                    catchAction
                );
            }
        }

        public static T Do<T>(
            Func<T> action,
            int retryInterval = 100,
            int retryCount = 20,
            Action catchAction = null)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine("RETRY: {0} --- {1}", ex.Message, ex.GetType().Name);
                    exceptions.Add(ex);
                    if (catchAction != null)
                    {
                        catchAction();
                        Debug.WriteLine("*** CATCH ACTION ***");
                    }
                    Thread.Sleep(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}
