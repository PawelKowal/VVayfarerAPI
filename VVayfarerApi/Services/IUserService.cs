using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Models;

namespace VVayfarerApi.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginModel model);
    }
}
