using AutoMapper;
using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Util;
using PhotoProject.DAL.Entities;
using PhotoProject.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services.Impl
{
    public class PostService : IPostService
    {
        private IUnitOfWork db;
        private IPhotoService photoService;
        public PostService(IUnitOfWork uow)
        {
            db = uow;
            photoService = new PhotoService(db);
        }

        public async Task<OperationDetails> CreateAsync(PostDTO item)
        {
            Post post = new Post()
            {
                AlbumId = item.AlbumId,
                Description = item.Description,
                CreatedAt = (DateTime)item.CreatedAt
            };

            if (post != null)
            {
                db.Posts.Create(post);

                foreach (var photo in item.Photos)
                {
                    if (photo != null)
                    {
                        await photoService.CreateAsync(photo);
                    }
                }

                await db.SaveAsync();

                return new OperationDetails(true, "Post created", "");
            }
            else
            {
                return new OperationDetails(false, "Post not created", "Post");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(int id)
        {
            Post post = db.Posts.FindById(id);
            if (post != null)
            {
                db.Posts.Delete(post);
                await db.SaveAsync();
                return new OperationDetails(true, "Post deleted", "");
            }
            else
            {
                return new OperationDetails(false, "Post not deleted", "PostId");
            }
        }

        public async Task<PostDTO> FindByIdAsync(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Post, PostDTO>()
            .ForMember(x => x.Photos, option => option.Ignore()));
            Post post = await db.Posts.FindByIdAsync(id);
            if (post != null)
            {
                PostDTO postDto = Mapper.Map<Post, PostDTO>(post);             
                postDto.Photos = new List<PhotoDTO>();
                foreach (var photo in post.Photos)
                {
                    if (photo != null)
                    {
                        postDto.Photos.Add(new PhotoDTO()
                        {
                            Id = photo.Id,
                            Content = photo.Content,
                            PostId = photo.PostId,
                            Title = photo.Title
                        });
                    }
                }
                return postDto;
            }
            else return null;
        }

        public IQueryable<PostDTO> GetAll()
        {
            ICollection<PostDTO> result = new List<PostDTO>();
            var posts = db.Posts.GetAll();

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    PostDTO postDto = new PostDTO()
                    {
                        AlbumId = post.AlbumId,
                        Id = post.Id,
                        CreatedAt = post.CreatedAt,
                        Description = post.Description
                    };
                    result.Add(postDto);
                }
            }

            return result.AsQueryable();
        }
    }
}
