using Ninject;
using PhotoProject.BLL.Services;
using PhotoProject.BLL.Services.Impl;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PhotoProject.WEB.Util.Ninject
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IAlbumService>().To<AlbumService>();
            kernel.Bind<IPostService>().To<PostService>();
            kernel.Bind<IPhotoService>().To<PhotoService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<IRatingService>().To<RatingService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IRoleService>().To<RoleService>();       
        }
    }
}