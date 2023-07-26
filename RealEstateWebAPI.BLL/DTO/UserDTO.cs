using Microsoft.AspNetCore.Identity;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "UserName must contain only letters and numbers.")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 8 characters.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Your Email is not right")]
        public string Email { get; set; }
        [Required]
        [Range(1, 2, ErrorMessage = "RoleId must be 1(admin) or 2(agent).")]
        public int RoleId { get; set; }
    }
}
