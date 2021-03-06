﻿using Microsoft.AspNetCore.Identity;
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
using VVayfarerApi.Entities;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VVayfarerApi.Data;

namespace VVayfarerApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private VVayfarerDbContext _context;
        private UserManager<User> _userManager;
        private IConfiguration _configuration;

        public UserRepository(VVayfarerDbContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<UserManagerResponse> RegisterUserAsync(RegisterModel model)
        {
            if (model == null) throw new NullReferenceException("Register model is null");

            byte[] imageArray = System.IO.File.ReadAllBytes(@"./Models/defaultAvatar.png");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            var userModel = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                Image = base64ImageRepresentation,
                ProfileDescription = "",
                RefreshToken = null,
            };

            var result = await _userManager.CreateAsync(userModel, model.Password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                var accessToken = GenerateAccessToken(user);

                string accessTokenAsString = new JwtSecurityTokenHandler().WriteToken(accessToken);

                var refreshToken = await GenerateRefreshToken(user);

                userModel.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(userModel);

                return new UserManagerResponse
                {
                    Message = accessTokenAsString,
                    IsSuccess = true,
                    RefreshToken = refreshToken.Token,
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

            var accessToken = GenerateAccessToken(user);

            var refreshToken = await GenerateRefreshToken(user);

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            string accessTokenAsString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            return new UserManagerResponse
            {
                Message = accessTokenAsString,
                IsSuccess = true,
                RefreshToken = refreshToken.Token,
            };
        }

        public Task UpdateUser(User model)
        {
            _userManager.UpdateAsync(model);

            return Task.CompletedTask;
        }

        public Task<User> GetUserById(string Id)
        {
            return _userManager.FindByIdAsync(Id);
        }

        public Task<List<User>> GetAllUsers()
        {
            return _userManager.Users.ToListAsync();
        }

        public async Task<UserManagerResponse> RefreshTokenAsync(string token)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshToken.Token == token);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "User not found.",
                    IsSuccess = false,
                };
            }

            var refreshToken = user.RefreshToken;

            if (refreshToken.Expires < DateTime.Now) {
                return new UserManagerResponse
                {
                    Message = "Refresh token is no longer valid.",
                    IsSuccess = false,
                };
            }

            if (refreshToken.Token != token)
            {
                return new UserManagerResponse
                {
                    Message = "Refresh token is not valid.",
                    IsSuccess = false,
                };
            }

            var newRefreshToken = await GenerateRefreshToken(user);

            var newAccessToken = GenerateAccessToken(user);

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            string accessTokenAsString = new JwtSecurityTokenHandler().WriteToken(newAccessToken);

            return new UserManagerResponse
            {
                Message = accessTokenAsString,
                IsSuccess = true,
                RefreshToken = newRefreshToken.Token,
            };
        }

        public async Task<UserManagerResponse> LogoutUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that id.",
                    IsSuccess = false,
                };
            }

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return new UserManagerResponse
            {
                Message = "User logged out.",
                IsSuccess = true,
            };
        }

        //helper methods

        private JwtSecurityToken GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private async Task<RefreshToken> GenerateRefreshToken(User user)
        {
            var token = await _userManager.GenerateUserTokenAsync(user, "MyApp", "RefreshToken");

            return new RefreshToken
            {
                Token = token,
                Expires = DateTime.Now.AddDays(7)
            };
        }
    }
}
