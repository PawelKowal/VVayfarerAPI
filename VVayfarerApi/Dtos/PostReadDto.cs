using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Dtos
{
    public class PostReadDto
    {
        public int EntityId { get; set; }
        public Guid UserId { get; set; }
        public List<Comment> Comments { get; set; }
        public string PostContent { get; set; }
        public string PostLocation { get; set; }
        public string PostImage { get; set; }
    }
}
