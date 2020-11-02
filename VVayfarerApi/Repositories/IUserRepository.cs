using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Dtos;
using VVayfarerApi.Entities;
using VVayfarerApi.Models;

namespace VVayfarerApi.Repositories
{
    public interface IUserRepository
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginModel model);
        Task<User> GetUserById(string Id);
        Task<List<User>> GetAllUsers();
        Task UpdateUser(User model);
        Task<UserManagerResponse> RefreshTokenAsync(string token);
        Task<UserManagerResponse> LogoutUserAsync(string id);
    }
}
