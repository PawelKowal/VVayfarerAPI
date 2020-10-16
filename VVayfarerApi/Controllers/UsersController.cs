using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VVayfarerApi.Dtos;
using VVayfarerApi.Services;

namespace VVayfarerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        //GET /api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var userItems = await _userService.GetAllUsers();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItems));
        }
    }
}
