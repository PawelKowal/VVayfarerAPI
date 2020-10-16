using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VVayfarerApi.Models
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
