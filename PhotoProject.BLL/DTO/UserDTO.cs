using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public byte[] Avatar { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
