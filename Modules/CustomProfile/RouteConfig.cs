// <copyright file="RouteConfig.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

namespace Dowdian.Modules.CustomProfile.Controllers
{
    /// <summary>
    /// RouteConfig
    /// </summary>
    public class RouteConfig : DotNetNuke.Web.Mvc.Routing.IMvcRouteMapper
    {
        /// <summary>
        /// RegisterRoutes
        /// </summary>
        /// <param name="mapRouteManager">DotNetNuke.Web.Mvc.Routing.IMapRoute mapRouteManager</param>
        public void RegisterRoutes(DotNetNuke.Web.Mvc.Routing.IMapRoute mapRouteManager)
        {
            // http://CustomProfile.domain.com/DesktopModules/MVC/Dowdian.Modules.CustomProfile/Controller/Action
            mapRouteManager.MapRoute("Dowdian.Modules.CustomProfile", "default", "{controller}/{action}", new[] { "Dowdian.Modules.CustomProfile.Controllers" });
        }
    }
}