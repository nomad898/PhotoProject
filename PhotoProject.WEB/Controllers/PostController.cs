﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Infrastructure;
using PhotoProject.BLL.Services;
using PhotoProject.WEB.Models;
using PhotoProject.WEB.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PhotoProject.WEB.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class PostController : Controller
    {
        private readonly IUserService userService;
        private readonly IAlbumService albumService;
        private readonly IPostService postService;
        private readonly IPhotoService photoService;
        private readonly ICommentService commentService;
        private readonly IRatingService ratingService;

        public PostController(IUserService userService, 
            IPostService postService,
            IAlbumService albumService,
            IPhotoService photoService,
            ICommentService commentService,
            IRatingService ratingService)
        {
            this.userService = userService;
            this.postService = postService;
            this.albumService = albumService;
            this.photoService = photoService;
            this.commentService = commentService;
            this.ratingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumDTO albumDto = await albumService.FindByIdAsync((int)id);
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

                var result = await postService.CreateAsync(postDto);
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

            PostDTO postDto = await postService.FindByIdAsync((int)id);

            if (postDto == null)
            {
                return HttpNotFound();
            }

            Mapper.Initialize(cfg => cfg.CreateMap<PostDTO, PostViewModel>()
             .ForMember(p => p.Photos, option => option.Ignore()));
            PostViewModel postVM = Mapper.Map<PostDTO, PostViewModel>(postDto);

            AlbumDTO albumDto = await albumService.FindByIdAsync(postVM.AlbumId);

            if (albumDto.Public == false && albumDto.UserId != User.Identity.GetUserId())
            {
                return View("PrivateAlbum");
            }
            Mapper.Initialize(cfg => cfg.CreateMap<PhotoDTO, PhotoViewModel>());
            postVM.Photos = Mapper.Map<ICollection<PhotoDTO>, IEnumerable<PhotoViewModel>>(postDto.Photos);

            Mapper.Initialize(cfg => cfg.CreateMap<CommentDTO, CommentViewModel>());
            postVM.Comments = Mapper.Map<IQueryable<CommentDTO>, IEnumerable<CommentViewModel>>
                (commentService.GetAll().Where(x => x.PostId == id));                       

            foreach (var item in postVM.Comments)
            {
                UserDTO userDto = await userService.FindByIdAsync(item.UserId);                
                item.UserName = userDto.UserName;
            }

            Mapper.Initialize(cfg => cfg.CreateMap<RatingDTO, RatingViewModel>());
            postVM.Ratings = Mapper.Map<IQueryable<RatingDTO>, IEnumerable<RatingViewModel>>
                (ratingService.GetAll().Where(x => x.PostId == id));

            float ratingsSum = 0;
            int voteCounter = 0;
          
            foreach (var item in postVM.Ratings)
            {
                UserDTO userDto = await userService.FindByIdAsync(item.UserId);
                item.UserName = userDto.UserName;
                ratingsSum += item.RatingValue;
                voteCounter++;
            }
            
            postVM.AverageRating = ratingsSum / voteCounter;
            postVM.VoteCounter = voteCounter;
            PostInfo postInfo = new PostInfo()
            {
                Post = postVM,
                Rating = new RatingViewModel()
                {
                    PostId = (int)id,
                    UserId = postVM.UserId,
                    UserName = User.Identity.Name
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
            PostDTO postDto = await postService.FindByIdAsync(id);
            AlbumDTO albumDto = await albumService.FindByIdAsync(postDto.AlbumId);
            if (albumDto.UserId == User.Identity.GetUserId())
            {
                await postService.DeleteByIdAsync(id);
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
                OperationDetails result = await commentService.CreateAsync(commentDto);

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
                RatingDTO rDto = await ratingService.FindByUserIdAsync(ratingVM.UserId);
                if (rDto != null)
                {
                    await ratingService.DeleteByIdAsync(rDto.Id);
                }
                Mapper.Initialize(cfg => cfg.CreateMap<RatingViewModel, RatingDTO>());

                RatingDTO ratingDto = Mapper.Map<RatingViewModel, RatingDTO>(ratingVM);
               
                OperationDetails result = await ratingService.CreateAsync(ratingDto);

                RatingDTO r = await ratingService.FindByUserIdAsync(ratingVM.UserId);

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