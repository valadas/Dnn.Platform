// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.UI.Skins.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using System.Xml.Linq;

    using DotNetNuke.Abstractions;
    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Services.Cache;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.FileSystem;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Logo SkinObject.
    /// </summary>
    public partial class Logo : SkinObjectBase
    {
        private readonly INavigationManager navigationManager;
        private readonly string svgCacheKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logo"/> class.
        /// </summary>
        public Logo()
        {
            this.navigationManager = Globals.DependencyProvider.GetRequiredService<INavigationManager>();
            this.svgCacheKey = string.Format(DataCache.PortalCacheKey, this.PortalSettings.PortalId, this.PortalSettings.CultureCode) + "LogoSvg";
        }

        /// <summary>
        /// Gets or sets the width of the border around the image.
        /// </summary>
        public string BorderWidth { get; set; }

        /// <summary>
        /// Gets or sets the css class for the image.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wether to inject the svg content inline instead or wrapping it in an img tag.
        /// </summary>
        public bool? InjectSVG { get; set; }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                this.imgLogo.Visible = false;

                if (!string.IsNullOrEmpty(this.PortalSettings.LogoFile))
                {
                    // default value in case we can't load an image or svg
                    this.litLogo.Text = this.PortalSettings.PortalName;

                    var fileInfo = this.GetLogoFileInfo();
                    if (fileInfo != null)
                    {
                        if (this.InjectSVG.GetValueOrDefault() && fileInfo.Extension == "svg")
                        {
                            // svg injection was requested and we have an svg file.
                            this.imgLogo.Visible = false;
                            string svg = DataCache.GetCache<string>(this.svgCacheKey);
                            var svgXmlDoc = new XDocument();
                            if (string.IsNullOrEmpty(svg))
                            {
                                using (var fileContent = FileManager.Instance.GetFileContent(fileInfo))
                                {
                                    svgXmlDoc = XDocument.Load(fileContent);
                                }

                                var svgXmlNode = svgXmlDoc.Descendants().Where(x => x.Name.LocalName == "svg").SingleOrDefault();

                                if (svgXmlNode == null)
                                {
                                    throw new InvalidFileContentException("Invalid svg file.");
                                }

                                var ns = svgXmlNode.GetDefaultNamespace();

                                if (!string.IsNullOrEmpty(this.CssClass))
                                {
                                    // Append the css class.
                                    List<string> classList = new List<string>();
                                    classList.Add(this.CssClass);

                                    if (svgXmlNode.Attribute("class") != null)
                                    {
                                        classList.AddRange(svgXmlNode.Attribute("class").Value.Split(' '));
                                    }

                                    svgXmlNode.SetAttributeValue("class", string.Join(" ", classList.ToArray()));
                                }

                                if (svgXmlNode.Descendants().FirstOrDefault(x => x.Name.LocalName == "title") == null)
                                {
                                    // Add the title for ADA compliance.
                                    var titleNode = new XElement(
                                        ns + "title",
                                        new XAttribute("id", this.litLogo.UniqueID),
                                        this.PortalSettings.PortalName);

                                    svgXmlNode.AddFirst(new XElement(titleNode));

                                    // Link the title to the svg node.
                                    svgXmlNode.SetAttributeValue("aria-labelledby", this.litLogo.UniqueID);
                                }

                                // Ensure we have the image role for ADA Compliance
                                svgXmlNode.SetAttributeValue("role", "img");

                                svg = svgXmlNode.ToString();
                                DataCache.SetCache(this.svgCacheKey, svg);
                            }

                            this.litLogo.Text = svg;
                        }
                        else
                        {
                            // display the raster image
                            this.imgLogo.Visible = true;

                            string imageUrl = FileManager.Instance.GetUrl(fileInfo);
                            if (!string.IsNullOrEmpty(imageUrl))
                            {
                                this.litLogo.Visible = false;
                                this.imgLogo.ImageUrl = imageUrl;
                            }

                            if (!string.IsNullOrEmpty(this.BorderWidth))
                            {
                                this.imgLogo.BorderWidth = Unit.Parse(this.BorderWidth);
                            }

                            if (!string.IsNullOrEmpty(this.CssClass))
                            {
                                this.imgLogo.CssClass = this.CssClass;
                            }

                            this.imgLogo.AlternateText = this.PortalSettings.PortalName;
                        }
                    }
                }

                this.hypLogo.ToolTip = this.PortalSettings.PortalName;
                this.hypLogo.Attributes.Add("aria-label", this.PortalSettings.PortalName);
                if (this.PortalSettings.HomeTabId != -1)
                {
                    this.hypLogo.NavigateUrl = this.navigationManager.NavigateURL(this.PortalSettings.HomeTabId);
                }
                else
                {
                    this.hypLogo.NavigateUrl = Globals.AddHTTP(this.PortalSettings.PortalAlias.HTTPAlias);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private IFileInfo GetLogoFileInfo()
        {
            string cacheKey = string.Format(DataCache.PortalCacheKey, this.PortalSettings.PortalId, this.PortalSettings.CultureCode) + "LogoFile";
            var file = CBO.GetCachedObject<FileInfo>(
                new CacheItemArgs(cacheKey, DataCache.PortalCacheTimeOut, DataCache.PortalCachePriority),
                this.GetLogoFileInfoCallBack);

            return file;
        }

        private IFileInfo GetLogoFileInfoCallBack(CacheItemArgs itemArgs)
        {
            return FileManager.Instance.GetFile(this.PortalSettings.PortalId, this.PortalSettings.LogoFile);
        }
    }
}
