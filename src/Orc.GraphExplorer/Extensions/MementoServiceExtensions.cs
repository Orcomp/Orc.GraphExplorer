#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="MementoServiceExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer
{
    using System.Linq;
    using Catel.Memento;
    using Operations;
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
            var description = operation.Description;
            if (string.IsNullOrEmpty(description))
            {
                return;
            }

            if (description.Length < 2)
            {
                StatusMessage.SendWith(description);
            }

            var firstLetter = description.Substring(0, 1);
            var lastPart = description.Substring(1);
            StatusMessage.SendWith(firstLetter.ToUpper() + lastPart);
        }
    }
}