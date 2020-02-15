// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace DotNetNuke.Entities.Portals
{
    /// <summary>
    /// Provides css custom properties to customize the Dnn UI in the platform, themes and modules for an overall consistent look.
    /// </summary>
    public class PortalStyles
    {
        /// <summary>
        /// Gets or sets the portal primary color.
        /// </summary>
        public StyleColorBase PrimaryColor { get; set; }
        
        /// <summary>
        /// Gets or sets a darker version of the primary color.
        /// </summary>
        public StyleColorBase PrimaryColorDark { get; set; }

        /// <summary>
        /// Gets or sets a lighter version of the primary color.
        /// </summary>
        public StyleColorBase PrimaryColorLight { get; set; }


        /// <summary>
        /// Gets or sets a color that has great contrast against the primary color.
        /// </summary>
        public StyleColorBase PrimaryColorContrast { get; set; }

        /// <summary>
        /// Gets or sets the portal secondary color.
        /// </summary>
        public StyleColorBase SecondaryColor { get; set; }

        /// <summary>
        /// Gets or sets a darker version of the secondary color.
        /// </summary>
        public StyleColorBase SecondaryColorDark { get; set; }

        /// <summary>
        /// Gets or sets a lighter version of the secondary color.
        /// </summary>
        public StyleColorBase SecondaryColorLight { get; set; }

        /// <summary>
        /// Gets or sets a color that has great contrast against the secondary color.
        /// </summary>
        public StyleColorBase SecondaryColorContrast { get; set; }

        /// <summary>
        /// Gets or sets the portal tertiary color.
        /// </summary>
        public StyleColorBase TertiaryColor { get; set; }

        /// <summary>
        /// Gets or sets a darker version of the tertiary color.
        /// </summary>
        public StyleColorBase TertiaryColorDark { get; set; }

        /// <summary>
        /// Gets or sets a lighter version of the tertiary color.
        /// </summary>
        public StyleColorBase TertiaryColorLight { get; set; }

        /// <summary>
        /// Gets or sets a color that has great contrast against the tertiary color.
        /// </summary>
        public StyleColorBase TertiaryColorContrast { get; set; }

        /// <summary>
        /// Gets or sets the radius for most controls.
        /// </summary>
        public int ControlsRadius { get; set; }
    }
}
