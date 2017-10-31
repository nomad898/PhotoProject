using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoProject.BLL.DTO;
using PhotoProject.DAL.Repositories;
using AutoMapper;
using PhotoProject.DAL.Entities;
using PhotoProject.BLL.Infrastructure;

namespace PhotoProject.BLL.Services.Impl
{
    public class AlbumService : IAlbumService
    {
        private IUnitOfWork db;

        public AlbumService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<OperationDetails> CreateAsync(AlbumDTO item)
        {          
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, Album>());
            Album album = Mapper.Map<AlbumDTO, Album>(item);

            if (album != null)
            {
                db.Albums.Create(album);

                await db.SaveAsync();

                return new OperationDetails(true, "Album created", "");
            }
            else
            {
                return new OperationDetails(true, "Album not created", "Album");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(int id)
        {
            Album album = db.Albums.FindById(id);
            if (album != null)
            {
                db.Albums.Delete(album);
                await db.SaveAsync();
                return new OperationDetails(true, "Album deleted", "");
            }
            else
            {
                return new OperationDetails(true, "Album not deleted", "AlbumId");
            }
        }

        public async Task<AlbumDTO> FindByIdAsync(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Album, AlbumDTO>());
            Album album = await db.Albums.FindByIdAsync(id);
            if (album != null)
            {
                AlbumDTO albumDto = Mapper.Map<Album, AlbumDTO>(album);
                return albumDto;
            }
            else return null;
        }

        public IQueryable<AlbumDTO> FindByName(string name)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Album, AlbumDTO>());
            var albums = db.Albums.GetAll().Where(x => x.Title == name);
            ICollection<AlbumDTO> albumsDTO = new List<AlbumDTO>();
            if (albums != null)
            {
                foreach (var album in albums)
                {
                    AlbumDTO albumDto = Mapper.Map<Album, AlbumDTO>(album);
                    albumsDTO.Add(albumDto);
                }
            }
            return albumsDTO.AsQueryable();
        }

        public IQueryable<AlbumDTO> GetAll()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Album, AlbumDTO>());
            ICollection<AlbumDTO> result = new List<AlbumDTO>();
            var albums = db.Albums.GetAll();

            if (albums != null)
            {
                foreach (var album in albums)
                {
                    AlbumDTO albumDto = Mapper.Map<Album, AlbumDTO>(album);
                    result.Add(albumDto);
                }
            }
            return result.AsQueryable();
        }        

        public async Task<OperationDetails> UpdateByIdAsync(AlbumDTO albumDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<AlbumDTO, Album>());
      
            Album album = Mapper.Map<AlbumDTO, Album>(albumDto);
          
            if (album != null)
            {
                db.Albums.Update(album);
                await db.SaveAsync();
                return new OperationDetails(true, "Album updated", "");
            }
            else
            {
                return new OperationDetails(true, "Album not updated", "AlbumId");
            }
        }
    }
}
