#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataLocationSettingsService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using Models;

    public interface IDataLocationSettingsService
    {
        #region Methods
        DataLocationSettings Load();
        DataLocationSettings GetCurrentOrLoad();

        void Save(DataLocationSettings dataLocationSettings);
        #endregion
    }
}