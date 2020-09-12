using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Dtos
{
    public class UserUpdateDto
    {
        public string Image { get; set; }
        public string ProfileDescription { get; set; }
    }
}
