using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Dtos;
using VVayfarerApi.Entities;
using VVayfarerApi.Models;

namespace VVayfarerApi.Repositories
{
    public interface IPostRepository
    {
        ValueTask<Entity> GetPostById(int PostId);
        Task<List<Entity>> GetAllPosts();
        Task<List<Entity>> GetUserPosts(Guid UserId);
        Task<Entity> AddPost(Post post, string authorId);
        Task UpdatePost(Entity post);
        Task DeletePost(Entity post);
    }
}
