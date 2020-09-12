using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Dtos
{
    public class UserReadDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string ProfileDescription { get; set; }
    }
}
