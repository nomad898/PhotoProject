using PhotoProject.DAL.Identity;
using PhotoProject.DAL.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        AppRoleManager RoleManager { get; }
        PostRepository Posts { get; }
        AlbumRepository Albums { get; }
        CommentRepository Comments { get; }
        PhotoRepository Photos { get; }
        RatingRepository Ratings { get; }
        Task SaveAsync();
    }
}
