using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(100)]
        public required string userName { get; set; }
        [Required]
        public required string password { get; set; }
    }
}