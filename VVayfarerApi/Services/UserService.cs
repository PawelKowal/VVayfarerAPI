using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VVayfarerApi.Dtos;
using VVayfarerApi.Models;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VVayfarerApi.Services
{
    public class UserService : IUserService
    {
        private UserManager<UserModel> _userManager;
        private IConfiguration _configuration;

        public UserService(UserManager<UserModel> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<UserManagerResponse> RegisterUserAsync(RegisterModel model)
        {
            if (model == null) throw new NullReferenceException("Register model is null");

            byte[] imageArray = System.IO.File.ReadAllBytes(@"./Models/defaultAvatar.png");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            var userModel = new UserModel
            {
                Email = model.Email,
                UserName = model.UserName,
                Image = base64ImageRepresentation,
                ProfileDescription = "",
            };

            var result = await _userManager.CreateAsync(userModel, model.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

            foreach (var error in result.Errors)
            {
                errors.Add(error.Code, new string[1] { error.Description });
            }

            return new UserManagerResponse
            {
                Message = "User did not create.",
                IsSuccess = false,
                Errors = errors,
            };

        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that email address.",
                    IsSuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password.",
                    IsSuccess = false,
                };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo,
            };
        }

        public async Task UpdateUser(UserModel model)
        {
            await _userManager.UpdateAsync(model);
        }

        public async Task<UserModel> GetUserById(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}
