#region Copyright

// 
// DotNetNuke® - https://www.dnnsoftware.com
// Copyright (c) 2002-2018
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

#region Usings

using System;
using System.Globalization;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Personalization;
using DotNetNuke.Services.Tokens;
using DotNetNuke.Common;
using DotNetNuke.Abstractions.Portals;

#endregion

namespace DotNetNuke.Entities.Portals
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The PortalSettings class encapsulates all of the settings for the Portal, 
    /// as well as the configuration settings required to execute the current tab
    /// view within the portal.
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable]
    public partial class PortalSettings : BaseEntityInfo, IPropertyAccess, IPortalSettings
    {
        #region ControlPanelPermission enum

        public enum ControlPanelPermission
        {
            TabEditor,
            ModuleEditor
        }

        #endregion

        #region Mode enum

        public enum Mode
        {
            View,
            Edit,
            Layout
        }

        #endregion

        #region PortalAliasMapping enum

        public enum PortalAliasMapping
        {
            None,
            CanonicalUrl,
            Redirect
        }

        #endregion

        #region Data Consent UserDeleteAction enum
        public enum UserDeleteAction
        {
            Off = 0,
            Manual = 1,
            DelayedHardDelete = 2,
            HardDelete = 3
        }
        #endregion

        private TimeZoneInfo _timeZone = TimeZoneInfo.Local;

        #region Constructors

        public PortalSettings()
        {
            Registration = new RegistrationSettings();
        }

        public PortalSettings(int portalId)
            : this(Null.NullInteger, portalId)
        {
        }

        public PortalSettings(int tabId, int portalId)
        {
            PortalId = portalId;
            var portal = PortalController.Instance.GetPortal(portalId);
            BuildPortalSettings(tabId, portal);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The PortalSettings Constructor encapsulates all of the logic
        /// necessary to obtain configuration settings necessary to render
        /// a Portal Tab view for a given request.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ///	<param name="tabId">The current tab</param>
        ///	<param name="portalAliasInfo">The current portal</param>
        /// -----------------------------------------------------------------------------
        public PortalSettings(int tabId, PortalAliasInfo portalAliasInfo)
        {
            PortalId = portalAliasInfo.PortalID;
            PortalAlias = portalAliasInfo;
            var portal = string.IsNullOrEmpty(portalAliasInfo.CultureCode) ?
                            PortalController.Instance.GetPortal(portalAliasInfo.PortalID)
                            : PortalController.Instance.GetPortal(portalAliasInfo.PortalID, portalAliasInfo.CultureCode);

            BuildPortalSettings(tabId, portal);
        }

        public PortalSettings(PortalInfo portal)
            : this(Null.NullInteger, portal)
        {
        }

        public PortalSettings(int tabId, PortalInfo portal)
        {
            PortalId = portal != null ? portal.PortalID : Null.NullInteger;
            BuildPortalSettings(tabId, portal);
        }

        private void BuildPortalSettings(int tabId, PortalInfo portal)
        {
            PortalSettingsController.Instance().LoadPortalSettings(this);

            if (portal == null) return;

            PortalSettingsController.Instance().LoadPortal(portal, this);

            var key = string.Join(":", "ActiveTab", portal.PortalID.ToString(), tabId.ToString());
            var items = HttpContext.Current != null ? HttpContext.Current.Items : null;
            if (items != null && items.Contains(key))
            {
                ActiveTab = items[key] as TabInfo;
            }
            else
            {
                ActiveTab = PortalSettingsController.Instance().GetActiveTab(tabId, this);
                if (items != null && ActiveTab != null)
                {
                    items[key] = ActiveTab;
                }
            }
        }

        #endregion

        #region Auto-Properties
        /// <summary>
        /// Gets or sets the ID of the active tab (page)
        /// </summary>
        public TabInfo ActiveTab { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user that is the portal primary administrator (owner).
        /// </summary>
		public int AdministratorId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the role that represents administrators for this portal.
        /// </summary>
		public int AdministratorRoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role that represents administrators for this portal.
        /// </summary>
		public string AdministratorRoleName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the tab (page) that contains the administration modules.
        /// </summary>
		public int AdminTabId { get; set; }

        /// <summary>
        /// Gets or sets a file that can optionnaly be used as a background image if the theme supports it.
        /// </summary>
		public string BackgroundFile { get; set; }

        /// <summary>
        /// Gets or sets a value that can be used by the Vendors/Banner module, there is no built-in UI to manage this setting.
        /// </summary>
		public int BannerAdvertising { get; set; }

        /// <summary>
        /// Gets or sets the culture code in a format that represents the language and contry as in "fr-CA" for French Canada.
        /// </summary>
		public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the currency used for this portal, in the platform this is used for Paypal subscriptions.
        /// </summary>
		public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the culture code of the default language for this portal.
        /// </summary>
		public string DefaultLanguage { get; set; }

        /// <summary>
        /// Gets or sets the portal description, used for SEO purposes and RSS feeds.
        /// </summary>
		public string Description { get; set; }

        /// <summary>
        /// Gets or sets the portal owner email address.
        /// </summary>
		public string Email { get; set; }

        /// <summary>
        /// Gets or sets when the portal will expire (stop loading).
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets a configurable text that can be injected by the theme, usually used for copyright notice and consumed by the copyright skinObject.
        /// [year] in this text will get replaced by the current year when used with the skinObject.
        /// </summary>
		public string FooterText { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Guid"/> to uniquely identify this portal, can be used for such things as encryption and licensing.
        /// </summary>
		public Guid GUID { get; set; }

        /// <summary>
        /// Gets or sets the public facing portal nome directory (url).
        /// </summary>
		public string HomeDirectory { get; set; }

        /// <summary>
        /// Gets or sets the public facing portal (url) path to the special system directory for that portal.
        /// </summary>
		public string HomeSystemDirectory { get; set; }

        /// <summary>
        /// Gets or sets the tab (page) ID of the home page of this portal.
        /// </summary>
		public int HomeTabId { get; set; }

        /// <summary>
        /// Gets or sets the hosting fee for this portal if hosting fees are enabled.
        /// </summary>
		public float HostFee { get; set; }

        /// <summary>
        /// Gets or sets the hosting space allowed if hosting fees are enabled.
        /// </summary>
		public int HostSpace { get; set; }

        /// <summary>
        /// Gets or sets the portal keywords, used for SEO purposes and the Dnn search indexer.
        /// </summary>
		public string KeyWords { get; set; }

        /// <summary>
        /// Gets or sets the tab (page) ID of the login page.
        /// </summary>
		public int LoginTabId { get; set; }

        /// <summary>
        /// Gets or sets the logo for the portal.
        /// </summary>
		public string LogoFile { get; set; }

        /// <summary>
        /// Gets or sets the number of allowed pages if hosting fees are enabled.
        /// </summary>
		public int PageQuota { get; set; }

        /// <summary>
        /// Gets or sets the number of pages (tabs) this portal contains.
        /// </summary>
		public int Pages { get; set; }

        /// <summary>
        /// Gets or sets the ID of the portal.
        /// </summary>
		public int PortalId { get; set; }

        /// <summary>
        /// Gets or sets portal alias currently used by the request.
        /// </summary>
		public PortalAliasInfo PortalAlias { get; set; }

        /// <summary>
        /// Gets or sets the portal primary alias.
        /// </summary>
		public PortalAliasInfo PrimaryAlias { get; set; }

        /// <summary>
        /// Gets or sets the portal name, used for SEO, menus, mesages, logs, etc.
        /// </summary>
		public string PortalName { get; set; }

        /// <summary>
        /// Gets or sets the role id for users that are registered on this portal.
        /// </summary>
		public int RegisteredRoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role that represents registered users on this portal.
        /// </summary>
		public string RegisteredRoleName { get; set; }

        /// <summary>
        /// Gets or sets the tab (page) id where users can go to register.
        /// </summary>
		public int RegisterTabId { get; set; }

        /// <summary>
        /// Gets or sets the registration settings, <see cref="RegistrationSettings"/>.
        /// </summary>
        public RegistrationSettings Registration { get; set; }

        /// <summary>
        /// Gets or sets the tab (page) id where the search results are located.
        /// </summary>
        public int SearchTabId { get; set; }

        /// <summary>
        /// Gets or sets the id or the tab (page) that is used as a splash page.
        /// </summary>
        public int SplashTabId { get; set; }

        /// <summary>
        /// Gets or sets that tab (page) ID that contains superuser (host) modules.
        /// </summary>
		public int SuperTabId { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of users allowed if hosting fees are enabled.
        /// </summary>
		public int UserQuota { get; set; }

        /// <summary>
        /// Gets or setsthe type of user registration in use, <see cref="Globals.PortalRegistrationType"/>.
        /// </summary>
		public int UserRegistration { get; set; }

        /// <summary>
        /// Get or sets the number of users this portal contains.
        /// </summary>
		public int Users { get; set; }

        /// <summary>
        /// Gets or sets the tab (page) ID where the user profile is located.
        /// </summary>
		public int UserTabId { get; set; }

        /// <summary>
        /// Gets or sets the tab (page) ID that contains the portal terms and conditions.
        /// </summary>
        public int TermsTabId { get; set; }

        /// <summary>
        /// Gets or sets the tab (page) ID that contains the portal privacy policy.
        /// </summary>
		public int PrivacyTabId { get; set; }

        /// <summary>
        /// Gets or sets the portal styles, these get injected as css custom properties on each page head, <see cref="PortalStyles"/>.
        /// </summary>
        public PortalStyles Styles { get; set; }
        
        #endregion

        #region Read-Only Properties

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Allows users to select their own UI culture.
        /// When set to false (default) framework will allways same culture for both
        /// CurrentCulture (content) and CurrentUICulture (interface)
        /// </summary>
        /// <remarks>Defaults to False</remarks>
        /// -----------------------------------------------------------------------------
        public bool AllowUserUICulture { get; internal set; }

        public int CdfVersion { get; internal set; }

        public bool ContentLocalizationEnabled { get; internal set; }

        public ControlPanelPermission ControlPanelSecurity { get; internal set; }

        public string DefaultAdminContainer { get; internal set; }

        public string DefaultAdminSkin { get; internal set; }

        public string DefaultAuthProvider { get; internal set; }

        public Mode DefaultControlPanelMode { get; internal set; }

        public bool DefaultControlPanelVisibility { get; internal set; }

        public string DefaultIconLocation { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the Default Module Id
        /// </summary>
        /// <remarks>Defaults to Null.NullInteger</remarks>
        /// -----------------------------------------------------------------------------
        public int DefaultModuleId { get; internal set; }

        public string DefaultModuleActionMenu { get; internal set; }

        public string DefaultPortalContainer { get; internal set; }

        public string DefaultPortalSkin { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the Default Tab Id
        /// </summary>
        /// <remarks>Defaults to Null.NullInteger</remarks>
        /// -----------------------------------------------------------------------------
        public int DefaultTabId { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether Browser Language Detection is Enabled
        /// </summary>
        /// <remarks>Defaults to True</remarks>
        /// -----------------------------------------------------------------------------
        public bool EnableBrowserLanguage { get; internal set; }

        public bool EnableCompositeFiles { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether to use the popup.
        /// </summary>
        /// <remarks>Defaults to True</remarks>
        /// -----------------------------------------------------------------------------
        public bool EnablePopUps { get; internal set; }

        /// <summary>
        /// Website Administrator whether receive the notification email when new user register.
        /// </summary>
        public bool EnableRegisterNotification { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether the Skin Widgets are enabled/supported
        /// </summary>
        /// <remarks>Defaults to True</remarks>
        /// -----------------------------------------------------------------------------
        public bool EnableSkinWidgets { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether a cookie consent popup should be shown
        /// </summary>
        /// <remarks>Defaults to False</remarks>
        /// -----------------------------------------------------------------------------
        public bool ShowCookieConsent { get; internal set; }

        /// <summary>
        /// Link for the user to find out more about cookies. If not specified the link
        /// shown will point to cookiesandyou.com
        /// </summary>
        public string CookieMoreLink { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether enable url language.
        /// </summary>
        /// <remarks>Defaults to True</remarks>
        /// -----------------------------------------------------------------------------
        public bool EnableUrlLanguage { get; internal set; }

        public int ErrorPage404 { get; internal set; }

        public int ErrorPage500 { get; internal set; }

        /// -----------------------------------------------------------------------------
		/// <summary>
		///   Gets whether folders which are hidden or whose name begins with underscore
		///   are included in folder synchronization.
		/// </summary>
		/// <remarks>
		///   Defaults to True
		/// </remarks>
		/// -----------------------------------------------------------------------------
        public bool HideFoldersEnabled { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether hide the login link.
        /// </summary>
        /// <remarks>Defaults to False.</remarks>
        /// -----------------------------------------------------------------------------
        public bool HideLoginControl { get; internal set; }

        public string HomeDirectoryMapPath { get; internal set; }

        public string HomeSystemDirectoryMapPath { get; internal set; }

        /// -----------------------------------------------------------------------------
		/// <summary>
		/// Gets whether the Inline Editor is enabled
		/// </summary>
		/// <remarks>Defaults to True</remarks>
		/// -----------------------------------------------------------------------------
        public bool InlineEditorEnabled { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether to inlcude Common Words in the Search Index
        /// </summary>
        /// <remarks>Defaults to False</remarks>
        /// -----------------------------------------------------------------------------
        public bool SearchIncludeCommon { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether to inlcude Numbers in the Search Index
        /// </summary>
        /// <remarks>Defaults to False</remarks>
        /// -----------------------------------------------------------------------------
        public bool SearchIncludeNumeric { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   Gets the filter used for inclusion of tag info
        /// </summary>
        /// <remarks>
        ///   Defaults to ""
        /// </remarks>
        /// -----------------------------------------------------------------------------
        public string SearchIncludedTagInfoFilter { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the maximum Search Word length to index
        /// </summary>
        /// <remarks>Defaults to 3</remarks>
        /// -----------------------------------------------------------------------------
        public int SearchMaxWordlLength { get; internal set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the minum Search Word length to index
        /// </summary>
        /// <remarks>Defaults to 3</remarks>
        /// -----------------------------------------------------------------------------
        public int SearchMinWordlLength { get; internal set; }

        public bool SSLEnabled { get; internal set; }

        public bool SSLEnforced { get; internal set; }

        public string SSLURL { get; internal set; }

        public string STDURL { get; internal set; }

        public int SMTPConnectionLimit { get; internal set; }

        public int SMTPMaxIdleTime { get; internal set; }

        #endregion

        #region Public Properties

        public CacheLevel Cacheability
        {
            get
            {
                return CacheLevel.fullyCacheable;
            }
        }

        public bool ControlPanelVisible
        {
            get
            {
                var setting = Convert.ToString(Personalization.GetProfile("Usability", "ControlPanelVisible" + PortalId));
                return String.IsNullOrEmpty(setting) ? DefaultControlPanelVisibility : Convert.ToBoolean(setting);
            }
        }

        [Obsolete("Deprecated in Platform 9.4.2. Scheduled removal in v11.0.0. Use CurrentPortalSettings instead.")]
        public static PortalSettings Current
        {
            get
            {
                return PortalController.Instance.GetCurrentPortalSettings();
            }
        }

        /// <summary>
        /// Gets the current portal settings.
        /// </summary>
        public static IPortalSettings CurrentSettings
        {
            get
            {
                return PortalController.Instance.GetCurrentSettings();
            }
        }

        public string DefaultPortalAlias
        {
            get
            {
                foreach (var alias in PortalAliasController.Instance.GetPortalAliasesByPortalId(PortalId).Where(alias => alias.IsPrimary))
                {
                    return alias.HTTPAlias;
                }
                return String.Empty;
            }
        }

        public PortalAliasMapping PortalAliasMappingMode
        {
            get
            {
                return PortalSettingsController.Instance().GetPortalAliasMappingMode(PortalId);
            }
        }

        /// <summary>Gets the currently logged in user identifier.</summary>
        /// <value>The user identifier.</value>
		public int UserId
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Request.IsAuthenticated)
                {
                    return UserInfo.UserID;
                }
                return Null.NullInteger;
            }
        }

        /// <summary>Gets the currently logged in user.</summary>
        /// <value>The current user information.</value>
		public UserInfo UserInfo
        {
            get
            {
                return UserController.Instance.GetCurrentUserInfo();
            }
        }

        public Mode UserMode
        {
            get
            {
                Mode mode;
                if (HttpContext.Current != null && HttpContext.Current.Request.IsAuthenticated)
                {
                    mode = DefaultControlPanelMode;
                    string setting = Convert.ToString(Personalization.GetProfile("Usability", "UserMode" + PortalId));
                    switch (setting.ToUpper())
                    {
                        case "VIEW":
                            mode = Mode.View;
                            break;
                        case "EDIT":
                            mode = Mode.Edit;
                            break;
                        case "LAYOUT":
                            mode = Mode.Layout;
                            break;
                    }
                }
                else
                {
                    mode = Mode.View;
                }
                return mode;
            }
        }

        /// <summary>
        /// Get a value indicating whether the current portal is in maintenance mode (if either this specific portal or the entire instance is locked). If locked, any actions which update the database should be disabled.
        /// </summary>
        public bool IsLocked
        {
            get { return IsThisPortalLocked || Host.Host.IsLocked; }
        }

        /// <summary>
        /// Get a value indicating whether the current portal is in maintenance mode (note, the entire instance may still be locked, this only indicates whether this portal is specifically locked). If locked, any actions which update the database should be disabled.
        /// </summary>
        public bool IsThisPortalLocked
        {
            get { return PortalController.GetPortalSettingAsBoolean("IsLocked", PortalId, false); }
        }

        public TimeZoneInfo TimeZone
        {
            get { return _timeZone; }
            set
            {
                _timeZone = value;
                PortalController.UpdatePortalSetting(PortalId, "TimeZone", value.Id, true);
            }
        }


        public string PageHeadText
        {
            get
            {
                // For New Install
                string pageHead = "<meta content=\"text/html; charset=UTF-8\" http-equiv=\"Content-Type\" />";
                string setting;
                if (PortalController.Instance.GetPortalSettings(PortalId).TryGetValue("PageHeadText", out setting))
                {
                    // Hack to store empty string portalsetting with non empty default value
                    pageHead = (setting == "false") ? "" : setting;
                }
                return pageHead;
            }
        }

        /*
         * add <a name="[moduleid]"></a> on the top of the module
         * 
         * Desactivate this remove the html5 compatibility warnings
         * (and make the output smaller)
         * 
         */
        public bool InjectModuleHyperLink
        {
            get
            {
                return PortalController.GetPortalSettingAsBoolean("InjectModuleHyperLink", PortalId, true);
            }
        }
        /*
         * generates a : Page.Response.AddHeader("X-UA-Compatible", "");
         * 
         
         */
        public string AddCompatibleHttpHeader
        {
            get
            {
                string CompatibleHttpHeader = "IE=edge";
                string setting;
                if (PortalController.Instance.GetPortalSettings(PortalId).TryGetValue("AddCompatibleHttpHeader", out setting))
                {
                    // Hack to store empty string portalsetting with non empty default value
                    CompatibleHttpHeader = (setting == "false") ? "" : setting;
                }
                return CompatibleHttpHeader;
            }
        }

        /*
         * add a cachebuster parameter to generated file URI's
         * 
         * of the form ver=[file timestame] ie ver=2015-02-17-162255-735
         * 
         */
        public bool AddCachebusterToResourceUris
        {
            get
            {
                return PortalController.GetPortalSettingAsBoolean("AddCachebusterToResourceUris", PortalId, true);
            }
        }

        /// <summary>
        /// If this is true, then regular users can't send message to specific user/group.
        /// </summary>
        public bool DisablePrivateMessage
        {
            get
            {
                return PortalController.GetPortalSetting("DisablePrivateMessage", PortalId, "N") == "Y";
            }
        }

        /// <summary>
        /// If true then all users will be pushed through the data consent workflow
        /// </summary>
        public bool DataConsentActive { get; internal set; }

        /// <summary>
        /// Last time the terms and conditions have been changed. This will determine if the user needs to 
        /// reconsent or not. Legally once the terms have changed, users need to sign again. This value is set
        /// by the "reset consent" button on the UI.
        /// </summary>
        public DateTime DataConsentTermsLastChange { get; internal set; }

        /// <summary>
        /// If set this is a tab id of a page where the user will be redirected to for consent. If not set then
        /// the platform's default logic is used.
        /// </summary>
        public int DataConsentConsentRedirect { get; internal set; }

        /// <summary>
        /// Sets what should happen to the user account if a user has been deleted. This is important as
        /// under certain circumstances you may be required by law to destroy the user's data within a 
        /// certain timeframe after a user has requested deletion.
        /// </summary>
        public UserDeleteAction DataConsentUserDeleteAction { get; internal set; }

        /// <summary>
        /// Sets the delay time (in conjunction with DataConsentDelayMeasurement) for the DataConsentUserDeleteAction
        /// </summary>
        public int DataConsentDelay { get; internal set; }

        /// <summary>
        /// Units for DataConsentDelay
        /// </summary>
        public string DataConsentDelayMeasurement { get; internal set; }

        #endregion

        #region IPropertyAccess Members

        public string GetProperty(string propertyName, string format, CultureInfo formatProvider, UserInfo accessingUser, Scope accessLevel, ref bool propertyNotFound)
        {
            var outputFormat = string.Empty;
            if (format == string.Empty)
            {
                outputFormat = "g";
            }
            var lowerPropertyName = propertyName.ToLowerInvariant();
            if (accessLevel == Scope.NoSettings)
            {
                propertyNotFound = true;
                return PropertyAccess.ContentLocked;
            }
            propertyNotFound = true;
            var result = string.Empty;
            var isPublic = true;
            switch (lowerPropertyName)
            {
                case "url":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(PortalAlias.HTTPAlias, format);
                    break;
                case "fullurl": //return portal alias with protocol
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(Globals.AddHTTP(PortalAlias.HTTPAlias), format);
                    break;
                case "passwordreminderurl": //if regsiter page defined in portal settings, then get that page url, otherwise return home page.
                    propertyNotFound = false;
                    var reminderUrl = Globals.AddHTTP(PortalAlias.HTTPAlias);
                    if (RegisterTabId > Null.NullInteger)
                    {
                        reminderUrl = Globals.RegisterURL(string.Empty, string.Empty);
                    }
                    result = PropertyAccess.FormatString(reminderUrl, format);
                    break;
                case "portalid":
                    propertyNotFound = false;
                    result = (PortalId.ToString(outputFormat, formatProvider));
                    break;
                case "portalname":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(PortalName, format);
                    break;
                case "homedirectory":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(HomeDirectory, format);
                    break;
                case "homedirectorymappath":
                    isPublic = false;
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(HomeDirectoryMapPath, format);
                    break;
                case "logofile":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(LogoFile, format);
                    break;
                case "footertext":
                    propertyNotFound = false;
                    var footerText = FooterText.Replace("[year]", DateTime.Now.Year.ToString());
                    result = PropertyAccess.FormatString(footerText, format);
                    break;
                case "expirydate":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (ExpiryDate.ToString(outputFormat, formatProvider));
                    break;
                case "userregistration":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (UserRegistration.ToString(outputFormat, formatProvider));
                    break;
                case "banneradvertising":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (BannerAdvertising.ToString(outputFormat, formatProvider));
                    break;
                case "currency":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(Currency, format);
                    break;
                case "administratorid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (AdministratorId.ToString(outputFormat, formatProvider));
                    break;
                case "email":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(Email, format);
                    break;
                case "hostfee":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (HostFee.ToString(outputFormat, formatProvider));
                    break;
                case "hostspace":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (HostSpace.ToString(outputFormat, formatProvider));
                    break;
                case "pagequota":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (PageQuota.ToString(outputFormat, formatProvider));
                    break;
                case "userquota":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (UserQuota.ToString(outputFormat, formatProvider));
                    break;
                case "administratorroleid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (AdministratorRoleId.ToString(outputFormat, formatProvider));
                    break;
                case "administratorrolename":
                    isPublic = false;
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(AdministratorRoleName, format);
                    break;
                case "registeredroleid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = (RegisteredRoleId.ToString(outputFormat, formatProvider));
                    break;
                case "registeredrolename":
                    isPublic = false;
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(RegisteredRoleName, format);
                    break;
                case "description":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(Description, format);
                    break;
                case "keywords":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(KeyWords, format);
                    break;
                case "backgroundfile":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(BackgroundFile, format);
                    break;
                case "admintabid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = AdminTabId.ToString(outputFormat, formatProvider);
                    break;
                case "supertabid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = SuperTabId.ToString(outputFormat, formatProvider);
                    break;
                case "splashtabid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = SplashTabId.ToString(outputFormat, formatProvider);
                    break;
                case "hometabid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = HomeTabId.ToString(outputFormat, formatProvider);
                    break;
                case "logintabid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = LoginTabId.ToString(outputFormat, formatProvider);
                    break;
                case "registertabid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = RegisterTabId.ToString(outputFormat, formatProvider);
                    break;
                case "usertabid":
                    isPublic = false;
                    propertyNotFound = false;
                    result = UserTabId.ToString(outputFormat, formatProvider);
                    break;
                case "defaultlanguage":
                    propertyNotFound = false;
                    result = PropertyAccess.FormatString(DefaultLanguage, format);
                    break;
                case "users":
                    isPublic = false;
                    propertyNotFound = false;
                    result = Users.ToString(outputFormat, formatProvider);
                    break;
                case "pages":
                    isPublic = false;
                    propertyNotFound = false;
                    result = Pages.ToString(outputFormat, formatProvider);
                    break;
                case "contentvisible":
                    isPublic = false;
                    break;
                case "controlpanelvisible":
                    isPublic = false;
                    propertyNotFound = false;
                    result = PropertyAccess.Boolean2LocalizedYesNo(ControlPanelVisible, formatProvider);
                    break;
            }
            if (!isPublic && accessLevel != Scope.Debug)
            {
                propertyNotFound = true;
                result = PropertyAccess.ContentLocked;
            }
            return result;
        }

        #endregion

        #region Public Methods

        public PortalSettings Clone()
        {
            return (PortalSettings)MemberwiseClone();
        }

        #endregion
    }
}
