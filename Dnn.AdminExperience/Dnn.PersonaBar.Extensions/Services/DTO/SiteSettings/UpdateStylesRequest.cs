// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using DotNetNuke.Entities.Portals;

namespace Dnn.PersonaBar.Extensions.Services.Dto.SiteSettings
{
    public class UpdateStylesRequest
    {
        /// <summary>
        /// Gets or sets the portal id.
        /// </summary>
        public int? PortalId { get; set; }

        public string PrimaryColor { get; set; }
        public string PrimaryColorLight { get; set; }
        public string PrimaryColorDark { get; set; }
        public string PrimaryColorContrast { get; set; }
        public string SecondaryColor { get; set; }
        public string SecondaryColorLight { get; set; }
        public string SecondaryColorDark { get; set; }
        public string SecondaryColorContrast { get; set; }
        public string TertiaryColor { get; set; }
        public string TertiaryColorLight { get; set; }
        public string TertiaryColorDark { get; set; }
        public string TertiaryColorContrast { get; set; }
        public int ControlsRadius { get; set; }
    }
}