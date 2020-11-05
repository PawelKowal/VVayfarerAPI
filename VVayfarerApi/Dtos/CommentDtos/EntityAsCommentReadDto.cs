using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Dtos
{
    public class EntityAsCommentReadDto
    {
        public int EntityId { get; set; }
        public Guid UserId { get; set; }
        public int ReactionsCounter { get; set; }
        public DateTime PublicationDate { get; set; }
        [JsonProperty(PropertyName = "Content")]
        public string CommentContent { get; set; }
        [JsonProperty(PropertyName = "TargetEntityId")]
        public int CommentTargetEntityId { get; set; }
    }
}
