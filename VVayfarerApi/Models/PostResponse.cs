using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VVayfarerApi.Dtos;
using VVayfarerApi.Entities;

namespace VVayfarerApi.Models
{
    public class PostResponse
    {
        public Guid AuthorId { get; set; }
        public PostReadDto Post { get; set; }
    }
}
