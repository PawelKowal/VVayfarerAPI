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
        ValueTask<Entity> GetCommentById(int CommentId);
        Task<List<Entity>> GetPostComments(int PostId);
        Task<List<Entity>> GetUserComments(Guid UserId);
        Task UpdateComment(Entity comment);
        Task DeleteComment(Entity comment);
        Task<bool> IfEntityExists(int id);
    }
}
