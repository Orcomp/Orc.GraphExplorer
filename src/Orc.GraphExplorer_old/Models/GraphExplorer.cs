#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphExplorer.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using Catel.Data;
    using Catel.IoC;

    using Orc.GraphExplorer.Services.Interfaces;

    public class GraphExplorer : ModelBase
    {
        private readonly IOperationObserver _operationObserver;

        public GraphExplorer()
        {
            var serviceLocator = ServiceLocator.Default; 
            _operationObserver = serviceLocator.ResolveType<IOperationObserver>();
   
        }


        public IOperationObserver OperationObserver
        {
            get { return _operationObserver; }
        }
    }
}