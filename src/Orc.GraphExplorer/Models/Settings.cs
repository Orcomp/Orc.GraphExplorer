#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Models
{
    using Catel.Data;
    using Messages;
    using Services;

    public class Settings : ModelBase
    {
        public Settings(IConfigLocationService configLocationService)
        {
            ConfigLocation = configLocationService.Load();
            SettingsChangedMessage.SendWith(true);
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool IsSettingsVisible
        {
            get { return GetValue<bool>(IsSettingsVisibleProperty); }
            set { SetValue(IsSettingsVisibleProperty, value); }
        }

        /// <summary>
        /// Register the IsSettingsVisible property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IsSettingsVisibleProperty = RegisterProperty("IsSettingsVisible", typeof(bool), () => false);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public ConfigLocation ConfigLocation
        {
            get { return GetValue<ConfigLocation>(ConfigLocationProperty); }
            set { SetValue(ConfigLocationProperty, value); }
        }

        /// <summary>
        /// Register the ConfigLocation property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ConfigLocationProperty = RegisterProperty("ConfigLocation", typeof(ConfigLocation), null);
    }
}