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

        /// <summary>
        /// Gets or sets the portal styles.
        /// </summary>
        public PortalStyles Styles { get; set; }
    }
}