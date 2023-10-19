// <copyright file="DnnPortalRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetNuke.Abstractions.Application;
using DotNetNuke.Abstractions.Portals;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Framework;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnPortalRepository
    /// </summary>
    public partial interface IDnnPortalRepository
    {
        /// <summary>
        /// This is for the install process.
        /// </summary>
        /// <param name="portalAliasService">IPortalAliasService portalAliasService</param>
        /// <param name="applicationStatusInfo">IApplicationStatusInfo applicationStatusInfo</param>
        /// <param name="errors">List of string errors</param>
        /// <param name="serverPath">string serverPath</param>
        /// <param name="siteName">string siteName</param>
        /// <param name="siteAlias">string siteAlias</param>
        /// <param name="siteDescription">string siteDescription</param>
        /// <param name="siteKeywords">string siteKeywords</param>
        /// <param name="isChildSite">bool isChildSite</param>
        /// <param name="homeDirectory">string homeDirectory</param>
        /// <param name="siteGroupId">int siteGroupId</param>
        /// <param name="useCurrent">bool useCurrent</param>
        /// <param name="firstname">string firstname</param>
        /// <param name="lastname">string lastname</param>
        /// <param name="username">string username</param>
        /// <param name="email">string email</param>
        /// <param name="password">string password</param>
        /// <param name="confirm">string confirm</param>
        /// <param name="question">string question = ""</param>
        /// <param name="answer">string answer = ""</param>
        /// <returns>int</returns>
        int CreatePortal(IPortalAliasService portalAliasService, IApplicationStatusInfo applicationStatusInfo, List<string> errors, string serverPath, string siteName, string siteAlias, string siteDescription, string siteKeywords, bool isChildSite, string homeDirectory, int siteGroupId, bool useCurrent, string firstname, string lastname, string username, string email, string password, string confirm, string question = "", string answer = "");

        /// <summary>
        /// GetPortalLanguages
        /// </summary>
        /// <returns>Dictionary of string and Locale</returns>
        Dictionary<string, Locale> GetPortalLanguages();
    }

    /// <summary>
    /// DnnPortalRepository
    /// </summary>
    public partial class DnnPortalRepository : ServiceLocator<IDnnPortalRepository, DnnPortalRepository>, IDnnPortalRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public int CreatePortal(IPortalAliasService portalAliasService, IApplicationStatusInfo applicationStatusInfo, List<string> errors, string serverPath, string siteName, string siteAlias, string siteDescription, string siteKeywords, bool isChildSite, string homeDirectory, int siteGroupId, bool useCurrent, string firstname, string lastname, string username, string email, string password, string confirm, string question = "", string answer = "")
        {
            var localResourcesFile = Path.Combine("~/DesktopModules/admin/Dnn.PersonaBar/Modules/Dnn.Sites/App_LocalResources/Sites.resx");
            var templatePath = string.Empty;
            var templateCulture = string.Empty;
            var portalTemplates = PortalController.Instance.GetAvailablePortalTemplates();
            foreach (var templateInfo in portalTemplates)
            {
                if (templateInfo.Name != "CustomProfile Unit Site")
                {
                    continue;
                }

                templatePath = templateInfo.TemplateFilePath;
                templateCulture = templateInfo.CultureCode;
            }

            var template = PortalController.Instance.GetPortalTemplate(templatePath, templateCulture);

            var strChildPath = string.Empty;
            var closePopUpStr = string.Empty;
            var intPortalId = -1;

            // check template validity
            var schemaFilename = System.Web.HttpContext.Current.Server.MapPath("~/DesktopModules/Admin/Portals/portal.template.xsd");
            var xmlFilename = template.TemplateFilePath;
            var portalTemplateValidator = new PortalTemplateValidator();
            if (!portalTemplateValidator.Validate(xmlFilename, schemaFilename))
            {
                errors.AddRange(portalTemplateValidator.Errors.OfType<string>());
                return intPortalId;
            }

            // Set Portal Name
            siteAlias = siteAlias.ToLowerInvariant().Replace("http://", string.Empty).Replace("https://", string.Empty);

            // Validate Portal Name
            var strPortalAlias = isChildSite
                ? PortalController.GetPortalFolder(siteAlias)
                : siteAlias;

            var error = false;
            var message = string.Empty;
            if (!portalAliasService.ValidateAlias(strPortalAlias, isChildSite))
            {
                error = true;
                message = Localization.GetString("InvalidName", localResourcesFile);
            }

            // check whether have conflict between tab path and portal alias.
            var checkTabPath = $"//{strPortalAlias}";
            if (DnnTabRepository.Instance.GetTabByTabPath(0, checkTabPath, string.Empty) != Null.NullInteger
                || DnnTabRepository.Instance.GetTabByTabPath(Null.NullInteger, checkTabPath, string.Empty) != Null.NullInteger)
            {
                error = true;
                message = Localization.GetString("DuplicateWithTab", localResourcesFile);
            }

            // Validate Password
            if (password != confirm)
            {
                error = true;
                if (!string.IsNullOrEmpty(message))
                {
                    message += "<br/>";
                }

                message += Localization.GetString("InvalidPassword", localResourcesFile);
            }

            // Set Portal Alias for Child Portals
            if (string.IsNullOrEmpty(message))
            {
                if (isChildSite)
                {
                    strChildPath = serverPath + strPortalAlias;

                    if (Directory.Exists(strChildPath))
                    {
                        error = true;
                        message = Localization.GetString("ChildExists", localResourcesFile);
                    }
                    else
                    {
                        strPortalAlias = siteAlias;
                    }
                }
            }

            // Get Home Directory
            var homeDir = homeDirectory != @"Portals/[PortalID]" ? homeDirectory : string.Empty;

            // Validate Home Folder
            if (!string.IsNullOrEmpty(homeDir))
            {
                var fullHomeDir = $"{applicationStatusInfo.ApplicationMapPath}\\{homeDir}\\".Replace("/", "\\");
                if (Directory.Exists(fullHomeDir))
                {
                    error = true;
                    message = string.Format(Localization.GetString("CreatePortalHomeFolderExists.Error", localResourcesFile), homeDir);
                }

                if (homeDir.Contains("admin") || homeDir.Contains("DesktopModules") || homeDir.ToLowerInvariant() == "portals/")
                {
                    error = true;
                    message = Localization.GetString("InvalidHomeFolder", localResourcesFile);
                }
            }

            // Validate Portal Alias
            if (!string.IsNullOrEmpty(strPortalAlias))
            {
                var portalAlias = portalAliasService.GetPortalAliases()
                    .Select(alias => alias.Value)
                    .FirstOrDefault(portalAliasInfo => string.Equals(
                        portalAliasInfo.HttpAlias,
                        strPortalAlias,
                        StringComparison.OrdinalIgnoreCase));

                if (portalAlias != null)
                {
                    error = true;
                    message = Localization.GetString("DuplicatePortalAlias", localResourcesFile);
                }
            }

            // Create Portal
            if (!error)
            {
                // Attempt to create the portal
                try
                {
                    intPortalId = PortalController.Instance.CreatePortal(
                        siteName,
                        1, // The Host user
                        siteDescription,
                        siteKeywords,
                        template,
                        homeDir,
                        strPortalAlias,
                        serverPath,
                        strChildPath,
                        isChildSite);
                }
                catch (Exception ex)
                {
                    Exceptions.LogException(ex);

                    intPortalId = Null.NullInteger;
                    message = ex.Message;

                    this.TryDeleteCreatingPortal(serverPath, isChildSite ? strChildPath : string.Empty);
                }

                if (intPortalId != -1)
                {
                    // Add new portal to Site Group
                    if (siteGroupId != Null.NullInteger)
                    {
                        var portal = PortalController.Instance.GetPortal(intPortalId);
                        var portalGroup = PortalGroupController.Instance.GetPortalGroups().SingleOrDefault(g => g.PortalGroupId == siteGroupId);
                        if (portalGroup != null)
                        {
                            PortalGroupController.Instance.AddPortalToGroup(portal, portalGroup, args => { });
                        }
                    }

                    // Create a Portal Settings object for the new Portal
                    var objPortal = PortalController.Instance.GetPortal(intPortalId);
                    var webUrl = DnnGlobalsRepository.Instance.AddHTTP(strPortalAlias);

                    // mark default language as published if content localization is enabled
                    var contentLocalizationEnabled = PortalController.GetPortalSettingAsBoolean("ContentLocalizationEnabled", 0, false);
                    if (contentLocalizationEnabled)
                    {
                        var lc = new LocaleController();
                        lc.PublishLanguage(intPortalId, objPortal.DefaultLanguage, true);
                    }

                    // Redirect to this new site
                    if (message != Null.NullString)
                    {
                        message = string.Format(Localization.GetString("SendMail.Error", localResourcesFile), message, webUrl, closePopUpStr);
                    }
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                errors.Add(message);
            }

            return intPortalId;
        }

        public Dictionary<string, Locale> GetPortalLanguages()
        {
            var locales = LocaleController.Instance.GetLocales(Null.NullInteger);
            return locales;
        }

        protected override Func<IDnnPortalRepository> GetFactory()
        {
            return () => new DnnPortalRepository();
        }

        #region - Private Methods
        private void TryDeleteCreatingPortal(string serverPath, string childPath)
        {
            try
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Items.Contains("CreatingPortalId"))
                {
                    var creatingPortalId = Convert.ToInt32(System.Web.HttpContext.Current.Items["CreatingPortalId"]);
                    var portalInfo = PortalController.Instance.GetPortal(creatingPortalId);
                    DotNetNuke.Entities.Portals.PortalController.DeletePortal(portalInfo, serverPath);
                }

                if (!string.IsNullOrEmpty(childPath))
                {
                    DotNetNuke.Entities.Portals.PortalController.DeletePortalFolder(string.Empty, childPath);
                }
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }
        #endregion - Private Methods
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}