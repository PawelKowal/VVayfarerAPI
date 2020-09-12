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
using VVayfarerApi.Dtos;
using VVayfarerApi.Models;
using VVayfarerApi.Services;

namespace VVayfarerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //GET api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var userItems = await _userService.GetAllUsers();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItems));
        }

        //GET api/user/{id}
        [HttpGet("{Id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            var userItem = await _userService.GetUserById(Id);
            if (userItem != null)
            {
                return Ok(_mapper.Map<UserReadDto>(userItem));
            }
            return NotFound();
        }

        //PATCH api/user/{id}
        [HttpPatch("{Id}")]
        [Authorize]
        public async Task<IActionResult> UserUpdate(string Id, JsonPatchDocument<UserUpdateDto> patchDoc)
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (authorizedUserId.Value != Id)
            {
                return Unauthorized();
            }

            var userModelFromRepo = await _userService.GetUserById(Id);
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

            await _userService.UpdateUser(userModelFromRepo);

            return NoContent();
        }
    }
}
