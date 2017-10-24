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
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAlbumService AlbumService;
        private IPostService PostService;

        public SearchController(IPostService postService, IAlbumService albumService)
        {
            PostService = postService;
            AlbumService = albumService;
        }

        public ActionResult Index()
        {
            var albumsDto = AlbumService.GetAll()
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
            var albumsDto = AlbumService.GetAll()
                .Where(a => a.Title == title || title == null);
            ICollection<AlbumViewModel> albums = new List<AlbumViewModel>();

            if (albumsDto != null)
            {
                foreach (var albumDto in albumsDto)
                {
                    UserDTO user = await UserService.FindByIdAsync(albumDto.UserId);
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