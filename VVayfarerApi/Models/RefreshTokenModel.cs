using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Models
{
    [Owned]
    public class RefreshTokenModel
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
