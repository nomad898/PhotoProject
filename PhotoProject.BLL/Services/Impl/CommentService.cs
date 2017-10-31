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
    public class CommentService : ICommentService
    {
        private IUnitOfWork db;

        public CommentService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<OperationDetails> CreateAsync(CommentDTO item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<CommentDTO, Comment>());

            Comment comment = Mapper.Map<CommentDTO, Comment>(item);

            if (comment != null)
            {
                db.Comments.Create(comment);

                await db.SaveAsync();

                return new OperationDetails(true, "Comment created", "");
            }
            else
            {
                return new OperationDetails(false, "Comment not created", "Comment");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(int id)
        {
            Comment comment = db.Comments.FindById(id);
            if (comment != null)
            {
                db.Comments.Delete(comment);
                await db.SaveAsync();
                return new OperationDetails(true, "Comment deleted", "");
            }
            else
            {
                return new OperationDetails(false, "Comment not deleted", "CommentId");
            }
        }

        public async Task<CommentDTO> FindByIdAsync(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Comment, CommentDTO>());
            Comment comment = await db.Comments.FindByIdAsync(id);
            if (comment != null)
            {
                CommentDTO commentDto = Mapper.Map<Comment, CommentDTO>(comment);
                return commentDto;
            }
            else return null;
        }

        public IQueryable<CommentDTO> GetAll()
        {
            ICollection<CommentDTO> result = new List<CommentDTO>();
            var comments = db.Comments.GetAll();

            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    CommentDTO commentDto = new CommentDTO()
                    {
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt,
                        Id = comment.Id,
                        PostId = comment.PostId,
                        UserId = comment.UserId
                    };
                    result.Add(commentDto);
                }
            }

            return result.AsQueryable();
        }
    }
}
