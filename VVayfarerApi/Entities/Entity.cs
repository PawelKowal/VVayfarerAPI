using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Entities
{
    public class Entity
    {
        public int EntityId { get; set; }
        public int ReactionsCounter { get; set; }
        public DateTime PublicationDate { get; set; }

        [InverseProperty("Entity")]
        public List<Reaction> Reactions { get; set; }

        [InverseProperty("TargetEntity")]
        public List<Comment> Comments { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public Comment Comment { get; set; }
        public Post Post { get; set; }
    }
}
