using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Models;

namespace VVayfarerApi.Data
{
    public class VVayfarerDbContext: IdentityDbContext<UserModel>
    {
        public VVayfarerDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
