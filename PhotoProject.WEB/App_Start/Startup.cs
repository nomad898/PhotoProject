using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using PhotoProject.BLL.Services;
using PhotoProject.BLL.Services.Impl;

[assembly: OwinStartup(typeof(PhotoProject.WEB.App_Start.Startup))]

namespace PhotoProject.WEB.App_Start
{
    public class Startup
    {
        IServiceCreator serviceCreator = new ServiceCreator();

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.CreatePerOwinContext<IRoleService>(CreateRoleService);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        private const string DB_NAME = "PhotoProjectDb";

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService(DB_NAME);
        }

        private IRoleService CreateRoleService()
        {
            return serviceCreator.CreateRoleService(DB_NAME);
        }
    }
}
