using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PhotoProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Identity
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(RoleStore<AppRole> store)
            : base(store)
        { }
    }
}
