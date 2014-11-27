#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer
{
    using System.Linq;

    using Catel;

    public static class StringExtensions
    {
        public static bool IsInteger(this string str)
        {
            Argument.IsNotNull(() => str);

            return !string.IsNullOrEmpty(str) && str.All(c => "0123456789".Contains(c));
        }
    }
}