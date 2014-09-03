#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphExplorerView.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Views.Interfaces
{
    using Catel.MVVM.Views;

    using Orc.GraphExplorer.Views.Enums;

    public interface IGraphExplorerView : IView
    {
        void ShowAllEdgesLabels(GraphExplorerTab tab,bool show);

        void FitToBounds(GraphExplorerTab tab);
    }
}