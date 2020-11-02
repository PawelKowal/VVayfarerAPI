using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Data;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private VVayfarerDbContext _context;
        public CommentRepository(VVayfarerDbContext context)
        {
            _context = context;
        }
        public async Task<Entity> AddComment(Comment comment, string authorId)
        {
            var newEntity = new Entity { UserId = Guid.Parse(authorId), Comment = comment };

            newEntity.PublicationDate = DateTime.Now;
            newEntity.ReactionsCounter = 0;

            await _context.Entities.AddAsync(newEntity);

            return newEntity;
        }

        public Task DeleteComment(Entity comment)
        {
            _context.Entities.Remove(comment);

            return Task.CompletedTask;
        }

        public ValueTask<Entity> GetCommentById(int CommentId)
        {
            return _context.Entities.FindAsync(CommentId);
        }

        public Task<List<Entity>> GetPostComments(int PostId)
        {
            var comments = _context.Entities.Where(e => e.Comment != null).Include(e => e.Comment);

            return comments.Where(c => c.Comment.TargetEntityId == PostId).ToListAsync();
        }

        public Task<List<Entity>> GetUserComments(Guid UserId)
        {
            return _context.Entities.Where(e => e.UserId == UserId && e.Comment != null).ToListAsync();
        }
               
        public Task UpdateComment(Entity comment)
        {
            _context.Entities.Update(comment);

            return Task.CompletedTask;
        }

        public Task<bool> IfEntityExists(int id)
        {
            return _context.Entities.AnyAsync(e => e.EntityId == id);
        }
    }
}
