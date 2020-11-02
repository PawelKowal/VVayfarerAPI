using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Dtos
{
    public class CommentReadDto
    {
        public int EntityId { get; set; }
        public Guid UserId { get; set; }
        public List<Comment> Comments { get; set; }
        public string CommentContent { get; set; }
        public int CommentTargetEntityId { get; set; }
    }
}
