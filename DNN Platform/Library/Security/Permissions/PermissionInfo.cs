﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.Security.Permissions
{
    using System;
    using System.Data;
    using System.Xml;
    using System.Xml.Serialization;

    using DotNetNuke.Abstractions.Security.Permissions;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities;
    using Newtonsoft.Json;

    /// <summary>PermissionInfo provides the Entity Layer for Permissions.</summary>
    [Serializable]
    public class PermissionInfo : BaseEntityInfo, IPermissionDefinitionInfo
    {
        /// <inheritdoc cref="IPermissionDefinitionInfo.ModuleDefId" />
        [XmlIgnore]
        [JsonIgnore]
        [Obsolete($"Deprecated in DotNetNuke 9.13.1. Use {nameof(ModuleDefId)} instead. Scheduled for removal in v11.0.0.")]
        [CLSCompliant(false)]
        public int ModuleDefID
        {
            get => this.ModuleDefId;
            set => this.ModuleDefId = value;
        }

        /// <inheritdoc />
        [XmlElement("permissioncode")]
        public string PermissionCode { get; set; }

        /// <inheritdoc cref="IPermissionDefinitionInfo.PermissionID" />
        [XmlIgnore]
        [Obsolete($"Deprecated in DotNetNuke 9.13.1. Use {nameof(PermissionId)} instead. Scheduled for removal in v11.0.0.")]
        [CLSCompliant(false)]
        public int PermissionID
        {
            get => this.PermissionId;
            set => this.PermissionId = value;
        }

        /// <inheritdoc />
        [XmlElement("permissionkey")]
        public string PermissionKey { get; set; }

        /// <inheritdoc />
        [XmlIgnore]
        [JsonIgnore]
        public string PermissionName { get; set; }

        /// <inheritdoc />
        [XmlIgnore]
        [JsonIgnore]
        public int ModuleDefId { get; set; }

        /// <inheritdoc />
        [XmlElement("permissionid")]
        public int PermissionId { get; set; }

        /// <summary>FillInternal fills a PermissionInfo from a Data Reader.</summary>
        /// <param name="dr">The Data Reader to use.</param>
        protected override void FillInternal(IDataReader dr)
        {
            base.FillInternal(dr);

            var @this = (IPermissionDefinitionInfo)this;
            @this.PermissionId = Null.SetNullInteger(dr["PermissionID"]);
            @this.ModuleDefId = Null.SetNullInteger(dr["ModuleDefID"]);
            @this.PermissionCode = Null.SetNullString(dr["PermissionCode"]);
            @this.PermissionKey = Null.SetNullString(dr["PermissionKey"]);
            @this.PermissionName = Null.SetNullString(dr["PermissionName"]);
        }
    }
}
