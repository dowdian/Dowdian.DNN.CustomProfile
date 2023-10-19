// <copyright file="DnnModuleRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnModuleRepository
    /// </summary>
    public partial interface IDnnModuleRepository
    {
        /// <summary>
        /// This is for the install process.
        /// </summary>
        /// <param name="tabId">int tabId</param>
        /// <returns>Dictionary of int, ModuleInfo</returns>
        Dictionary<int, ModuleInfo> GetTabModules(int tabId);
    }

    /// <summary>
    /// DnnModuleRepository
    /// </summary>
    public partial class DnnModuleRepository : ServiceLocator<IDnnModuleRepository, DnnModuleRepository>, IDnnModuleRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Dictionary<int, ModuleInfo> GetTabModules(int tabId)
        {
            return ModuleController.Instance.GetTabModules(tabId);
        }

        #region - Private Methods
        #endregion - Private Methods

        protected override Func<IDnnModuleRepository> GetFactory()
        {
            return () => new DnnModuleRepository();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}