// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace DotNetNuke.Entities.Portals
{
    /// <summary>
    /// Manages reading and updating portal styles.
    /// </summary>
    public interface IPortalStylesController
    {
        /// <summary>
        /// Gets the portal styles for the given portal.
        /// </summary>
        /// <param name="color">Gets white or black depending on which contrasts the best against the provided color.</param>
        /// <returns>"000000" or "FFFFFF"</returns>
        string GetContrastColor(StyleColorBase color);

        /// <summary>
        /// Get the portal styles for a given portal.
        /// </summary>
        /// <param name="portalId">The id of the portal.</param>
        /// <returns><see cref="PortalStyles"/></returns>
        PortalStyles GetPortalStyles(int portalId);

        /// <summary>
        /// Updates the portal styles for a given portal.
        /// </summary>
        /// <param name="portalId">The portal id of the portal to update.</param>
        /// <param name="portalStyles"><see cref="PortalStyles"/></param>
        void UpdatePortalStyles(int portalId, PortalStyles portalStyles);
    }
}