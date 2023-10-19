// <copyright file="RouteMapper.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

namespace Dowdian.Modules.CustomProfile.Controllers
{
    /// <summary>
    /// RouteMapper
    /// </summary>
    public class RouteMapper : DotNetNuke.Web.Api.IServiceRouteMapper
    {
        /// <summary>
        /// RegisterRoutes
        /// </summary>
        /// <param name="mapRouteManager">DotNetNuke.Web.Api.IMapRoute mapRouteManager</param>
        public void RegisterRoutes(DotNetNuke.Web.Api.IMapRoute mapRouteManager)
        {
            // http://CustomProfile.domain.com/DesktopModules/CustomProfile/API/Controller/Action
            mapRouteManager.MapHttpRoute("CustomProfile", "default", "{controller}/{action}", new[] { "Dowdian.Modules.CustomProfile.Controllers" });
        }
    }
}