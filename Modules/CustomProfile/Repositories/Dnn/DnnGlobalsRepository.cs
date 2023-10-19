// <copyright file="DnnGlobalsRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Framework;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnGlobalsRepository
    /// </summary>
    public partial interface IDnnGlobalsRepository
    {
        /// <summary>
        /// This is for the install process.
        /// </summary>
        /// <param name="request">HttpRequestBase request</param>
        /// <param name="parsePortNumber">bool parsePortNumber</param>
        /// <returns>string</returns>
        string GetDomainName(HttpRequestBase request, bool parsePortNumber);

        /// <summary>
        /// This is for the install process.
        /// </summary>
        /// <param name="portalAlias">string portalAlias</param>
        /// <returns>string</returns>
        string AddHTTP(string portalAlias);
    }

    /// <summary>
    /// DnnGlobalsRepository
    /// </summary>
    public partial class DnnGlobalsRepository : ServiceLocator<IDnnGlobalsRepository, DnnGlobalsRepository>, IDnnGlobalsRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string GetDomainName(HttpRequestBase request, bool parsePortNumber)
        {
            return Globals.GetDomainName(request, parsePortNumber);
        }

        public string AddHTTP(string portalAlias)
        {
            return Globals.AddHTTP(portalAlias);
        }

        #region - Private Methods
        #endregion - Private Methods

        protected override Func<IDnnGlobalsRepository> GetFactory()
        {
            return () => new DnnGlobalsRepository();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}