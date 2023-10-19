// <copyright file="DnnTabRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Framework;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnTabRepository
    /// </summary>
    public partial interface IDnnTabRepository
    {
        /// <summary>
        /// This is for the install process.
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="tabPath">string tabPath</param>
        /// <param name="cultureCode">string cultureCode</param>
        /// <returns>int</returns>
        int GetTabByTabPath(int portalId, string tabPath, string cultureCode);

        /// <summary>
        /// Get a Tab by it's name.
        /// </summary>
        /// <param name="tabName">string tabName</param>
        /// <param name="portalId">int portalId</param>
        /// <returns>TabInfo</returns>
        TabInfo GetTabByName(string tabName, int portalId);

        /// <summary>
        /// Get all the Tabs in the given Portal ID
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <returns>TabCollection</returns>
        TabCollection GetTabsByPortal(int portalId);
    }

    /// <summary>
    /// DnnTabRepository
    /// </summary>
    public partial class DnnTabRepository : ServiceLocator<IDnnTabRepository, DnnTabRepository>, IDnnTabRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public int GetTabByTabPath(int portalId, string tabPath, string cultureCode)
        {
            return TabController.GetTabByTabPath(portalId, tabPath, cultureCode);
        }

        public TabInfo GetTabByName(string tabName, int portalId)
        {
            return TabController.Instance.GetTabByName(tabName, portalId);
        }

        public TabCollection GetTabsByPortal(int portalId)
        {
            return TabController.Instance.GetTabsByPortal(portalId);
        }

        #region - Private Methods
        #endregion - Private Methods

        protected override Func<IDnnTabRepository> GetFactory()
        {
            return () => new DnnTabRepository();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}