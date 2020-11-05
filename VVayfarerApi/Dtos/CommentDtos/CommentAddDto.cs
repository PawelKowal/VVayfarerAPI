using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Dtos
{
    public class CommentAddDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int TargetEntityId { get; set; }
    }
}
