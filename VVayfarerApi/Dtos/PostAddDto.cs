using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Dtos
{
    public class PostAddDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Image { get; set; }
    }
}
