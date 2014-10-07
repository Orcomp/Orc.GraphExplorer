#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="MementoServiceExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Helpers
{
    using System.Linq;
    using Catel.Memento;

    public static class MementoServiceExtensions
    {
        public static void ClearRedoBatches(this IMementoService mementoService)
        {
            if (mementoService.CanRedo)
            {
                var mementoBatches = mementoService.UndoBatches.ToArray();
                mementoService.Clear();

                for (var index = mementoBatches.Length - 1; index >= 0; index--)
                {
                    var mementoBatch = mementoBatches[index];
                    mementoService.Add(mementoBatch);
                }
            }
        }
    }
}