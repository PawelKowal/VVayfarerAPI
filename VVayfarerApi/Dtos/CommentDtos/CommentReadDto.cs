﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Dtos
{
    public class CommentReadDto
    {
        [JsonProperty(PropertyName = "EntityId")]
        public int EntityEntityId { get; set; }
        [JsonProperty(PropertyName = "UserId")]
        public Guid EntityUserId { get; set; }
        [JsonProperty(PropertyName = "ReactionsCounter")]
        public int EntityReactionsCounter { get; set; }
        [JsonProperty(PropertyName = "PublicationDate")]
        public DateTime EntityPublicationDate { get; set; }
        public string Content { get; set; }
        public int TargetEntityId { get; set; }
    }
}
