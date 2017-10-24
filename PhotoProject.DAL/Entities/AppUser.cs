using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public byte[] Avatar { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual ICollection<Album> Albums { get; set; }

        public AppUser() : base()
        {
        }

        public async Task<ClaimsIdentity> 
            GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {            
            var userIdentity =
                await manager.CreateIdentityAsync
                (this, DefaultAuthenticationTypes.ApplicationCookie);
          
            return userIdentity;
        }
    }
}
