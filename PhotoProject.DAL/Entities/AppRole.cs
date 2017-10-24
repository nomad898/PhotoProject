using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }
    }
}
