// <copyright file="DnnRoleRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Roles;
using Dowdian.Modules.CustomProfile.Repositories.Encryption;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnRoleRepository
    /// </summary>
    public partial interface IDnnRoleRepository
    {
        /// <summary>
        /// Use this to get a Role Group by its name
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="roleGroupName">string roleGroupName</param>
        /// <returns>RoleGroupInfo</returns>
        RoleGroupInfo GetRoleGroupByName(int portalId, string roleGroupName);

        /// <summary>
        /// Use this to get a list of roles in a portal that meet the given criteria
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="predicate">Func of (RoleInfo, bool) predicate</param>
        /// <returns>List of RoleInfo</returns>
        List<RoleInfo> GetRoles(int portalId, Func<RoleInfo, bool> predicate);

        /// <summary>
        /// Use this to get a list of roles for the given user
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="includePrivate">bool includePrivate</param>
        /// <returns>List of UserRoleInfo</returns>
        List<UserRoleInfo> GetUserRoles(UserInfo user, bool includePrivate);

        /// <summary>
        /// Use this to get a Role by its unique identifier
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="roleId">int roleId</param>
        /// <returns>RoleInfo</returns>
        RoleInfo GetRole(int portalId, int roleId);

        /// <summary>
        /// Use this to get a Role by the given condition
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="predicate">Func of (RoleInfo, bool) predicate</param>
        /// <returns>RoleInfo</returns>
        RoleInfo GetRole(int portalId, Func<RoleInfo, bool> predicate);

        /// <summary>
        /// GetRoleByName
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="roleName">string roleName</param>
        /// <returns>RoleInfo</returns>
        RoleInfo GetRoleByName(int portalId, string roleName);

        /// <summary>
        /// UserIsInRole
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="roleName">string roleName</param>
        /// <returns>bool</returns>
        bool UserIsInRole(UserInfo user, string roleName);

        /// <summary>
        /// GetPermissions
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <returns>IDictionary of string, string</returns>
        IDictionary<string, string> GetPermissions(UserInfo user);

        /// <summary>
        /// RoleExists
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="roleName">string roleName</param>
        /// <returns>bool</returns>
        bool RoleExists(int portalId, string roleName);

        /// <summary>
        /// Use this to get Role Group by its name
        /// </summary>
        /// <param name="roleGroup">RoleGroupInfo roleGroup</param>
        /// <returns>int</returns>
        int AddRoleGroup(RoleGroupInfo roleGroup);

        /// <summary>
        /// Use this to create a new security role
        /// </summary>
        /// <param name="managingPortalId">int managingPortalId</param>
        /// <param name="portalId">int portalId</param>
        /// <param name="roleGroupId">int roleGroupId</param>
        /// <param name="roleName">string roleName</param>
        /// <param name="roleDescription">string roleDescription</param>
        /// <param name="autoAssignment">bool autoAssignment</param>
        /// <param name="settings">A Dictionary of settings.</param>
        void CreateSecurityRole(int managingPortalId, int portalId, int roleGroupId, string roleName, string roleDescription, bool autoAssignment, IDictionary<string, string> settings);

        /// <summary>
        /// Use this to update an existing role
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="roleGroupId">int roleGroupId</param>
        /// <param name="roleId">int roleId</param>
        /// <param name="roleName">string roleName</param>
        /// <param name="roleDescription">string roleDescription</param>
        /// <param name="autoAssignment">bool autoAssignment</param>
        /// <param name="settings">A Dictionary of settings.</param>
        void UpdateSecurityRole(int portalId, int roleGroupId, int roleId, string roleName, string roleDescription, bool autoAssignment, IDictionary<string, string> settings);

        /// <summary>
        /// UpdateRole
        /// </summary>
        /// <param name="role">RoleInfo role</param>
        void UpdateRole(RoleInfo role);

        /// <summary>Deletes a role.</summary>
        /// <param name="role">The Role to delete.</param>
        void DeleteRole(RoleInfo role);

        /// <summary>
        /// Gets the settings for a role.
        /// </summary>
        /// <param name="roleId">Id of the role.</param>
        /// <returns>A Dictionary of settings.</returns>
        IDictionary<string, string> GetRoleSettings(int roleId);

        /// <summary>
        /// Update the role settings.
        /// </summary>
        /// <param name="role">The Role.</param>
        /// <param name="clearCache">A flag that indicates whether the cache should be cleared.</param>
        void UpdateRoleSettings(RoleInfo role, bool clearCache);

        /// <summary>
        /// HasPermission
        /// </summary>
        /// <param name="permissionName">string permissionName</param>
        /// <returns>bool</returns>
        bool HasPermission(string permissionName);

        /// <summary>
        /// HasPermission
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="permissionName">string permissionName</param>
        /// <returns>string</returns>
        string HasPermission(UserInfo user, string permissionName);
    }

    /// <summary>
    /// DnnRoleRepository
    /// </summary>
    public partial class DnnRoleRepository : ServiceLocator<IDnnRoleRepository, DnnRoleRepository>, IDnnRoleRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public RoleGroupInfo GetRoleGroupByName(int portalId, string roleGroupName)
        {
            return RoleController.GetRoleGroupByName(portalId, roleGroupName);
        }

        public List<RoleInfo> GetRoles(int portalId, Func<RoleInfo, bool> predicate)
        {
            return RoleController.Instance.GetRoles(portalId, predicate).ToList();
        }

        public List<UserRoleInfo> GetUserRoles(UserInfo user, bool includePrivate)
        {
            return RoleController.Instance.GetUserRoles(user, includePrivate).ToList();
        }

        public RoleInfo GetRole(int portalId, int roleId)
        {
            return RoleController.Instance.GetRoleById(portalId, roleId);
        }

        public RoleInfo GetRole(int portalId, Func<RoleInfo, bool> predicate)
        {
            return RoleController.Instance.GetRole(portalId, predicate);
        }

        public RoleInfo GetRoleByName(int portalId, string roleName)
        {
            return RoleController.Instance.GetRoleByName(portalId, roleName);
        }

        public bool UserIsInRole(UserInfo user, string roleName)
        {
            if (user.IsSuperUser)
            {
                return true;
            }

            var userRoles = this.GetUserRoles(user, true);
            return userRoles.Any(x => x.RoleName == roleName);
        }

        public IDictionary<string, string> GetPermissions(UserInfo user)
        {
            var permissions = new Dictionary<string, string>();
            var roles = this.GetUserRoles(user, true);
            if (user.IsSuperUser)
            {
                var ownerRole = this.GetRoleByName(0, "CustomProfile Owner");
                roles.Add(new UserRoleInfo() { RoleID = ownerRole.RoleID });
            }

            foreach (var role in roles)
            {
                var settings = this.GetRoleSettings(role.RoleID);
                foreach (var permission in settings)
                {
                    var permissionName = permission.Key;
                    var permissionValue = JsonConvert.DeserializeObject<PermissionValue>(this.DecryptSettingValue(permission.Value)).v;

                    if (bool.TryParse(permissionValue, out _))
                    {
                        if (user.IsSuperUser)
                        {
                            permissions[permissionName] = "true";
                            continue;
                        }

                        if (!permissions.ContainsKey(permissionName) || permissions[permissionName].ToLower() == "true")
                        {
                            permissions[permissionName] = permissionValue;
                            continue;
                        }
                    }

                    if (permissionName == "Case_MinValue" && int.TryParse(permissionValue, out _))
                    {
                        if (user.IsSuperUser)
                        {
                            permissions[permissionName] = string.Empty;
                            continue;
                        }

                        if (!permissions.ContainsKey(permissionName) || int.Parse(permissions[permissionName]) < int.Parse(permissionValue))
                        {
                            permissions[permissionName] = permissionValue;
                            continue;
                        }
                    }

                    if (permissionName == "Case_MaxValue" && int.TryParse(permissionValue, out _))
                    {
                        if (user.IsSuperUser)
                        {
                            permissions[permissionName] = string.Empty;
                            continue;
                        }

                        if (!permissions.ContainsKey(permissionName) || int.Parse(permissions[permissionName]) > int.Parse(permissionValue))
                        {
                            permissions[permissionName] = permissionValue;
                            continue;
                        }
                    }

                    permissions[permissionName] = permissionValue;
                }
            }

            return permissions;
        }

        public bool RoleExists(int portalId, string roleName)
        {
            if (RoleController.Instance.GetRoleByName(portalId, roleName) != null)
            {
                return true;
            }

            return false;
        }

        public int AddRoleGroup(RoleGroupInfo roleGroup)
        {
            return RoleController.AddRoleGroup(roleGroup);
        }

        public void CreateSecurityRole(int managingPortalId, int portalId, int roleGroupId, string roleName, string roleDescription, bool autoAssignment, IDictionary<string, string> settings)
        {
            if (!this.RoleExists(portalId, roleName))
            {
                var role = new RoleInfo
                {
                    PortalID = portalId,
                    RoleGroupID = roleGroupId,
                    RoleName = roleName,
                    Description = roleDescription,
                    AutoAssignment = autoAssignment,

                    Status = RoleStatus.Approved,
                    SecurityMode = SecurityMode.SecurityRole,
                    IsPublic = false,
                    RSVPCode = managingPortalId.ToString(),
                    IconFile = string.Empty,
                    TrialPeriod = Null.NullInteger,
                    BillingPeriod = Null.NullInteger,
                    IsSystemRole = false,
                };

                foreach (var setting in settings)
                {
                    // Apply the Tab Access settings to the tabs while we're saving to the database.
                    if (setting.Key.Contains("_TabAccess"))
                    {
                        this.ApplyTabPermission(setting, role);
                    }

                    role.Settings[setting.Key] = EncryptionRepository.Instance.Encrypt(setting.Value);
                }

                RoleController.Instance.AddRole(role);

                this.UpdateRoleSettings(role, true);
            }
        }

        public void UpdateSecurityRole(int portalId, int roleGroupId, int roleId, string roleName, string roleDescription, bool autoAssignment, IDictionary<string, string> settings)
        {
            var role = RoleController.Instance.GetRoleById(portalId, roleId);

            if (role == null)
            {
                return;
            }

            role.PortalID = portalId;
            role.RoleName = roleName;
            role.Description = roleDescription;
            role.AutoAssignment = autoAssignment;
            foreach (var setting in settings)
            {
                // Apply the Tab Access settings to the tabs while we're saving to the database.
                if (setting.Key.Contains("_TabAccess"))
                {
                    this.ApplyTabPermission(setting, role);
                }

                role.Settings[setting.Key] = EncryptionRepository.Instance.Encrypt(setting.Value);
            }

            RoleController.Instance.UpdateRole(role);

            this.UpdateRoleSettings(role, true);
        }

        public void UpdateRole(RoleInfo role)
        {
            RoleController.Instance.UpdateRole(role);
        }

        public void DeleteRole(RoleInfo role)
        {
            RoleController.Instance.DeleteRole(role);
        }

        public IDictionary<string, string> GetRoleSettings(int roleId)
        {
            return RoleController.Instance.GetRoleSettings(roleId);
        }

        public void UpdateRoleSettings(RoleInfo role, bool clearCache)
        {
            RoleController.Instance.UpdateRoleSettings(role, clearCache);
        }

        public bool HasPermission(string permissionName)
        {
            var user = DnnUserRepository.Instance.GetCurrentUser();
            var permissionValue = this.HasPermission(user, permissionName);
            var booleanPermissionValue = false;
            if (bool.TryParse(permissionValue, out booleanPermissionValue))
            {
                return booleanPermissionValue;
            }

            return false;
        }

        public string HasPermission(UserInfo user, string permissionName)
        {
            var permissions = this.GetPermissions(user);
            if (permissions.Count() > 0)
            {
                return permissions[permissionName];
            }

            return Null.NullString;
        }

        protected override Func<IDnnRoleRepository> GetFactory()
        {
            return () => new DnnRoleRepository();
        }

        private string DecryptSettingValue(string value)
        {
            if (value.StartsWith("{\"t\":\""))
            {
                return value;
            }
            else
            {
                return EncryptionRepository.Instance.Decrypt(value);
            }
        }

        private void ApplyTabPermission(KeyValuePair<string, string> setting, RoleInfo role)
        {
            var portalId = role.PortalID;
            var tabCollection = DnnTabRepository.Instance.GetTabsByPortal(portalId);
            var tab = tabCollection.FirstOrDefault(x => x.Value.TabPath.Contains(setting.Key.Replace("_TabAccess", string.Empty))).Value;
            var allowAccess = JsonConvert.DeserializeObject<PermissionValue>(setting.Value).v.ToLower() == "true";
            var permissionCtrl = new PermissionController();
            var permissionsList = permissionCtrl.GetPermissionByCodeAndKey("SYSTEM_TAB", "VIEW");

            // You can't give permission to something that doesn't exists
            if (tab != null)
            {
                // There had better be something here or we've got much bigger issues
                if (permissionsList != null && permissionsList.Count > 0)
                {
                    var translatePermisison = (PermissionInfo)permissionsList[0];

                    if (!tab.TabPermissions.ToList().Any(x => x.RoleName == role.RoleName && x.PermissionKey == translatePermisison.PermissionKey))
                    {
                        tab.TabPermissions.Add(new TabPermissionInfo(translatePermisison)
                        {
                            AllowAccess = allowAccess,
                            RoleID = role.RoleID,
                            RoleName = role.RoleName,
                        });
                    }
                    else
                    {
                        var tabPermission = tab.TabPermissions.ToList().First(x => x.RoleName == role.RoleName && x.PermissionKey == translatePermisison.PermissionKey);
                        tabPermission.AllowAccess = allowAccess;
                    }
                }
            }
        }

        private class PermissionValue
        {
            public string t { get; set; }

            public string v { get; set; }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}