using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Models
{
    public class UserModel: IdentityUser
    {
        public string Image { get; set; }
        public string ProfileDescription { get; set; }
        public RefreshTokenModel RefreshToken { get; set; }
    }
}
