namespace PhotoProject.DAL.Migrations
{
    using global::PhotoProject.DAL.EF;
    using global::PhotoProject.DAL.Entities;
    using global::PhotoProject.DAL.Identity;
    using global::PhotoProject.DAL.Util;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppContext context)
        {
            AppUserManager userManager =
                  new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleManager =
                new AppRoleManager(new RoleStore<AppRole>(context));

            string roleNameAdmin = MyConstants.ROLE_NAME_ADMIN;
            string roleNameUser = MyConstants.ROLE_NAME_USER;

            if (!roleManager.RoleExists(roleNameAdmin))
            {
                roleManager.Create(new AppRole(roleNameAdmin));
            }

            if (!roleManager.RoleExists(roleNameUser))
            {
                roleManager.Create(new AppRole(roleNameUser));
            }          
        }
    }
}

