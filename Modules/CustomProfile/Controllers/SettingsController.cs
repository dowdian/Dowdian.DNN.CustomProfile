
using DotNetNuke.Collections;
using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using System.Web.Mvc;

namespace Dowdian.Modules.CustomProfile.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : DnnController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Settings()
        {
            var settings = new Models.Settings();
            settings.Setting1 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Dowdian.Modules.CustomProfile_Setting1", false);
            settings.Setting2 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Dowdian.Modules.CustomProfile_Setting2", System.DateTime.Now);

            return View(settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Settings(Models.Settings settings)
        {
			var security = new PortalSecurity();
			
            ModuleContext.Configuration.ModuleSettings["Dowdian.Modules.CustomProfile_Setting1"] = security.InputFilter(settings.Setting1.ToString().Trim(), PortalSecurity.FilterFlag.NoMarkup);
            ModuleContext.Configuration.ModuleSettings["Dowdian.Modules.CustomProfile_Setting2"] = security.InputFilter(settings.Setting2.ToUniversalTime().ToString("u"), PortalSecurity.FilterFlag.NoMarkup);

            return RedirectToDefaultRoute();
        }
    }
}