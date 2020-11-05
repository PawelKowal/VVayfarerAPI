using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Repositories
{
    public interface ICommentRepository
    {
        Task<Entity> AddComment(Comment comment, string authorId);
        Task<Comment> GetCommentById(int CommentId);
        Task<List<Comment>> GetEntityComments(int PostId);
        Task<List<Comment>> GetUserComments(Guid UserId);
        Task UpdateComment(Comment comment);
        Task DeleteComment(Comment comment);
        Task<bool> IfEntityExists(int id);
    }
}
