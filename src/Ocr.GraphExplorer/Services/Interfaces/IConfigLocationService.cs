#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilePickerService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services.Interfaces
{
    using Orc.GraphExplorer.Models;

    public interface IConfigLocationService
    {
        ConfigLocation Load();

        void Save(ConfigLocation configLocation);
/*        string ChangeRelationshipsFileLocation();*/
    }
}