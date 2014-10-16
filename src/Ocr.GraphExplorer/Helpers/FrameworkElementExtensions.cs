#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Helpers
{
    using System.Windows;

    public static class FrameworkElementExtensions
    {
        public static T FindFirstParentOfType<T>(this FrameworkElement currentElement) where T : FrameworkElement
        {
            var parent = currentElement.Parent as FrameworkElement;
            if (parent == null)
            {
                return null;
            }

            var parentOfTypeT = parent as T;
            if (parentOfTypeT != null)
            {
                return parentOfTypeT;
            }

            return parent.FindFirstParentOfType<T>();
        }
    }
}