﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Infrastructure;
using PhotoProject.BLL.Services;
using PhotoProject.WEB.Models;
using PhotoProject.WEB.Infrastructure.Helpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PhotoProject.WEB.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class AccountController : Controller
    {
        private readonly IUserService userService;        
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        #region Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    UserName = model.UserName,
                    Password = model.Password
                };
                ClaimsIdentity claim = await userService.AuthenticateAsync(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Wrong login or password.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] avatarData = ImageConverter.ConvertImage(registerViewModel.Avatar);

                UserDTO userDto = new UserDTO
                {
                    UserName = registerViewModel.UserName,
                    Password = registerViewModel.Password,
                    Role = "User",
                    CreatedAt = DateTime.Now,
                    Avatar = avatarData
                };

                OperationDetails operationDetails = await userService.CreateAsync(userDto);

                if (operationDetails.Succeeded)
                {
                    return View("SuccessRegister");
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                }
            }
            return View(registerViewModel);
        }
        #endregion
        
        public ActionResult Index()
        {
            return RedirectToAction("Details");
        }

        public async Task<ActionResult> Details()
        {
            UserDTO userDto = await GetCurrentUserAsync();
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            UserViewModel userVM = Mapper.Map<UserDTO, UserViewModel>(userDto);

            return View(userVM);
        }

        private async Task<UserDTO> GetCurrentUserAsync()
        {
            return await userService.FindByIdAsync(User.Identity.GetUserId());
        }
    }
}