using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Entities
{
    public class Reaction
    {
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int EntityId { get; set; }
        [ForeignKey("EntityId")]
        public virtual Entity Entity { get; set; }
    }
}
