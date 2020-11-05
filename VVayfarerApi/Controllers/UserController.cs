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

namespace VVayfarerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        //GET api/user
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLoggedUser()
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                return Unauthorized();
            }

            var userItem = await _uow.UserRepository.GetUserById(authorizedUserId.Value);
            if (userItem != null)
            {
                return Ok(_mapper.Map<UserReadDto>(userItem));
            }

            return NotFound();
        }

        //GET api/user/{id}
        [HttpGet("{Id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            var userItem = await _uow.UserRepository.GetUserById(Id);
            if (userItem != null)
            {
                return Ok(_mapper.Map<UserReadDto>(userItem));
            }
            return NotFound();
        }

        //PATCH api/user/{id}
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UserUpdate([FromQuery] string Id, [FromBody] JsonPatchDocument<UserUpdateDto> patchDoc)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId.Value != Id)
            {
                return Unauthorized();
            }

            var userModelFromRepo = await _uow.UserRepository.GetUserById(Id);
            if (userModelFromRepo == null)
            {
                return NotFound();
            }

            var userToPatch = _mapper.Map<UserUpdateDto>(userModelFromRepo);
            patchDoc.ApplyTo(userToPatch, ModelState);

            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToPatch, userModelFromRepo);

            await _uow.UserRepository.UpdateUser(userModelFromRepo);

            _uow.SaveChanges();

            return Ok(_mapper.Map<UserReadDto>(userModelFromRepo));
        }

        //GET /api/user/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userItems = await _uow.UserRepository.GetAllUsers();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItems));
        }
    }
}
