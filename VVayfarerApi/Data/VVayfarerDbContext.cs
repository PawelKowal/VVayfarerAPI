using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Data
{
    public class VVayfarerDbContext: IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        public VVayfarerDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(t => t.UserName).HasMaxLength(50);
            builder.Entity<Reaction>().HasKey(c => new { c.EntityId, c.UserId });
        }
    }
}
