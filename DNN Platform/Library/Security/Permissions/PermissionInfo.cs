// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.Security.Permissions
{
    using System;
    using System.Data;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using DotNetNuke.Abstractions.Security.Permissions;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities;
    using Newtonsoft.Json;

    /// <summary>PermissionInfo provides the Entity Layer for Permissions.</summary>
    [Serializable]
    public class PermissionInfo : BaseEntityInfo, IPermissionDefinitionInfo, IXmlSerializable
    {
        private int permissionId;
        private int moduleDefId;

        /// <inheritdoc cref="IPermissionDefinitionInfo.ModuleDefId" />
        [XmlIgnore]
        [JsonIgnore]
        [Obsolete($"Deprecated in DotNetNuke 9.13.1. Use {nameof(IPermissionDefinitionInfo)}.{nameof(IPermissionDefinitionInfo.ModuleDefId)} instead. Scheduled for removal in v11.0.0.")]
        [CLSCompliant(false)]
        public int ModuleDefID
        {
            get => this.moduleDefId;
            set => this.moduleDefId = value;
        }

        /// <inheritdoc />
        [XmlElement("permissioncode")]
        public string PermissionCode { get; set; }

        /// <inheritdoc cref="IPermissionDefinitionInfo.PermissionID" />
        [XmlElement("permissionid")]
        [Obsolete($"Deprecated in DotNetNuke 9.13.1. Use {nameof(IPermissionDefinitionInfo)}.{nameof(IPermissionDefinitionInfo.PermissionId)} instead. Scheduled for removal in v11.0.0.")]
        [CLSCompliant(false)]
        public int PermissionID
        {
            get => this.permissionId;
            set => this.permissionId = value;
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
        int IPermissionDefinitionInfo.ModuleDefId
        {
            get => this.moduleDefId;
            set => this.moduleDefId = value;
        }

        /// <inheritdoc />
        [XmlIgnore]
        [JsonIgnore]
        int IPermissionDefinitionInfo.PermissionId
        {
            get => this.permissionId;
            set => this.permissionId = value;
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc/>
        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "PermissionInfo")
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        string elementName = reader.LocalName;
                        reader.Read();
                        switch (elementName)
                        {
                            case "permissioncode":
                                this.PermissionCode = reader.Value;
                                break;
                            case "permissionid":
                                var permissionId = int.Parse(reader.Value);
                                (this as IPermissionDefinitionInfo).PermissionId = permissionId;
                                break;
                            case "permissionkey":
                                this.PermissionKey = reader.Value;
                                break;
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void WriteXml(XmlWriter writer)
        {
            var @this = (IPermissionDefinitionInfo)this;
            writer.WriteElementString(nameof(@this.PermissionCode).ToLowerInvariant(), @this.PermissionCode);
            writer.WriteElementString(nameof(@this.PermissionId).ToLowerInvariant(), @this.PermissionId.ToString());
            writer.WriteElementString(nameof(@this.PermissionKey).ToLowerInvariant(), @this.PermissionKey);
        }

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
