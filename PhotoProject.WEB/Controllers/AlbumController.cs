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
        private readonly IUserService userService;
        private readonly IAlbumService albumService;
        private readonly IPostService postService;

        public AlbumController(IUserService userService, IPostService postService, IAlbumService albumService)
        {
            this.userService = userService;
            this.postService = postService;
            this.albumService = albumService;
        }

        public ActionResult Index()
        {
            IQueryable<AlbumDTO> albumsDto =
                albumService.GetAll().Where(a => a.Public == true);
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());
            var albumsVM = Mapper.Map<IQueryable<AlbumDTO>, IEnumerable<AlbumViewModel>>(albumsDto);
            return View(albumsVM);
        }

        public ActionResult MyAlbums()
        {
            IQueryable<AlbumDTO> albumsDto = albumService.GetAll()
                .Where(x => x.UserId == User.Identity.GetUserId());
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());
            var albumsVM = Mapper.Map<IQueryable<AlbumDTO>, IEnumerable<AlbumViewModel>>(albumsDto);
            return View(albumsVM);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            AlbumDTO albumDto = await albumService.FindByIdAsync((int)id);
            if (albumDto == null)
            {
                return HttpNotFound();
            }
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>()
            );
            AlbumViewModel albumVM = Mapper.Map<AlbumDTO, AlbumViewModel>(albumDto);

            if (albumVM.Public == false && albumVM.UserId != User.Identity.GetUserId())
            {
                return View("PrivateAlbum");
            }

            UserDTO userDto = await userService.FindByIdAsync(albumDto.UserId);
            albumVM.UserName = userDto.UserName;

            IQueryable<PostDTO> posts = postService.GetAll()
                .Where(x => x.AlbumId == albumDto.Id);
            Mapper.Initialize(cfg => cfg.CreateMap<PostDTO, PostViewModel>());
            albumVM.Posts = Mapper.Map<IQueryable<PostDTO>, IEnumerable<PostViewModel>>(posts);

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

                var result = await albumService.CreateAsync(albumDto);
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
            AlbumDTO albumDto = await albumService.FindByIdAsync(id);
            if (albumDto.UserId == User.Identity.GetUserId())
            {
                await albumService.DeleteByIdAsync(id);
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

            AlbumDTO albumDto = await albumService.FindByIdAsync((int)id);

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

                var result = await albumService.UpdateByIdAsync(albumDto);
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