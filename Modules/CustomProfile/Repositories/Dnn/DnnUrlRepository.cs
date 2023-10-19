// <copyright file="DnnUrlRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Web.Routing;
using DotNetNuke.Framework;
using DotNetNuke.UI.Modules;
using DotNetNuke.Web.Mvc.Common;
using DotNetNuke.Web.Mvc.Routing;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnUrlRepository
    /// </summary>
    public partial interface IDnnUrlRepository
    {
        /// <summary>
        /// Use this to get the url instead of the built-in DNN Method "Url.Action"
        /// </summary>
        /// <param name="actionName">string actionName</param>
        /// <param name="controllerName">string controllerName</param>
        /// <param name="parameters">object parameters</param>
        /// <param name="moduleInstanceContext">ModuleInstanceContext moduleInstanceContext</param>
        /// <returns>string</returns>
        string Action(string actionName, string controllerName, object parameters, ModuleInstanceContext moduleInstanceContext);

        /// <summary>
        /// Use this to get the url instead of the built-in DNN Method "Url.Action"
        /// </summary>
        /// <param name="actionName">string actionName</param>
        /// <param name="controllerName">string controllerName</param>
        /// <param name="moduleInstanceContext">ModuleInstanceContext moduleInstanceContext</param>
        /// <returns>string</returns>
        string Action(string actionName, string controllerName, ModuleInstanceContext moduleInstanceContext);
    }

    /// <summary>
    /// DnnUrlRepository
    /// </summary>
    public partial class DnnUrlRepository : ServiceLocator<IDnnUrlRepository, DnnUrlRepository>, IDnnUrlRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Action(string actionName, string controllerName, ModuleInstanceContext moduleInstanceContext)
        {
            return this.Action(actionName, controllerName, new RouteValueDictionary(), moduleInstanceContext);
        }

        public string Action(string actionName, string controllerName, object parameters, ModuleInstanceContext moduleInstanceContext)
        {
            RouteValueDictionary routeValues = TypeHelper.ObjectToDictionary(parameters);
            routeValues["controller"] = controllerName;
            routeValues["action"] = actionName;

            var url = ModuleRoutingProvider.Instance().GenerateUrl(routeValues, moduleInstanceContext);
            return url;
        }

        protected override Func<IDnnUrlRepository> GetFactory()
        {
            return () => new DnnUrlRepository();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}