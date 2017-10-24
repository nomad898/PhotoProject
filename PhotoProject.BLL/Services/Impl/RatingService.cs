using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoProject.BLL.DTO;
using PhotoProject.DAL.Repositories;
using AutoMapper;
using PhotoProject.DAL.Entities;
using PhotoProject.BLL.Util;

namespace PhotoProject.BLL.Services.Impl
{
    public class RatingService : IRatingService
    {
        private IUnitOfWork db;

        public RatingService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<OperationDetails> CreateAsync(RatingDTO item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RatingDTO, Rating>());

            Rating rating = Mapper.Map<RatingDTO, Rating>(item);

            if (rating != null)
            {
                db.Ratings.Create(rating);

                await db.SaveAsync();

                return new OperationDetails(true, "Rating created", "");
            }
            else
            {
                return new OperationDetails(false, "Rating not created", "Rating");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(int id)
        {
            Rating rating = db.Ratings.FindById(id);
            if (rating != null)
            {
                db.Ratings.Delete(rating);
                await db.SaveAsync();
                return new OperationDetails(false, "Rating deleted", "");
            }
            else
            {
                return new OperationDetails(true, "Rating not deleted", "RatingId");
            }
        }

        public async Task<RatingDTO> FindByIdAsync(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Rating, RatingDTO>());
            Rating rating = await db.Ratings.FindByIdAsync(id);
            if (rating != null)
            {
                RatingDTO ratingDto = Mapper.Map<Rating, RatingDTO>(rating);
                return ratingDto;
            }
            else return null;
        }

        public async Task<RatingDTO> FindByUserIdAsync(string userId)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Rating, RatingDTO>());
            Rating rating = await db.Ratings.FindByUserIdAsync(userId);
            if (rating != null)
            {
                RatingDTO ratingDto = Mapper.Map<Rating, RatingDTO>(rating);
                return ratingDto;
            }
            else return null;
        }

        public IQueryable<RatingDTO> GetAll()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Rating, RatingDTO>());
            ICollection<RatingDTO> result = new List<RatingDTO>();
            var ratings = db.Ratings.GetAll();

            if (ratings != null)
            {
                foreach (var rating in ratings)
                {
                    RatingDTO ratingDto = Mapper.Map<Rating, RatingDTO>(rating);
                    result.Add(ratingDto);
                }
            }

            return result.AsQueryable();
        }

        public async Task<OperationDetails> UpdateByIdAsync(RatingDTO ratingDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RatingDTO, Rating>());

            Rating rating = Mapper.Map<RatingDTO, Rating>(ratingDto);

            if (rating != null)
            { 
                db.Ratings.Update(rating);
                await db.SaveAsync();
                return new OperationDetails(true, "Rating updated", "");
            }
            else
            {
                return new OperationDetails(true, "Rating not updated", "RatingId");
            }
        }
    }
}
