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
    public class PhotoService : IPhotoService
    {
        private IUnitOfWork db;

        public PhotoService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<OperationDetails> CreateAsync(PhotoDTO item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<PhotoDTO, Photo>());

            Photo photo = Mapper.Map<PhotoDTO, Photo>(item);

            if (photo != null)
            {
                await Task.Run(() => db.Photos.Create(photo));

                return new OperationDetails(true, "Photo created", "");
            }
            else
            {
                return new OperationDetails(false, "Photo not created", "Photo");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(int id)
        {
            Photo photo = db.Photos.FindById(id);
            if (photo != null)
            {
                db.Photos.Delete(photo);
                await db.SaveAsync();
                return new OperationDetails(true, "Photo deleted", "");
            }
            else
            {
                return new OperationDetails(false, "Photo not deleted", "PhotoId");
            }
        }

        public async Task<PhotoDTO> FindByIdAsync(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Photo, PhotoDTO>());
            Photo photo = await db.Photos.FindByIdAsync(id);
            if (photo != null)
            {
                PhotoDTO photoDto = Mapper.Map<Photo, PhotoDTO>(photo);
                return photoDto;
            }
            else return null;
        }

        public IQueryable<PhotoDTO> GetAll()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Photo, PhotoDTO>());
            ICollection<PhotoDTO> result = new List<PhotoDTO>();
            var photos = db.Photos.GetAll();

            if (photos != null)
            {
                foreach (var photo in photos)
                {
                    PhotoDTO photoDto = Mapper.Map<Photo, PhotoDTO>(photo);
                    result.Add(photoDto);
                }
            }

            return result.AsQueryable();
        }
    }
}
