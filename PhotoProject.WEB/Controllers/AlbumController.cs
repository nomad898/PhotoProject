using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Services;
using PhotoProject.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PhotoProject.WEB.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class AlbumController : Controller
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

        public AlbumController(IPostService postService, IAlbumService albumService)
        {
            PostService = postService;
            AlbumService = albumService;
        }

        public ActionResult Index()
        {
            IQueryable<AlbumDTO> albumsDto = AlbumService.GetAll().Where(a => a.Public == true);
            ICollection<AlbumViewModel> albumsVM = new List<AlbumViewModel>();
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());

            foreach (var albumDto in albumsDto)
            {
                AlbumViewModel albumVM = Mapper.Map<AlbumDTO, AlbumViewModel>(albumDto);
                albumsVM.Add(albumVM);
            }

            return View(albumsVM.AsEnumerable());
        }

        public ActionResult MyAlbums()
        {
            IQueryable<AlbumDTO> albumsDto = AlbumService.GetAll()
                .Where(x => x.UserId == User.Identity.GetUserId());
            ICollection<AlbumViewModel> albumsVM = new List<AlbumViewModel>();
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());

            foreach (var albumDto in albumsDto)
            {
                AlbumViewModel albumVM = Mapper.Map<AlbumDTO, AlbumViewModel>(albumDto);
                albumsVM.Add(albumVM);
            }
            return View(albumsVM.AsEnumerable());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            AlbumDTO albumDto = await AlbumService.FindByIdAsync((int)id);
            if (albumDto == null)
            {
                return HttpNotFound();
            }

            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());
            AlbumViewModel albumVM = Mapper.Map<AlbumDTO, AlbumViewModel>(albumDto);            

            if (albumVM.Public == false && albumVM.UserId != User.Identity.GetUserId())
            {
                return View("PrivateAlbum");
            }

            UserDTO userDto = await UserService.FindByIdAsync(albumDto.UserId);
            albumVM.UserName = userDto.UserName;

            IQueryable<PostDTO> posts = PostService.GetAll()
                .Where(x => x.AlbumId == albumDto.Id);
            albumVM.Posts = new List<PostViewModel>();
            foreach (var p in posts)
            {
                PostViewModel post = new PostViewModel()
                {
                    AlbumId = p.AlbumId,
                    CreatedAt = p.CreatedAt,
                    Description = p.Description,
                    Id = p.Id,
                    UserId = albumDto.UserId
                };
                albumVM.Posts.Add(post);
            }

            return View(albumVM);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(AlbumViewModel albumVM)
        {
            if (ModelState.IsValid)
            {
                albumVM.CreatedAt = DateTime.Now;
                albumVM.UserId = User.Identity.GetUserId();
                Mapper.Initialize(cfg => cfg.CreateMap<AlbumViewModel, AlbumDTO>());
                AlbumDTO albumDto = Mapper.Map<AlbumViewModel, AlbumDTO>(albumVM);

                var result = await AlbumService.CreateAsync(albumDto);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            return View(albumVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            AlbumDTO albumDto = await AlbumService.FindByIdAsync(id);
            if (albumDto.UserId == User.Identity.GetUserId())
            {
                await AlbumService.DeleteByIdAsync(id);
            }
            return RedirectToAction("MyAlbums");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            AlbumDTO albumDto = await AlbumService.FindByIdAsync((int)id);

            if (albumDto == null)
            {
                return HttpNotFound();
            }

            if (albumDto.UserId == User.Identity.GetUserId())
            {
                Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());
                AlbumViewModel albumVM = Mapper.Map<AlbumDTO, AlbumViewModel>(albumDto);
                return View(albumVM);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AlbumViewModel albumVM)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<AlbumViewModel, AlbumDTO>());
                AlbumDTO albumDto = Mapper.Map<AlbumViewModel, AlbumDTO>(albumVM);

                var result = await AlbumService.UpdateByIdAsync(albumDto);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            return View(albumVM);
        }
    }
}