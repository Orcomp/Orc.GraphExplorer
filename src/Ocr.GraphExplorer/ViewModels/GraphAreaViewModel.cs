#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphAreaViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;

    public class GraphAreaViewModel : ViewModelBase
    {
        public GraphAreaViewModel()
        {
            Area = new GraphArea();    
        }

        protected override void Initialize()
        {
            base.Initialize();
            Area.CreateGraphArea(600);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [Model]
        public GraphArea Area
        {
            get { return GetValue<GraphArea>(EditorProperty); }
            private set { SetValue(EditorProperty, value); }
        }

        /// <summary>
        /// Register the Area property so it is known in the class.
        /// </summary>
        public static readonly PropertyData EditorProperty = RegisterProperty("Area", typeof(GraphArea));

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Area")]
        public GraphLogic Logic
        {
            get { return GetValue<GraphLogic>(LogicProperty); }
            set { SetValue(LogicProperty, value); }
        }

        /// <summary>
        /// Register the Logic property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LogicProperty = RegisterProperty("Logic", typeof(GraphLogic));
    }
}