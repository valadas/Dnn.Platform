// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using DotNetNuke.Collections;
using System;

namespace DotNetNuke.Entities.Portals
{
    public class PortalStylesController : IPortalStylesController
    {
        private const string PRIMARY_COLOR = "PrimaryColor";
        private const string PRIMARY_COLOR_LIGHT = "PrimaryColorLight";
        private const string PRIMARY_COLOR_DARK = "PrimaryColorDark";
        private const string PRIMARY_COLOR_CONTRAST = "PrimaryColorContrast";
        private const string SECONDARY_COLOR = "SecondaryColor";
        private const string SECONDARY_COLOR_LIGHT = "SecondaryColorLight";
        private const string SECONDARY_COLOR_DARK = "SecondaryColorDark";
        private const string SECONDARY_COLOR_CONTRAST = "SecondaryColorContrast";
        private const string TERTIARY_COLOR = "TertiaryColor";
        private const string TERTIARY_COLOR_LIGHT = "TertiaryColorLight";
        private const string TERTIARY_COLOR_DARK = "TertiaryColorDark";
        private const string TERTIARY_COLOR_CONTRAST = "TertiaryColorContrast";
        private const string CONTROLS_RADIUS = "ControlsRadius";

        /// <summary>
        /// Gets the portal styles for a given portalId.
        /// </summary>
        /// <param name="portalId">The id of the portal.</param>
        /// <returns><see cref="PortalStyles"/></returns>
        public PortalStyles GetPortalStyles(int portalId)
        {
            var settings = PortalController.Instance.GetPortalSettings(portalId);
            var portalStyles = new PortalStyles();

            portalStyles.PrimaryColor = new StyleColorBase(settings.GetValueOrDefault(PRIMARY_COLOR, "3792ED"));
            portalStyles.PrimaryColorLight = new StyleColorBase(settings.GetValueOrDefault(PRIMARY_COLOR_LIGHT, "6CB6F3"));
            portalStyles.PrimaryColorDark = new StyleColorBase(settings.GetValueOrDefault(PRIMARY_COLOR_DARK, "0D569E"));
            portalStyles.PrimaryColorContrast = new StyleColorBase(settings.GetValueOrDefault(PRIMARY_COLOR_CONTRAST, this.GetContrastColor(portalStyles.PrimaryColor)));

            portalStyles.SecondaryColor = new StyleColorBase(settings.GetValueOrDefault(SECONDARY_COLOR, "F5F5F5"));
            portalStyles.SecondaryColorLight = new StyleColorBase(settings.GetValueOrDefault(SECONDARY_COLOR_LIGHT, "FEFEFE"));
            portalStyles.SecondaryColorDark = new StyleColorBase(settings.GetValueOrDefault(SECONDARY_COLOR_DARK, "E8E8E8"));
            portalStyles.SecondaryColorContrast = new StyleColorBase(settings.GetValueOrDefault(SECONDARY_COLOR_CONTRAST, this.GetContrastColor(portalStyles.SecondaryColor)));

            portalStyles.TertiaryColor = new StyleColorBase(settings.GetValueOrDefault(TERTIARY_COLOR, "EAEAEA"));
            portalStyles.TertiaryColorLight = new StyleColorBase(settings.GetValueOrDefault(TERTIARY_COLOR_LIGHT, "F2F2F2"));
            portalStyles.TertiaryColorDark = new StyleColorBase(settings.GetValueOrDefault(TERTIARY_COLOR_DARK, "D8D8D8"));
            portalStyles.TertiaryColorContrast = new StyleColorBase(settings.GetValueOrDefault(TERTIARY_COLOR_CONTRAST, this.GetContrastColor(portalStyles.TertiaryColor)));

            portalStyles.ControlsRadius = settings.GetValueOrDefault(CONTROLS_RADIUS, 3);

            return portalStyles;
        }

        /// <summary>
        /// Updates the portal styles for a given portal.
        /// </summary>
        /// <param name="portalId">The id of the portal.</param>
        /// <param name="portalStyles">The <see cref="PortalStyles"/></param>
        public void UpdatePortalStyles(int portalId, PortalStyles portalStyles)
        {
            PortalController.Instance.UpdatePortalSetting(portalId, PRIMARY_COLOR, portalStyles.PrimaryColor.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, PRIMARY_COLOR_LIGHT, portalStyles.PrimaryColorLight.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, PRIMARY_COLOR_DARK, portalStyles.PrimaryColorDark.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, PRIMARY_COLOR_CONTRAST, portalStyles.PrimaryColorContrast.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, SECONDARY_COLOR, portalStyles.SecondaryColor.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, SECONDARY_COLOR_LIGHT, portalStyles.SecondaryColorLight.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, SECONDARY_COLOR_DARK, portalStyles.SecondaryColorDark.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, SECONDARY_COLOR_CONTRAST, portalStyles.SecondaryColorContrast.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, TERTIARY_COLOR, portalStyles.TertiaryColor.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, TERTIARY_COLOR_LIGHT, portalStyles.TertiaryColorLight.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, TERTIARY_COLOR_DARK, portalStyles.TertiaryColorDark.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, TERTIARY_COLOR_CONTRAST, portalStyles.TertiaryColorContrast.HexValue, false, null, false);
            PortalController.Instance.UpdatePortalSetting(portalId, CONTROLS_RADIUS, portalStyles.ControlsRadius.ToString(), true, null, false);
        }

        /// <summary>
        /// Gets white or black css color depending on which provides the most contrast against the base color.
        /// </summary>
        /// <remarks>
        /// Math based on, <see href="https://www.w3.org/TR/WCAG20/">WCAG 2.0 recommendations</see> and
        /// <see href="https://en.wikipedia.org/wiki/Luma_(video)#Rec._601_luma_versus_Rec._709_luma_coefficients">Rec. 601 luma versus Rec. 709 luma coefficients.</see>
        /// </remarks>
        /// <param name="color">The color to contrast against.</param>
        /// <returns>"000000" or "FFFFFF"</returns>
        public string GetContrastColor(StyleColorBase color)
        {
            var r = color.Red * 0.299d;
            var g = color.Green * 0.587d;
            var b = color.Blue * 0.114d;
            var total = r + g + b;
            string result = total > 186 ? "000000" : "FFFFFF";
            return result;
        }
    }
}
