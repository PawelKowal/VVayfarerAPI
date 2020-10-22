using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Dtos;
using VVayfarerApi.Models;

namespace VVayfarerApi.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginModel model);
        Task<UserModel> GetUserById(string Id);
        Task<List<UserModel>> GetAllUsers();
        Task UpdateUser(UserModel model);
        Task<UserManagerResponse> RefreshTokenAsync(string token);
        Task<UserManagerResponse> LogoutUserAsync(string id);
    }
}
