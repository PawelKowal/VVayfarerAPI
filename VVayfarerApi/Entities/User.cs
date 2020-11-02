using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Entities
{
    public class User: IdentityUser<Guid>
    {
        public string Image { get; set; }
        public string ProfileDescription { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public List<Entity> Entities { get; set; }
        public List<Reaction> Reactions { get; set; }
        
    }
}
