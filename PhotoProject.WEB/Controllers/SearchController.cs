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
            var albumsDto = albumService.GetAll()
                .Where(a => a.Public == true);
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, AlbumViewModel>());
            ICollection<AlbumViewModel> albums = new List<AlbumViewModel>();

            if (albumsDto != null)
            {
                foreach (var albumDto in albumsDto)
                {
                    AlbumViewModel albumVM = Mapper.Map<AlbumDTO, AlbumViewModel>(albumDto);
                    albums.Add(albumVM);
                }
            }
            return View(albums.AsEnumerable());
        }

        [HttpPost]
        public async Task<ActionResult> Index(string title)
        {
            var albumsDto = albumService.GetAll()
                .Where(a => a.Title == title || title == null);
            ICollection<AlbumViewModel> albums = new List<AlbumViewModel>();

            if (albumsDto != null)
            {
                foreach (var albumDto in albumsDto)
                {
                    UserDTO user = await userService.FindByIdAsync(albumDto.UserId);
                    AlbumViewModel albumVM = new AlbumViewModel()
                    {
                        CreatedAt = albumDto.CreatedAt,
                        Description = albumDto.Description,
                        Id = albumDto.Id,
                        Public = albumDto.Public,
                        Title = albumDto.Title,
                        UserId = albumDto.UserId,
                        UserName = user.UserName
                    };
                    albums.Add(albumVM);
                }
            }
            return View(albums.AsEnumerable());
        }
    }
}