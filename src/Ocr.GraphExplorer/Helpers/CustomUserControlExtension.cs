namespace Orc.GraphExplorer.Helpers
{
    using System;

    using Catel.MVVM.Views;

    public static class CustomUserControlExtension
    {
        public static void InvokeEvent<T>(this IUserControl userControl, EventHandler<T> handler, T args) where T : EventArgs
        {
            if (handler != null)
            {
                handler(userControl, args);
            }
        }
    }
}