// <copyright file="DnnUserModuleBaseRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnUserModuleBaseRepository
    /// </summary>
    public partial interface IDnnUserModuleBaseRepository
    {
        /// <summary>
        /// This is for the install process.
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="key">string key</param>
        /// <param name="setting">string setting</param>
        void UpdateSetting(int portalId, string key, string setting);
    }

    /// <summary>
    /// DnnUserModuleBaseRepository
    /// </summary>
    public partial class DnnUserModuleBaseRepository : ServiceLocator<IDnnUserModuleBaseRepository, DnnUserModuleBaseRepository>, IDnnUserModuleBaseRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void UpdateSetting(int portalId, string key, string setting)
        {
            UserModuleBase.UpdateSetting(portalId, key, setting);
        }

        #region - Private Methods
        #endregion - Private Methods

        protected override Func<IDnnUserModuleBaseRepository> GetFactory()
        {
            return () => new DnnUserModuleBaseRepository();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}