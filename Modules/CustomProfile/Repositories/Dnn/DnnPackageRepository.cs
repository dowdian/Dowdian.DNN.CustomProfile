// <copyright file="DnnPackageRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using DotNetNuke.Framework;
using DotNetNuke.Services.Installer.Packages;

namespace Dowdian.Packages.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnPackageRepository
    /// </summary>
    public partial interface IDnnPackageRepository
    {
        /// <summary>
        /// This is for the install process.
        /// </summary>
        /// <param name="portalId">int portalId.</param>
        /// <returns>IList of PackageInfo.</returns>
        IList<PackageInfo> GetExtensionPackages(int portalId);
    }

    /// <summary>
    /// DnnPackageRepository
    /// </summary>
    public partial class DnnPackageRepository : ServiceLocator<IDnnPackageRepository, DnnPackageRepository>, IDnnPackageRepository
    {
        /// <inheritdoc/>
        public IList<PackageInfo> GetExtensionPackages(int portalId)
        {
            return PackageController.Instance.GetExtensionPackages(portalId);
        }

        /// <inheritdoc/>
        protected override Func<IDnnPackageRepository> GetFactory()
        {
            return () => new DnnPackageRepository();
        }
    }
}