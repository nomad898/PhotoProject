using Ninject.Modules;
using PhotoProject.DAL.Repositories;
using PhotoProject.DAL.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Util
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;

        public ServiceModule(string connection)
        {
            connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
