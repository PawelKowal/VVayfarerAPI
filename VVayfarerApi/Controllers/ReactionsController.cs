using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VVayfarerApi.Data;
using VVayfarerApi.Dtos;
using VVayfarerApi.Dtos.PostDtos;
using VVayfarerApi.Entities;
using VVayfarerApi.Models;

namespace VVayfarerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ReactionsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/reactions/entity-reactions/{id}
        [HttpGet("entity-reactions/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetEntityReactions(int id)
        {
            var users = await _uow.ReactionRepository.GetEntityReactions(id);

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
        }

        // GET: api/reactions/user-reactions/{id}
        [HttpGet("user-post-reactions/{id}")]
        public async Task<ActionResult<IEnumerable<Entity>>> GetUserPostReactions(Guid id)
        {
            var posts = await _uow.ReactionRepository.GetUserPostReactions(id);

            return Ok(_mapper.Map<IEnumerable<EntityAsPostReadDto>>(posts));
        }

        // POST: api/reactions
        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult> AddReaction(int id)
        {
            if (ModelState.IsValid)
            {
                var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

                if (authorizedUserId == null)
                {
                    return Unauthorized();
                }

                if (await _uow.ReactionRepository.IfReactionExist(Guid.Parse(authorizedUserId.Value), id))
                {
                    return BadRequest(new SimpleResponse
                    {
                        Message = "Reaction already exists",
                        IsSuccess = false
                    });
                }

                var result = await _uow.ReactionRepository.AddReaction(Guid.Parse(authorizedUserId.Value), id);

                if (!result.IsSuccess)
                {
                    return BadRequest();
                }
                try
                {
                    _uow.SaveChanges();
                }
                catch
                {
                    return BadRequest();
                }
                return Ok();
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/reactions/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReaction(int id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                return Unauthorized();
            }

            var reaction = await _uow.ReactionRepository.GetReactionByIds(Guid.Parse(authorizedUserId.Value), id);

            if (reaction == null)
            {
                return NotFound();
            }

            await _uow.ReactionRepository.DeleteReaction(reaction);

            try
            {
                _uow.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
