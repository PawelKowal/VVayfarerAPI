using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Dtos
{
    public class RefreshTokenReadDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
