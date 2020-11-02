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
using Microsoft.EntityFrameworkCore;
using VVayfarerApi.Data;
using VVayfarerApi.Dtos;
using VVayfarerApi.Entities;
using VVayfarerApi.Models;

namespace VVayfarerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PostsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        { 
            var posts = await _uow.PostRepository.GetAllPosts();

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        // GET: api/Posts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _uow.PostRepository.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostReadDto>(post));
        }

        // GET: api/Posts/user/{id}
        [HttpGet("user/{id}")]
        public async Task<ActionResult<Post>> GetUserPosts(string id)
        {
            var posts = await _uow.PostRepository.GetUserPosts(Guid.Parse(id));

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        // PATCH: api/Posts/5
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> PostUpdate(int id, JsonPatchDocument<PostUpdateDto> patchDoc)
        {
            var postModelFromRepo = await _uow.PostRepository.GetPostById(id);

            if (postModelFromRepo == null)
            {
                return NotFound();
            }

            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (postModelFromRepo.UserId.ToString() != authorizedUserId.Value)
            {
                return Unauthorized();
            }

            var postToPatch = _mapper.Map<PostUpdateDto>(postModelFromRepo.Post);
            patchDoc.ApplyTo(postToPatch, ModelState);

            if (!TryValidateModel(postToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(postToPatch, postModelFromRepo.Post);

            await _uow.PostRepository.UpdatePost(postModelFromRepo);

            _uow.SaveChanges();

            return NoContent();
        }

        // POST: api/Posts
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Post>> AddPost([FromBody] PostAddDto post)
        {
            if (ModelState.IsValid)
            {
                var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

                if (authorizedUserId == null)
                {
                    return Unauthorized();
                }

                var newPost = await _uow.PostRepository.AddPost(_mapper.Map<Post>(post), authorizedUserId.Value);
                try
                {
                    _uow.SaveChanges();
                }
                catch
                {
                    return BadRequest();
                }
                return Ok(_mapper.Map<PostReadDto>(newPost));
            }

            return BadRequest(ModelState);  
        }

        // DELETE: api/Posts/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                return Unauthorized();
            }

            var post = await _uow.PostRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            if (authorizedUserId.Value != post.UserId.ToString())
            {
                return Unauthorized();
            }

            await _uow.PostRepository.DeletePost(post);
            
            _uow.SaveChanges();

            return Ok(_mapper.Map<PostReadDto>(post));
        }
    }
}
