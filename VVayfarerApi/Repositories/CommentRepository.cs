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
            var newEntity = new Entity {
                UserId = Guid.Parse(authorId),
                PublicationDate = DateTime.Now,
                ReactionsCounter = 0,
                Comment = comment
            };

            await _context.Entities.AddAsync(newEntity);

            return newEntity;
        }

        public Task DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);

            return Task.CompletedTask;
        }

        public Task<Comment> GetCommentById(int CommentId)
        {
            return _context.Comments.Include(c => c.Entity).FirstOrDefaultAsync(c => c.EntityId == CommentId);
        }

        public Task<List<Comment>> GetEntityComments(int EntityId)
        {
            return _context.Comments.Where(c => c.TargetEntityId == EntityId).Include(c => c.Entity).ToListAsync();
        }

            public Task<List<Comment>> GetUserComments(Guid UserId)
        {
            return _context.Comments.Include(c => c.Entity).Where(c => c.Entity.UserId == UserId).ToListAsync();
        }
               
        public Task UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);

            return Task.CompletedTask;
        }

        public Task<bool> IfEntityExists(int id)
        {
            return _context.Entities.AnyAsync(e => e.EntityId == id);
        }
    }
}
