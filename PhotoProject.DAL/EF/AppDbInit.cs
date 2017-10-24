using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PhotoProject.DAL.Entities;
using PhotoProject.DAL.Identity;
using PhotoProject.DAL.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.EF
{
    public class AppDbInit : DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(AppContext context)
        {
            AppUserManager userManager =
                   new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleManager =
                new AppRoleManager(new RoleStore<AppRole>(context));

            string roleNameAdmin = MyConstants.ROLE_NAME_ADMIN;
            string roleNameUser = MyConstants.ROLE_NAME_USER;
            string userName = MyConstants.USERNAME;
            string password = MyConstants.PASSWORD;

            if (!roleManager.RoleExists(roleNameAdmin))
            {
                roleManager.Create(new AppRole(roleNameAdmin));
            }

            if (!roleManager.RoleExists(roleNameUser))
            {
                roleManager.Create(new AppRole(roleNameUser));
            }

            AppUser user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new AppUser()
                {
                    UserName = userName
                };
                var userResult = userManager.Create(user, password);
                if (userResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, roleNameAdmin);
                }
            }
        }
    }
}
