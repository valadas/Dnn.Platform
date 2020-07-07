// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.UI.Skins.Controls
{
    using System;
    using System.Linq;
    using System.Web.UI.WebControls;
    using System.Xml.Linq;
    using DotNetNuke.Abstractions;
    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Services.Cache;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.FileSystem;
    using Microsoft.Extensions.DependencyInjection;

    /// -----------------------------------------------------------------------------
    /// <summary></summary>
    /// <returns></returns>
    /// <remarks></remarks>
    /// -----------------------------------------------------------------------------
    public partial class Logo : SkinObjectBase
    {
        private readonly INavigationManager _navigationManager;
        private readonly string _cacheKey;

        public Logo()
        {
            this._navigationManager = Globals.DependencyProvider.GetRequiredService<INavigationManager>();
            this._cacheKey = string.Format(DataCache.PortalCacheKey, this.PortalSettings.PortalId, this.PortalSettings.CultureCode) + "LogoSvg";
        }

        public string BorderWidth { get; set; }

        public string CssClass { get; set; }

        public Nullable<bool> InjectSVG { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                if (!string.IsNullOrEmpty(this.PortalSettings.LogoFile))
                {
                    var fileInfo = this.GetLogoFileInfo();
                    if (fileInfo != null)
                    {
                        if (this.InjectSVG.GetValueOrDefault() == true) // try to load as <svg>
                        {
                            imgLogo.Visible = false;
                            hypLogo.CssClass = this.CssClass;

                            string svg = (string)CachingProvider.Instance().GetItem(this._cacheKey);
                            if (string.IsNullOrEmpty(svg))
                            {
                                if (fileInfo.Extension == "svg")
                                {
                                    try
                                    {
                                        XDocument svgXmlDoc = XDocument.Load(fileInfo.PhysicalPath);
                                        XElement svgXmlNode = svgXmlDoc.Descendants().Where(x => x.Name.LocalName == "svg").SingleOrDefault();

                                        if (svgXmlNode != null)
                                        {
                                            svg = svgXmlNode.ToString();
                                            CachingProvider.Instance().Insert(this._cacheKey, svg);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        this.hypLogo.Text = this.PortalSettings.PortalName;
                                    }
                                }
                                else
                                {
                                    this.hypLogo.Text = this.PortalSettings.PortalName;
                                }
                            }

                            if (!string.IsNullOrEmpty(svg))
                            {
                                Literal litSvg = new Literal();
                                litSvg.Text = svg;
                                this.hypLogo.Controls.Add(litSvg);
                            }
                        }
                        else // try to load as <img>
                        {
                            imgLogo.Visible = true;

                            string imageUrl = FileManager.Instance.GetUrl(fileInfo);
                            if (!string.IsNullOrEmpty(imageUrl))
                            {
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
                    else
                    {
                        this.hypLogo.Text = this.PortalSettings.PortalName;
                    }
                }

                this.hypLogo.ToolTip = this.PortalSettings.PortalName;
                this.hypLogo.Attributes.Add("aria-label", this.PortalSettings.PortalName);
                if (this.PortalSettings.HomeTabId != -1)
                {
                    this.hypLogo.NavigateUrl = this._navigationManager.NavigateURL(this.PortalSettings.HomeTabId);
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
