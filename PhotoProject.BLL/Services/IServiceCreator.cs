using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
        IRoleService CreateRoleService(string connection);
    }
}
