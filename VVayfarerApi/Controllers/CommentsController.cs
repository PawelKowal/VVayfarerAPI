using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VVayfarerApi.Data;
using VVayfarerApi.Dtos;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CommentsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Comments/Post
        [HttpGet("post/{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetPostComments(int id)
        {
            var comments = await _uow.CommentRepository.GetPostComments(id);

            return Ok(_mapper.Map<IEnumerable<CommentReadDto>>(comments));
        }

        // GET: api/Comments/User
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetUserComments(Guid id)
        {
            var comments = await _uow.CommentRepository.GetUserComments(id);

            return Ok(_mapper.Map<IEnumerable<CommentReadDto>>(comments));
        }

        // GET: api/Comments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetComment(int id)
        {
            var comment = await _uow.CommentRepository.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommentReadDto>(comment));
        }

        // PATCH: api/Comments/5
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> CommentUpdate(int id, JsonPatchDocument<CommentUpdateDto> patchDoc)
        {
            var commentModelFromRepo = await _uow.CommentRepository.GetCommentById(id);

            if (commentModelFromRepo == null)
            {
                return NotFound();
            }

            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (commentModelFromRepo.UserId.ToString() != authorizedUserId.Value)
            {
                return Unauthorized();
            }

            var commentToPatch = _mapper.Map<CommentUpdateDto>(commentModelFromRepo.Comment);
            patchDoc.ApplyTo(commentToPatch, ModelState);

            if (!TryValidateModel(commentToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commentToPatch, commentModelFromRepo.Comment);

            await _uow.CommentRepository.UpdateComment(commentModelFromRepo);

            _uow.SaveChanges();

            return NoContent();
        }

        // POST: api/Comments
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Post>> AddComment([FromBody] CommentAddDto comment)
        {
            if (ModelState.IsValid)
            {
                var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

                if (authorizedUserId == null)
                {
                    return Unauthorized();
                }

                if (! await _uow.CommentRepository.IfEntityExists(comment.TargetEntityId))
                {
                    return BadRequest();
                }

                var newComment = await _uow.CommentRepository.AddComment(_mapper.Map<Comment>(comment), authorizedUserId.Value);
                try
                {
                    _uow.SaveChanges();
                }
                catch
                {
                    return BadRequest();
                }
                return Ok(_mapper.Map<CommentReadDto>(newComment));
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Comments/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeleteComment(int id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                return Unauthorized();
            }

            var comment = await _uow.CommentRepository.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (authorizedUserId.Value != comment.UserId.ToString())
            {
                return Unauthorized();
            }

            await _uow.CommentRepository.DeleteComment(comment);

            _uow.SaveChanges();

            return Ok(_mapper.Map<CommentReadDto>(comment));
        }
    }
}
