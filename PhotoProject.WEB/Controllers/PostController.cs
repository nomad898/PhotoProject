using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Services;
using PhotoProject.BLL.Util;
using PhotoProject.WEB.Models;
using PhotoProject.WEB.Util.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PhotoProject.WEB.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class PostController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }
        private IAlbumService AlbumService;
        private IPostService PostService;
        private IPhotoService PhotoService;
        private ICommentService CommentService;
        private IRatingService RatingService;

        public PostController(IPostService postService,
            IAlbumService albumService,
            IPhotoService photoService,
            ICommentService commentService,
            IRatingService ratingService)
        {
            PostService = postService;
            AlbumService = albumService;
            PhotoService = photoService;
            CommentService = commentService;
            RatingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumDTO albumDto = await AlbumService.FindByIdAsync((int)id);
            if (albumDto == null)
            {
                return HttpNotFound();
            }
            string userId = albumDto.UserId;
            if (userId != User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Home");
            }

            CreatePostViewModel postVM = new CreatePostViewModel()
            {
                AlbumId = albumDto.Id
            };
            return View(postVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(CreatePostViewModel postVM)
        {
            if (ModelState.IsValid)
            {
                postVM.CreatedAt = DateTime.Now;
                PostDTO postDto = new PostDTO()
                {
                    CreatedAt = postVM.CreatedAt,
                    AlbumId = postVM.AlbumId,
                    Description = postVM.Description,
                    Photos = new List<PhotoDTO>()
                };

                if (postVM.Photos != null)
                {
                    foreach (var photo in postVM.Photos)
                    {
                        if (photo != null &&
                            photo.Image != null
                            && photo.Image.ContentLength > 0)
                        {
                            byte[] imgData = null;
                            imgData = ImageConverter.ConvertImage(photo.Image);
                            PhotoDTO photoDto = new PhotoDTO()
                            {
                                Title = photo.Title,
                                Content = imgData
                            };
                            postDto.Photos.Add(photoDto);
                        }
                    }
                }

                var result = await PostService.CreateAsync(postDto);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyAlbums", "Album");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            return View(postVM);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            PostDTO postDto = await PostService.FindByIdAsync((int)id);

            if (postDto == null)
            {
                return HttpNotFound();
            }

            Mapper.Initialize(cfg => cfg.CreateMap<PostDTO, PostViewModel>()
            .ForMember(x => x.Photos, option => option.Ignore())); ;
            PostViewModel postVM = Mapper.Map<PostDTO, PostViewModel>(postDto);

            AlbumDTO albumDto = await AlbumService.FindByIdAsync(postVM.AlbumId);

            if (albumDto.Public == false && albumDto.UserId != User.Identity.GetUserId())
            {
                return View("PrivateAlbum");
            }
            postVM.Photos = new List<PhotoViewModel>();
            postVM.Comments = new List<CommentViewModel>();
            postVM.Ratings = new List<RatingViewModel>();
            foreach (var item in postDto.Photos)
            {
                postVM.Photos.Add(new PhotoViewModel()
                {
                    Content = item.Content,
                    Id = item.Id,
                    PostId = item.PostId,
                    Title = item.Title
                });
            }

            foreach (var item in CommentService.GetAll().Where(x => x.PostId == id).ToList())
            {
                UserDTO userDto = await UserService.FindByIdAsync(item.UserId);
                postVM.Comments.Add(new CommentViewModel()
                {
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    Id = item.Id,
                    PostId = item.PostId,
                    UserId = item.UserId,
                    UserName = userDto.UserName
                });
            }

            float averageRating = 0;
            int voteCounter = 0;
            foreach (var item in RatingService.GetAll().Where(x => x.PostId == id).ToList())
            {
                UserDTO userDto = await UserService.FindByIdAsync(item.UserId);
                postVM.Ratings.Add(new RatingViewModel()
                {                    
                    Id = item.Id,
                    RatingValue = item.RatingValue,
                    PostId = item.PostId,
                    UserId = item.UserId,
                    UserName = userDto.UserName
                });
                averageRating += item.RatingValue;
                voteCounter++;
            }

            postVM.AverageRating = averageRating / voteCounter;
            postVM.VoteCounter = voteCounter;
            PostInfo postInfo = new PostInfo()
            {
                Post = postVM,
                Rating = new RatingViewModel()
                {
                    PostId = (int)id,
                    UserId = postVM.UserId
                },
                Comment = new CommentViewModel()
                {
                    PostId = (int)id,
                    UserId = postVM.UserId,
                    UserName = User.Identity.Name
                }
            };
            return View(postInfo);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            PostDTO postDto = await PostService.FindByIdAsync(id);
            AlbumDTO albumDto = await AlbumService.FindByIdAsync(postDto.AlbumId);
            if (albumDto.UserId == User.Identity.GetUserId())
            {
                await PostService.DeleteByIdAsync(id);
            }
            return RedirectToAction("Details", albumDto.Id);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> AddComment(CommentViewModel commentVM)
        {
            if (ModelState.IsValid)
            {
                commentVM.CreatedAt = DateTime.Now;
                commentVM.UserId = User.Identity.GetUserId();
                commentVM.UserName = User.Identity.Name;
                Mapper.Initialize(cfg => cfg.CreateMap<CommentViewModel, CommentDTO>());
                CommentDTO commentDto = Mapper.Map<CommentViewModel, CommentDTO>(commentVM);
                OperationDetails result = await CommentService.CreateAsync(commentDto);

                if (result.Succeeded)
                {
                    return RedirectToAction("MyAlbums","Album");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            return View(commentVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> AddRating(RatingViewModel ratingVM)
        {
            if (ModelState.IsValid)
            {
                ratingVM.UserId = User.Identity.GetUserId();
                RatingDTO rDto = await RatingService.FindByUserIdAsync(ratingVM.UserId);
                if (rDto != null)
                {
                    await RatingService.DeleteByIdAsync(rDto.Id);
                }
                Mapper.Initialize(cfg => cfg.CreateMap<RatingViewModel, RatingDTO>());

                RatingDTO ratingDto = Mapper.Map<RatingViewModel, RatingDTO>(ratingVM);
               
                OperationDetails result = await RatingService.CreateAsync(ratingDto);

                RatingDTO r = await RatingService.FindByUserIdAsync(ratingVM.UserId);

                if (result.Succeeded)
                {
                    return RedirectToAction("MyAlbums", "Album");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            return View(ratingVM);
        }
    }
}