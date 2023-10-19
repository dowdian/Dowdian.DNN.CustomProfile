using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DotNetNuke.Abstractions;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Profile;
using DotNetNuke.Security.Membership;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using Dowdian.Modules.CustomProfile.Repositories.Dnn;

namespace Dowdian.Modules.CustomProfile.Controllers
{
    /// <summary>
    /// CustomProfileController
    /// </summary>
    [DnnHandleError]
    public class CustomProfileController : DnnController
    {
        #region Constructors
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
        private int? portalId;
        private int? moduleId;
        private int? userId;
        private string locale;

        public CustomProfileController(
            IDnnUserRepository dnnUserRepository,
            INavigationManager navigationManager,
            IDnnUrlRepository dnnUrlRepository
        )
        {
            this.DnnUserRepository = dnnUserRepository;
            this.NavigationManager = navigationManager;
            this.DnnUrlRepository = dnnUrlRepository;
        }

        public string Locale
        {
            get => this.locale ?? (this.Request?.Headers.GetValues("Locale") ?? new[] { this.PortalSettings.CultureCode }).First();
            set => this.locale = value;
        }

        public int PortalId
        {
            get => this.portalId ?? this.ModuleContext.PortalId;
            set => this.portalId = value;
        }

        public int ModuleId
        {
            get => this.moduleId ?? this.ModuleContext.ModuleId;
            set => this.moduleId = value;
        }

        public int UserId
        {
            get => this.userId ?? this.ModuleContext.PortalSettings.UserId;
            set => this.userId = value;
        }

        protected IDnnUserRepository DnnUserRepository { get; }

        protected INavigationManager NavigationManager { get; }

        protected IDnnUrlRepository DnnUrlRepository { get; }

#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion Constructors


        public ActionResult Index()
        {
            foreach (ProfilePropertyDefinition profProperty in this.User.Profile.ProfileProperties)
            {
                if (profProperty.Visible)
                {
                    var thing1 = profProperty.PropertyName;
                    var thing2 = profProperty.PropertyValue;

                }
            }

            return View(this.User.Profile.ProfileProperties);
        }
    }
}
