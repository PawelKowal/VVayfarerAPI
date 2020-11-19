using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;
using VVayfarerApi.Models;

namespace VVayfarerApi.Repositories
{
    public interface IReactionRepository
    {
        Task<SimpleResponse> AddReaction(Guid UserId, int EntityId);
        Task DeleteReaction(Reaction reaction);
        Task<List<User>> GetEntityReactions(int EntityId);
        Task<List<Entity>> GetUserPostReactions(Guid UserId);
        Task<bool> IfReactionExist(Guid UserId, int EntityId);
        Task<Reaction> GetReactionByIds(Guid UserId, int EntityId);
    }
}
