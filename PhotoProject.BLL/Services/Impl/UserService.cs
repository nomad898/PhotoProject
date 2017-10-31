using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoProject.BLL.DTO;
using PhotoProject.DAL.Repositories;
using AutoMapper;
using PhotoProject.DAL.Entities;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using PhotoProject.BLL.Infrastructure;

namespace PhotoProject.BLL.Services.Impl
{
    public class UserService : IUserService
    {
        private IUnitOfWork db;

        public UserService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<ClaimsIdentity> AuthenticateAsync(UserDTO userDto)
        {
            ClaimsIdentity claim = null;

            AppUser user = await db.UserManager.FindAsync
                (userDto.UserName, userDto.Password);

            if (user != null)
            {
                claim = await db.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public async Task<OperationDetails> CreateAsync(UserDTO userDto)
        {
            AppUser user = await db.UserManager.FindByNameAsync(userDto.UserName);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = userDto.UserName,
                    Avatar = userDto.Avatar,
                    CreatedAt = userDto.CreatedAt
                };

                var result =
                    await db.UserManager.CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                await db.UserManager.AddToRoleAsync(user.Id, userDto.Role);

                await db.SaveAsync();

                return new OperationDetails(true, "Registration successful", "");
            }
            else
            {
                return new OperationDetails(false, "This username already exists", "Username");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(string id)
        {
            AppUser user = await db.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                await db.UserManager.DeleteAsync(user);

                await db.SaveAsync();
                return new OperationDetails(true, "User deleted", "");
            }
            else
            {
                return new OperationDetails(false, "User not deleted", "UserId");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<UserDTO> FindByIdAsync(string id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<AppUser, UserDTO>());
            AppUser user = await db.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                UserDTO userDto = Mapper.Map<AppUser, UserDTO>(user);
                return userDto;
            }
            else return null;
        }

        public async Task<UserDTO> FindByNameAsync(string userName)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<AppUser, UserDTO>());

            AppUser user = await db.UserManager.FindByIdAsync(userName);

            if (user != null)
            {
                UserDTO userDto = Mapper.Map<AppUser, UserDTO>(user);
                return userDto;
            }
            else return null;
        }

        public IQueryable<UserDTO> GetAll()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<AppUser, UserDTO>());

            IQueryable<AppUser> users = db.UserManager.Users;
            List<UserDTO> usersDto = new List<UserDTO>();

            foreach (var user in users)
            {
                UserDTO userDto = Mapper.Map<AppUser, UserDTO>(user);
                usersDto.Add(userDto);
            }

            return usersDto.AsQueryable();
        }

        public async Task<OperationDetails> UpdateAsync(UserDTO userDto)
        {
            AppUser user = await db.UserManager.FindByIdAsync(userDto.Id);

            if (user != null)
            {
                user.UserName = userDto.UserName;

                IdentityResult validData =
                    await db.UserManager.UserValidator.ValidateAsync(user);

                if (!validData.Succeeded)
                {
                    return new OperationDetails(false, "User data is invalid", "UserDTO");
                }

                IdentityResult validPass = null;
                if (userDto.Password != string.Empty)
                {
                    validPass
                        = await db.UserManager
                        .PasswordValidator.ValidateAsync(userDto.Password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            db.UserManager.PasswordHasher.HashPassword(userDto.Password);
                    }
                    else
                    {
                        return new OperationDetails(false, "User password is invalid", "Password");
                    }
                }

                if ((validData.Succeeded && validPass == null) ||
                      (validData.Succeeded &&
                      userDto.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await db.UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return new OperationDetails(true, "User updated", "");
                    }
                    else
                    {
                        return new OperationDetails(false, "User not updated", "User");
                    }
                }
            }
            return new OperationDetails(false, "User not updated", "User");
        }

        public async Task<OperationDetails> UpdateByIdAsync(string id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, AppUser>());
            UserDTO userDto = await FindByIdAsync(id);
            AppUser user = Mapper.Map<UserDTO, AppUser>(userDto);

            if (user != null)
            {
                var result = await db.UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await db.SaveAsync();
                    return new OperationDetails(true, "User updated", "");
                }
                else
                {
                    return new OperationDetails(true, "User not updated", "UserId");
                }
            }
            else
            {
                return new OperationDetails(true, "User not updated", "UserId");
            }
        }
    }
}
