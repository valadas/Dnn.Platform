﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information
namespace DotNetNuke.Security.Permissions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DotNetNuke.Abstractions.Security.Permissions;
    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Data;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Internal.SourceGenerators;
    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Log.EventLog;

    public partial class PermissionController : IPermissionService
    {
        private static readonly DataProvider Provider = DataProvider.Instance();

        public static string BuildPermissions(IList permissions, string permissionKey)
        {
            var permissionsBuilder = new StringBuilder();
            foreach (PermissionInfoBase permission in permissions)
            {
                if (permissionKey.Equals(permission.PermissionKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Deny permissions are prefixed with a "!"
                    string prefix = !permission.AllowAccess ? "!" : string.Empty;

                    // encode permission
                    string permissionString;
                    if (Null.IsNull(permission.UserID))
                    {
                        permissionString = prefix + permission.RoleName + ";";
                    }
                    else
                    {
                        permissionString = prefix + "[" + permission.UserID + "];";
                    }

                    // build permissions string ensuring that Deny permissions are inserted at the beginning and Grant permissions at the end
                    if (prefix == "!")
                    {
                        permissionsBuilder.Insert(0, permissionString);
                    }
                    else
                    {
                        permissionsBuilder.Append(permissionString);
                    }
                }
            }

            // get string
            string permissionsString = permissionsBuilder.ToString();

            // ensure leading delimiter
            if (!permissionsString.StartsWith(";"))
            {
                permissionsString.Insert(0, ";");
            }

            return permissionsString;
        }

        /// <inheritdoc cref="IPermissionService.GetDefinitionsByFolder" />
        [DnnDeprecated(10, 0, 0, $"Use {nameof(IPermissionService)}.{nameof(IPermissionService.GetDefinitionsByFolder)} instead.")]
        public static partial ArrayList GetPermissionsByFolder()
        {
            return new ArrayList(GetPermissionsByFolderEnumerable().ToArray());
        }

        /// <inheritdoc cref="IPermissionService.GetDefinitionsByPortalDesktopModule" />
        [DnnDeprecated(10, 0, 0, $"Use {nameof(IPermissionService)}.{nameof(IPermissionService.GetDefinitionsByPortalDesktopModule)} instead.")]
        public static partial ArrayList GetPermissionsByPortalDesktopModule()
        {
            return new ArrayList(GetPermissionsByPortalDesktopModuleEnumerable().ToArray());
        }

        /// <inheritdoc cref="IPermissionService.GetDefinitionsByTab" />
        [DnnDeprecated(10, 0, 0, $"Use {nameof(IPermissionService)}.{nameof(IPermissionService.GetDefinitionsByTab)} instead.")]
        public static partial ArrayList GetPermissionsByTab()
        {
            return new ArrayList(GetPermissionsByTabEnumerable().ToArray());
        }

        /// <inheritdoc cref="IPermissionService.AddDefinition" />
        public int AddPermission(PermissionInfo permission)
        {
            return this.AddPermission((IPermissionDefinitionInfo)permission);
        }

        /// <inheritdoc cref="IPermissionService.AddDefinition" />
        public int AddPermission(IPermissionDefinitionInfo permission)
        {
            EventLogController.Instance.AddLog(permission, PortalController.Instance.GetCurrentPortalSettings(), UserController.Instance.GetCurrentUserInfo().UserID, string.Empty, EventLogController.EventLogType.PERMISSION_CREATED);
            var permissionId = Convert.ToInt32(Provider.AddPermission(
                permission.PermissionCode,
                permission.ModuleDefId,
                permission.PermissionKey,
                permission.PermissionName,
                UserController.Instance.GetCurrentUserInfo().UserID));

            this.ClearCache();
            return permissionId;
        }

        /// <inheritdoc cref="IPermissionService.DeleteDefinition" />
        public void DeletePermission(int permissionID)
        {
            EventLogController.Instance.AddLog(
                "PermissionID",
                permissionID.ToString(),
                PortalController.Instance.GetCurrentPortalSettings(),
                UserController.Instance.GetCurrentUserInfo().UserID,
                EventLogController.EventLogType.PERMISSION_DELETED);
            Provider.DeletePermission(permissionID);
            this.ClearCache();
        }

        /// <inheritdoc cref="IPermissionService.GetDefinition" />
        public PermissionInfo GetPermission(int permissionID)
        {
            return GetPermissions().SingleOrDefault(p => p.PermissionID == permissionID);
        }

        /// <inheritdoc cref="IPermissionService.GetDefinitionsByCodeAndKey" />
        [DnnDeprecated(10, 0, 0, $"Use {nameof(IPermissionService)}.{nameof(IPermissionService.GetDefinitionsByCodeAndKey)} instead.")]
        public partial ArrayList GetPermissionByCodeAndKey(string permissionCode, string permissionKey)
        {
            return new ArrayList(GetPermissionByCodeAndKeyEnumerable(permissionCode, permissionKey).ToArray());
        }

        /// <inheritdoc cref="IPermissionService.GetDefinitionsByModuleDefId" />
        [DnnDeprecated(10, 0, 0, $"Use {nameof(IPermissionService)}.{nameof(IPermissionService.GetDefinitionsByModuleDefId)} instead.")]
        public partial ArrayList GetPermissionsByModuleDefID(int moduleDefID)
        {
            return new ArrayList(GetPermissionsByModuleDefIdEnumerable(moduleDefID).ToArray());
        }

        /// <inheritdoc cref="IPermissionService.GetDefinitionsByModule" />
        [DnnDeprecated(10, 0, 0, $"Use {nameof(IPermissionService)}.{nameof(IPermissionService.GetDefinitionsByModule)} instead.")]
        public partial ArrayList GetPermissionsByModule(int moduleId, int tabId)
        {
            return new ArrayList(GetPermissionsByModuleEnumerable(moduleId, tabId).ToArray());
        }

        /// <inheritdoc cref="IPermissionService.UpdateDefinition" />
        public void UpdatePermission(PermissionInfo permission)
        {
            this.UpdatePermission((IPermissionDefinitionInfo)permission);
        }

        /// <inheritdoc cref="IPermissionService.UpdateDefinition" />
        public void UpdatePermission(IPermissionDefinitionInfo permission)
        {
            EventLogController.Instance.AddLog(permission, PortalController.Instance.GetCurrentPortalSettings(), UserController.Instance.GetCurrentUserInfo().UserID, string.Empty, EventLogController.EventLogType.PERMISSION_UPDATED);
            Provider.UpdatePermission(
                permission.PermissionId,
                permission.PermissionCode,
                permission.ModuleDefId,
                permission.PermissionKey,
                permission.PermissionName,
                UserController.Instance.GetCurrentUserInfo().UserID);
            this.ClearCache();
        }

        public T RemapPermission<T>(T permission, int portalId)
            where T : PermissionInfoBase
        {
            PermissionInfo permissionInfo = this.GetPermissionByCodeAndKey(permission.PermissionCode, permission.PermissionKey).ToArray().Cast<PermissionInfo>().FirstOrDefault();
            T result = null;

            if (permissionInfo != null)
            {
                int roleID = int.MinValue;
                int userID = int.MinValue;

                if (string.IsNullOrEmpty(permission.RoleName))
                {
                    UserInfo user = UserController.GetUserByName(portalId, permission.Username);
                    if (user != null)
                    {
                        userID = user.UserID;
                    }
                }
                else
                {
                    switch (permission.RoleName)
                    {
                        case Globals.glbRoleAllUsersName:
                            roleID = Convert.ToInt32(Globals.glbRoleAllUsers);
                            break;
                        case Globals.glbRoleUnauthUserName:
                            roleID = Convert.ToInt32(Globals.glbRoleUnauthUser);
                            break;
                        default:
                            RoleInfo role = RoleController.Instance.GetRole(portalId, r => r.RoleName == permission.RoleName);
                            if (role != null)
                            {
                                roleID = role.RoleID;
                            }

                            break;
                    }
                }

                // if role was found add, otherwise ignore
                if (roleID != int.MinValue || userID != int.MinValue)
                {
                    permission.PermissionID = permissionInfo.PermissionID;
                    if (roleID != int.MinValue)
                    {
                        permission.RoleID = roleID;
                    }

                    if (userID != int.MinValue)
                    {
                        permission.UserID = userID;
                    }

                    result = permission;
                }
            }

            return result;
        }

        [DnnDeprecated(7, 3, 0, "Replaced by GetPermissionsByModule(int, int)", RemovalVersion = 10)]
        public partial ArrayList GetPermissionsByModuleID(int moduleId)
        {
            var module = ModuleController.Instance.GetModule(moduleId, Null.NullInteger, true);

            return this.GetPermissionsByModuleDefID(module.ModuleDefID);
        }

        /// <inheritdoc />
        IEnumerable<IPermissionDefinitionInfo> IPermissionService.GetDefinitions() => GetPermissions();

        /// <inheritdoc />
        IEnumerable<IPermissionDefinitionInfo> IPermissionService.GetDefinitionsByFolder() => GetPermissionsByFolderEnumerable();

        /// <inheritdoc />
        IEnumerable<IPermissionDefinitionInfo> IPermissionService.GetDefinitionsByPortalDesktopModule() => GetPermissionsByPortalDesktopModuleEnumerable();

        /// <inheritdoc />
        IEnumerable<IPermissionDefinitionInfo> IPermissionService.GetDefinitionsByTab() => GetPermissionsByTabEnumerable();

        /// <inheritdoc />
        IEnumerable<IPermissionDefinitionInfo> IPermissionService.GetDefinitionsByCodeAndKey(string permissionCode, string permissionKey) => GetPermissionByCodeAndKeyEnumerable(permissionCode, permissionKey);

        /// <inheritdoc />
        IEnumerable<IPermissionDefinitionInfo> IPermissionService.GetDefinitionsByModuleDefId(int moduleDefId) => GetPermissionsByModuleDefIdEnumerable(moduleDefId);

        /// <inheritdoc />
        IEnumerable<IPermissionDefinitionInfo> IPermissionService.GetDefinitionsByModule(int moduleId, int tabId) => GetPermissionsByModuleEnumerable(moduleId, tabId);

        /// <inheritdoc />
        int IPermissionService.AddDefinition(IPermissionDefinitionInfo permission) => this.AddPermission(permission);

        /// <inheritdoc />
        void IPermissionService.DeleteDefinition(IPermissionDefinitionInfo permission) => this.DeletePermission(permission.PermissionId);

        /// <inheritdoc />
        IPermissionDefinitionInfo IPermissionService.GetDefinition(int permissionDefinitionId) => this.GetPermission(permissionDefinitionId);

        /// <inheritdoc />
        void IPermissionService.UpdateDefinition(IPermissionDefinitionInfo permission) => this.UpdatePermission(permission);

        /// <inheritdoc />
        void IPermissionService.ClearCache() => this.ClearCache();

        private static IEnumerable<PermissionInfo> GetPermissions()
        {
            return CBO.GetCachedObject<IEnumerable<PermissionInfo>>(
                new CacheItemArgs(
                DataCache.PermissionsCacheKey,
                DataCache.PermissionsCacheTimeout,
                DataCache.PermissionsCachePriority),
                c => CBO.FillCollection<PermissionInfo>(Provider.ExecuteReader("GetPermissions")));
        }

        private static IEnumerable<PermissionInfo> GetPermissionsByFolderEnumerable()
        {
            return GetPermissions().Where(p => p.PermissionCode == "SYSTEM_FOLDER");
        }

        private static IEnumerable<PermissionInfo> GetPermissionsByPortalDesktopModuleEnumerable()
        {
            return GetPermissions().Where(p => p.PermissionCode == "SYSTEM_DESKTOPMODULE");
        }

        private static IEnumerable<PermissionInfo> GetPermissionsByTabEnumerable()
        {
            return GetPermissions().Where(p => p.PermissionCode == "SYSTEM_TAB");
        }

        private static IEnumerable<PermissionInfo> GetPermissionByCodeAndKeyEnumerable(string permissionCode, string permissionKey)
        {
            return GetPermissions().Where(p => p.PermissionCode.Equals(permissionCode, StringComparison.InvariantCultureIgnoreCase)
                                               && p.PermissionKey.Equals(permissionKey, StringComparison.InvariantCultureIgnoreCase));
        }

        private static IEnumerable<PermissionInfo> GetPermissionsByModuleDefIdEnumerable(int moduleDefId)
        {
            return GetPermissions().Where(p => p.ModuleDefID == moduleDefId);
        }

        private static IEnumerable<PermissionInfo> GetPermissionsByModuleEnumerable(int moduleId, int tabId)
        {
            var module = ModuleController.Instance.GetModule(moduleId, tabId, false);
            var moduleDefId = module.ModuleDefID;

            return GetPermissions().Where(p => p.ModuleDefID == moduleDefId || p.PermissionCode == "SYSTEM_MODULE_DEFINITION");
        }

        private void ClearCache()
        {
            DataCache.RemoveCache(DataCache.PermissionsCacheKey);
        }
    }
}
