using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Repositories
{
    interface IReactionRepository
    {
        Task AddReaction(Guid UserId, int EntityId);
        Task DeleteReaction(Guid UserId, int EntityId);
        Task<List<User>> GetEntityReactions(int EntityId);
        Task<List<Entity>> GetUserReactions(Guid UserId);
    }
}
