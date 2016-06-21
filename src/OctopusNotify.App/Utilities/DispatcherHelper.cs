using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace OctopusNotify.App
{
    public static class DispatcherHelper
    {
        internal static void Run(Action action)
        {
            Run(Application.Current.Dispatcher, action);
        }

        internal static void Run<T>(Action<T> action, T param)
        {
            Run(Application.Current.Dispatcher, action, param);
        }

        internal static void Run(Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(action);
            }

        }

        internal static void Run<T>(Dispatcher dispatcher, Action<T> action, T param)
        {
            if (dispatcher.CheckAccess())
            {
                action(param);
            }
            else
            {
                dispatcher.BeginInvoke(action, param);
            }

        }

        internal static void RunEventHandler<T>(Action<object, T> action, object sender, T e)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (dispatcher.CheckAccess())
            {
                action(sender, e);
            }
            else
            {
                dispatcher.BeginInvoke(action, sender, e);
            }
        }
    }
}
