using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Services;
using PhotoProject.BLL.Util;
using PhotoProject.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PhotoProject.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService userService;         

        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            IQueryable<UserDTO> usersDto = userService.GetAll();
            ICollection<UserViewModel> usersVM = new List<UserViewModel>();
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());

            foreach (var user in usersDto)
            {
                if (user.UserName != User.Identity.Name &&
                    user.Role != "Admin")
                {
                    UserViewModel userVM = Mapper.Map<UserDTO, UserViewModel>(user);
                    usersVM.Add(userVM);
                }
            }
            return View(usersVM.AsEnumerable());
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            UserDTO userDto = await userService.FindByIdAsync(id);

            if (userDto != null)
            {
                var result = await userService.DeleteByIdAsync(userDto.Id);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(userDto);
                }
            }
            else
            {
                return View("Error", new string[] { "User not found" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            UserDTO userDto = await userService.FindByIdAsync(id);

            if (userDto == null)
            {
                return HttpNotFound();
            }
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            UserViewModel userVM = Mapper.Map<UserDTO, UserViewModel>(userDto);
            if (userVM != null)
            {
                return View(userVM);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
                UserDTO userDto = Mapper.Map<UserViewModel, UserDTO>(userVM);
                if (userDto != null)
                {
                    OperationDetails result
                        = await userService.UpdateAsync(userDto);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(result.Property, result.Message);
                    }
                }
            }
            return View(userVM);
        }
    }
}