// <copyright file="Startup.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using DotNetNuke.Abstractions.Application;
using DotNetNuke.Abstractions.Portals;
using DotNetNuke.Application;
using DotNetNuke.DependencyInjection;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security.Roles;
using Dowdian.Modules.CustomProfile.Repositories.Dnn;
using Dowdian.Modules.CustomProfile.Repositories.Encryption;
using Dowdian.Packages.CustomProfile.Repositories.Dnn;
using Microsoft.Extensions.DependencyInjection;

namespace Dowdian.Modules.CustomProfile
{
    /// <summary>
    /// Startup
    /// </summary>
    public partial class Startup : IDnnStartup
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services">IServiceCollection services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IApplicationStatusInfo, ApplicationStatusInfo>();
            services.AddScoped<IModuleController, ModuleController>();
            services.AddScoped<IPortalController, PortalController>();
            services.AddScoped<IPortalAliasService, PortalAliasController>();
            services.AddScoped<IPortalGroupController, PortalGroupController>();
            services.AddScoped<IRoleController, RoleController>();
            services.AddScoped<ITabController, TabController>();

            services.AddScoped<IDnnUrlRepository, DnnUrlRepository>();
            services.AddScoped<IDnnUserRepository, DnnUserRepository>();
            services.AddScoped<IDnnFileSystemRepository, DnnFileSystemRepository>();
            services.AddScoped<IEncryptionRepository, EncryptionRepository>();
            services.AddScoped<IDnnScheduleRepository, DnnScheduleRepository>();

            services.AddScoped<IDnnGlobalsRepository, DnnGlobalsRepository>();
            services.AddScoped<IDnnPortalRepository, DnnPortalRepository>();
            services.AddScoped<IDnnRoleRepository, DnnRoleRepository>();
            services.AddScoped<IDnnTabRepository, DnnTabRepository>();
            services.AddScoped<IDnnModuleRepository, DnnModuleRepository>();
            services.AddScoped<IDnnUserModuleBaseRepository, DnnUserModuleBaseRepository>();
            services.AddScoped<IDnnPackageRepository, DnnPackageRepository>();

            //this.ConfigureDataServices(services);
        }
    }
}
