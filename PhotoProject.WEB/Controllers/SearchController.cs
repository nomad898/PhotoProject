using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Services;
using PhotoProject.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PhotoProject.WEB.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly IUserService userService;
        private readonly IAlbumService albumService;
        private readonly IPostService postService;

        public SearchController(IUserService userService,
            IPostService postService,
            IAlbumService albumService)
        {
            this.userService = userService;
            this.postService = postService;
            this.albumService = albumService;
        }

        public ActionResult Index()
        {
            IQueryable<AlbumDTO> albumsDto = albumService.GetAll()
                .Where(a => a.Public == true);
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());
            var albumsVM = Mapper.Map<IQueryable<AlbumDTO>, IEnumerable<AlbumViewModel>>(albumsDto);
            return View(albumsVM);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string title)
        {
            var albumsDto = albumService.GetAll()
                .Where(a => a.Title == title || title == null);
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());
            var albumsVM = Mapper.Map<IQueryable<AlbumDTO>, IEnumerable<AlbumViewModel>>(albumsDto);
            foreach (var album in albumsVM)
            {
                UserDTO user = await userService.FindByIdAsync(album.UserId);
                album.UserName = user.UserName;      
            }
            return View(albumsVM);
        }
    }
}