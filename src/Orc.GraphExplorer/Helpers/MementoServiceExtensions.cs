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
    using Operations.Interfaces;

    using Orc.GraphExplorer.Messages;

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

        public static void Do(this IMementoService mementoService, IOperation operation)
        {
            operation.Do();
            mementoService.ClearRedoBatches();
            mementoService.Add(operation);
            if (string.IsNullOrEmpty(operation.Description))
            {
                return;
            }

            if (operation.Description.Length < 2)
            {
                StatusMessage.SendWith(operation.Description);
            }

            var firstLetter = operation.Description.Substring(0, 1);
            var lastPart = operation.Description.Substring(1);
            StatusMessage.SendWith(firstLetter.ToUpper() + lastPart);
        }
    }
}