using PhotoProject.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoProject.BLL.DTO;
using PhotoProject.DAL.Entities;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoProject.BLL.Util;

namespace PhotoProject.BLL.Services.Impl
{
    public class RoleService : IRoleService
    {
        private IUnitOfWork db;

        public RoleService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<OperationDetails> CreateAsync(RoleDTO roleDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RoleDTO, AppRole>());                       

            AppRole role = await db.RoleManager.FindByNameAsync(roleDto.RoleName);

            if (role == null)
            {
                role = Mapper.Map<RoleDTO, AppRole>(roleDto);

                var result = await db.RoleManager.CreateAsync(role);

                if (result.Errors.Count() > 0)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                await db.SaveAsync();

                return new OperationDetails(true, "Role created", "");
            }
            else
            {
                return new OperationDetails(false, "Role already exists", "RoleName");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(string id)
        {
            AppRole role = await db.RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                await db.RoleManager.DeleteAsync(role);
                await db.SaveAsync();
                return new OperationDetails(true, "Role deleted", "");
            }
            else
            {
                return new OperationDetails(true, "Role not deleted", "RoleId");
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<RoleDTO> FindByIdAsync(string id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<AppRole, RoleDTO>());
            AppRole role = await db.RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                RoleDTO roleDto = Mapper.Map<AppRole, RoleDTO>(role);
                return roleDto;
            }
            else return null;
        }

        public IQueryable<RoleDTO> GetAll()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<AppRole, RoleDTO>());

            IQueryable<AppRole> roles = db.RoleManager.Roles;
            List<RoleDTO> rolesDto = new List<RoleDTO>();

            foreach (var role in roles)
            {
                RoleDTO roleDto = Mapper.Map<AppRole, RoleDTO>(role);
                rolesDto.Add(roleDto);
            }

            return rolesDto.AsQueryable();
        }
    }
}
