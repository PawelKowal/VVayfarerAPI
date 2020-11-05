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
        Task<Post> GetPostById(int PostId);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetUserPosts(Guid UserId);
        Task<Entity> AddPost(Post post, string authorId);
        Task UpdatePost(Post post);
        Task DeletePost(Post post);
    }
}
