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
            var newEntity = new Entity { 
                UserId = Guid.Parse(authorId),
                PublicationDate = DateTime.Now,
                ReactionsCounter = 0,
                Post = post 
            };

            await _context.Entities.AddAsync(newEntity);

            return newEntity;
        }

        public Task DeletePost(Post post)
        {
            _context.Posts.Remove(post);

            return Task.CompletedTask;
        }

        public Task<List<Post>> GetAllPosts()
        {
            return _context.Posts.Include(p => p.Entity).ToListAsync();
        }

        public Task<Post> GetPostById(int PostId)
        {
            return _context.Posts.Include(p => p.Entity).FirstOrDefaultAsync(p => p.EntityId == PostId);
        }

        public Task<List<Post>> GetUserPosts(Guid UserId)
        {
            return _context.Posts.Include(p => p.Entity).Where(p => p.Entity.UserId == UserId).ToListAsync();
        }

        public Task UpdatePost(Post post)
        {
            _context.Posts.Update(post);

            return Task.CompletedTask;
        }
    }
}
