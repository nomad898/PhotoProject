using PhotoProject.DAL.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services.Impl
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new UnitOfWork(connection));
        }

        public IRoleService CreateRoleService(string connection)
        {
            return new RoleService(new UnitOfWork(connection));
        }
    }
}
