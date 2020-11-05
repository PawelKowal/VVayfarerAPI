using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Dtos.PostDtos
{
    public class EntityAsPostReadDto
    {
        public int EntityId { get; set; }
        public Guid UserId { get; set; }
        public int ReactionsCounter { get; set; }
        public DateTime PublicationDate { get; set; }
        [JsonProperty(PropertyName = "Content")]
        public string PostContent { get; set; }
        [JsonProperty(PropertyName = "Location")]
        public string PostLocation { get; set; }
        [JsonProperty(PropertyName = "Image")]
        public string PostImage { get; set; }
    }
}
