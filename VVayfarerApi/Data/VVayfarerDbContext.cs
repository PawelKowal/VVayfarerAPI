using Microsoft.AspNetCore.Identity;
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserModel>().Property(t => t.UserName).HasMaxLength(50);
        }
    }
}
