using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Data
{
    public class VVayfarerDbContext: IdentityDbContext
    {
        public VVayfarerDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
