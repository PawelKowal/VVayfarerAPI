using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Entities
{
    [Owned]
    public class Post
    { 
        [Key]
        public int EntityId { get; set; }
        [ForeignKey("EntityId")]
        public Entity Entity { get; set; }

        public string Content { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
    }
}
