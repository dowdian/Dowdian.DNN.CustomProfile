// <copyright file="DnnUserRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Text.RegularExpressions;
using System.Web.Security;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.Security.Membership;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Cache;
using DotNetNuke.Services.Exceptions;
using Dowdian.Modules.CustomProfile.Models;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnUserRepository
    /// </summary>
    public partial interface IDnnUserRepository
    {
        /// <summary>
        /// This is for the install process.
        /// </summary>
        void ChangeSuperUserName();

        /// <summary>
        /// Use this to move a DNN User from one portal to another
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="portalId">int portalId</param>
        void MoveUserToPortal(UserInfo user, int portalId);

        /// <summary>
        /// Use this to set the DNN User's password to a randomly generated 8 character password
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="portalId">int portalId</param>
        /// <returns>string</returns>
        string ResetPassword(UserInfo user, int portalId);

        /// <summary>
        /// Use this to create a new DNN User
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="position">string position</param>
        /// <param name="password">out string password</param>
        /// <returns>int</returns>
        int CreateUser(UserInfo user, string position, out string password);

        /// <summary>
        /// Use this to get a DNN User
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="userId">int userId</param>
        /// <returns>UserInfo</returns>
        UserInfo GetUser(int portalId, int userId);

        /// <summary>
        /// Use this to get a DNN User
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <returns>UserInfo</returns>
        UserInfo GetUser(UserInfo user);

        /// <summary>
        /// Use this to get the currently logged in DNN User
        /// </summary>
        /// <returns>UserInfo</returns>
        UserInfo GetCurrentUser();

        /// <summary>
        /// Use this to modify a DNN User
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="portalId">int portalId</param>
        /// <returns>bool</returns>
        bool UpdateUser(UserInfo user, int portalId);

        /// <summary>
        /// DnnDeleteUser
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="portalId">int portalId</param>
        /// <returns>bool</returns>
        bool DeleteUser(UserInfo user, int portalId);

        /// <summary>
        /// Use this to check if a DNN User is in a DNN Role
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="roleName">string roleName</param>
        /// <returns>bool</returns>
        bool UserIsInRole(UserInfo user, string roleName);

        /// <summary>
        /// Use this to add a DNN User to a DNN Role
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="roleName">string roleName</param>
        void AddUserToRole(UserInfo user, string roleName);

        /// <summary>
        /// Use this to remove a DNN User from a DNN Role
        /// </summary>
        /// <param name="user">UserInfo user</param>
        /// <param name="roleName">string roleName</param>
        void RemoveUserFromRole(UserInfo user, string roleName);
    }

    /// <summary>
    /// DnnUserRepository
    /// </summary>
    public partial class DnnUserRepository : ServiceLocator<IDnnUserRepository, DnnUserRepository>, IDnnUserRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void ChangeSuperUserName()
        {
            var portalId = 0;
            var superUser = UserController.GetUserByName("host");
            superUser.FirstName = "CustomProfile";
            superUser.LastName = "Support";
            superUser.DisplayName = "CustomProfile Support";
            UserController.UpdateUser(portalId, superUser);
        }

        public void MoveUserToPortal(UserInfo user, int portalId)
        {
            var membershipProvider = DotNetNuke.Security.Membership.MembershipProvider.Instance();
            var dnnUser = UserController.Instance.GetUser(user.PortalID, user.UserID);

            membershipProvider.AddUserPortal(portalId, user.UserID);
            membershipProvider.UpdateUser(dnnUser);
            this.AutoAssignUsersToPortalRoles(dnnUser, portalId);
            membershipProvider.RemoveUser(dnnUser);
            membershipProvider.UpdateUser(dnnUser);
        }

        public string ResetPassword(UserInfo user, int portalId)
        {
            var dnnUser = UserController.Instance.GetUser(user.PortalID, user.UserID);
            var newPassword = UserController.GeneratePassword(8);
            UserController.ResetAndChangePassword(dnnUser, newPassword);

            return newPassword;
        }

        public int CreateUser(UserInfo user, string position, out string password)
        {
            password = UserController.GeneratePassword(8);
            var dnnUser = new UserInfo
            {
                PortalID = user.PortalID,
                Username = user.Email,
                Roles = new[] { position },
                IsSuperUser = false,
                IsDeleted = false,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
            };

            // Fill MINIMUM Profile Items (KEY PIECE)
            UserController.GetUserMembership(dnnUser);
            dnnUser.Profile.PreferredLocale = PortalSettings.Current.DefaultLanguage;
            dnnUser.Profile.FirstName = dnnUser.FirstName;
            dnnUser.Profile.LastName = dnnUser.LastName;
            dnnUser.Membership.UpdatePassword = true;
            dnnUser.Membership.Approved = true;
            dnnUser.Membership.Password = password;
            dnnUser.Membership.PasswordConfirm = password;
            dnnUser.Membership.CreatedDate = DateTime.Now;
            dnnUser.Membership.Approved = user.Membership.Approved;

            dnnUser.Profile.SetProfileProperty("UserInterfaceTheme", "system");

            var result = UserController.CreateUser(ref dnnUser);

            if (result != UserCreateStatus.Success)
            {
                Exceptions.LogException(new Exception(result.ToString()));
            }
            else
            {
                var currentPosition = RoleController.Instance.GetRole(user.PortalID, r => r.RoleName == position);
                RoleController.Instance.AddUserRole(user.PortalID, dnnUser.UserID, currentPosition.RoleID, RoleStatus.Approved, false, Null.NullDate, Null.NullDate);
                user.UserID = dnnUser.UserID;
            }

            return user.UserID;
        }

        public UserInfo GetUser(int portalId, int userId)
        {
            return UserController.Instance.GetUser(portalId, userId);
        }

        public UserInfo GetUser(UserInfo user)
        {
            return UserController.Instance.GetUser(user.PortalID, user.UserID);
        }

        public UserInfo GetCurrentUser()
        {
            return UserController.Instance.GetCurrentUserInfo();
        }

        public bool UpdateUser(UserInfo user, int portalId)
        {
            try
            {
                var dnnUser = UserController.Instance.GetUser(user.PortalID, user.UserID);

                // Update the User
                dnnUser.PortalID = user.PortalID;
                dnnUser.FirstName = user.FirstName;
                dnnUser.LastName = user.LastName;
                dnnUser.DisplayName = $"{user.FirstName} {user.LastName}";
                dnnUser.Email = user.Email;

                // Fill the MINIMUM Profile Items
                UserController.GetUserMembership(dnnUser);
                dnnUser.Profile.PreferredLocale = PortalSettings.Current.CultureCode;
                dnnUser.Profile.FirstName = dnnUser.FirstName;
                dnnUser.Profile.LastName = dnnUser.LastName;
                dnnUser.Membership.Approved = user.Membership.Approved;

                DotNetNuke.Security.Membership.MembershipProvider.Instance().UpdateUser(dnnUser);

                // Remove the UserInfo from the Cache, as it has been modified
                var actualCacheKey = string.Format(DataCache.UserProfileCacheKey, portalId, user.Username);
                CachingProvider.Instance().Remove(actualCacheKey);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return false;
            }

            return true;
        }

        public bool DeleteUser(UserInfo user, int portalId)
        {
            try
            {
                var dnnUser = UserController.Instance.GetUser(user.PortalID, user.UserID);

                // Update the User
                dnnUser.IsDeleted = true;

                // TODO: this call is not compatible with Dependency Injection and so cannot be unit tested
                UserController.UpdateUser(portalId, dnnUser);

                // Remove the UserInfo from the Cache, as it has been modified
                var actualCacheKey = string.Format(DataCache.UserProfileCacheKey, portalId, user.Username);
                CachingProvider.Instance().Remove(actualCacheKey);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool UserIsInRole(UserInfo user, string roleName)
        {
            var dnnUser = UserController.Instance.GetUser(user.PortalID, user.UserID);

            return dnnUser.IsInRole(roleName);
        }

        public void AddUserToRole(UserInfo user, string roleName)
        {
            var dnnRole = RoleController.Instance.GetRoleByName(user.PortalID, roleName);
            RoleController.Instance.AddUserRole(user.PortalID, user.UserID, dnnRole.RoleID, RoleStatus.Approved, false, DateTime.Now, Null.NullDate);

            RoleController.Instance.ClearRoleCache(user.PortalID);
            DataCache.ClearUserCache(user.PortalID, user.Username);
        }

        public void RemoveUserFromRole(UserInfo user, string roleName)
        {
            var dnnUser = UserController.Instance.GetUser(user.PortalID, user.UserID);
            var dnnRole = RoleController.Instance.GetRoleByName(user.PortalID, roleName);
            var dnnUserRole = RoleController.Instance.GetUserRole(user.PortalID, user.UserID, dnnRole.RoleID);
            if (dnnUser != null && dnnUserRole != null)
            {
                DotNetNuke.Security.Roles.RoleProvider.Instance().RemoveUserFromRole(user.PortalID, dnnUser, dnnUserRole);
            }

            RoleController.Instance.ClearRoleCache(user.PortalID);
            DataCache.ClearUserCache(user.PortalID, dnnUser.Username);
        }

        protected override Func<IDnnUserRepository> GetFactory()
        {
            return () => new DnnUserRepository();
        }

        #region - Private Methods
        private void AutoAssignUsersToPortalRoles(UserInfo user, int portalId)
        {
            foreach (var role in RoleController.Instance.GetRoles(portalId, item => user.IsInRole(item.RoleName)))
            {
                RoleController.Instance.AddUserRole(portalId, user.UserID, role.RoleID, RoleStatus.Approved, false, Null.NullDate, Null.NullDate);
            }

            // Clear the roles cache - so the usercount is correct
            RoleController.Instance.ClearRoleCache(portalId);
        }
        #endregion - Private Methods
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}