using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Data;
using VVayfarerApi.Entities;
using VVayfarerApi.Models;

namespace VVayfarerApi.Repositories
{
    public class ReactionRepository : IReactionRepository
    {
        private VVayfarerDbContext _context;
        public ReactionRepository(VVayfarerDbContext context)
        {
            _context = context;
        }

        public async Task<SimpleResponse> AddReaction(Guid UserId, int EntityId)
        {
            var entity = await _context.Entities.FindAsync(EntityId);

            if(entity == null){
                return new SimpleResponse
                {
                    Message = "Entity doesn't exist.",
                    IsSuccess = false
                };
            }

            entity.ReactionsCounter += 1;

            var newReaction = new Reaction
            {
                UserId = UserId,
                EntityId = EntityId
            };

            await _context.Reactions.AddAsync(newReaction);

            return new SimpleResponse
            {
                Message = "Reaction was added",
                IsSuccess = true
            };
        }

        public async Task DeleteReaction(Reaction reaction)
        {
            var entity = await _context.Entities.FirstOrDefaultAsync(e => e.EntityId == reaction.EntityId);

            entity.ReactionsCounter -= 1;

            _context.Entities.Update(entity);

            _context.Reactions.Remove(reaction);
        }

        public Task<List<User>> GetEntityReactions(int EntityId)
        {
            return _context
                .Reactions
                .Where(r => r.EntityId == EntityId)
                .Include(r => r.User)
                .Select(r => r.User)
                .ToListAsync();
        }

        public Task<List<Entity>> GetUserPostReactions(Guid UserId)
        {
            return _context
                .Reactions
                .Where(r => r.UserId == UserId)
                .Include(r => r.Entity)
                .ThenInclude(e => e.Post)
                .Where(r => r.Entity.Post != null)
                .Select(r => r.Entity)
                .ToListAsync();
        }

        public Task<bool> IfReactionExist(Guid UserId, int EntityId)
        {
            return _context.Reactions.AnyAsync(e => e.EntityId == EntityId && e.UserId == UserId);
        }

        public Task<Reaction> GetReactionByIds(Guid UserId, int EntityId)
        {
            return _context.Reactions.FirstOrDefaultAsync(r => r.UserId == UserId && r.EntityId == EntityId);
        }
    }
}
