using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Data;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private VVayfarerDbContext _context;
        public PostRepository(VVayfarerDbContext context)
        {
            _context = context;
        }

        public async Task<Entity> AddPost(Post post, string authorId)
        {
            var newEntity = new Entity { UserId = Guid.Parse(authorId), Post = post };

            newEntity.PublicationDate = DateTime.Now;
            newEntity.ReactionsCounter = 0;

            await _context.Entities.AddAsync(newEntity);

            return newEntity;
        }

        public Task DeletePost(Entity post)
        {
            _context.Entities.Remove(post);

            return Task.CompletedTask;
        }

        public Task<List<Entity>> GetAllPosts()
        {
                return _context.Entities
                    .Include(entity => entity.Post)
                    .ToListAsync();
        }

        public ValueTask<Entity> GetPostById(int PostId)
        {
            return _context.Entities.FindAsync(PostId);
        }

        public Task<List<Entity>> GetUserPosts(Guid UserId)
        {
            return _context.Entities.Where(e => e.UserId == UserId && e.Post != null).ToListAsync();
        }

        public Task UpdatePost(Entity post)
        {
            _context.Entities.Update(post);

            return Task.CompletedTask;
        }
    }
}
