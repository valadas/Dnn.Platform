﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace Dnn.PersonaBar.Prompt.Components.Commands.Client
{
    using System;

    using Dnn.PersonaBar.Library.Prompt;
    using Dnn.PersonaBar.Library.Prompt.Attributes;
    using Dnn.PersonaBar.Library.Prompt.Models;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Services.Localization;

    [ConsoleCommand("clear-screen", Constants.GeneralCategory, "Prompt_Cls_Description")]

    public class ClearScreen : IConsoleCommand
    {
        /// <inheritdoc/>
        public string LocalResourceFile => Constants.LocalResourcesFile;

        /// <inheritdoc/>
        public string ResultHtml => Localization.GetString("Prompt_Cls_ResultHtml", this.LocalResourceFile);

        /// <inheritdoc/>
        public string ValidationMessage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public void Initialize(string[] args, PortalSettings portalSettings, UserInfo userInfo, int activeTabId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ConsoleResultModel Run()
        {
            throw new NotImplementedException();
        }
    }
}
