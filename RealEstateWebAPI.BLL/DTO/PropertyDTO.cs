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
    public class PropertyDTO
    {

        public int PropertyId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title must not exceed 100 characters.")]
        public string Title { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Location must not exceed 100 characters.")]
        public string Location { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        public int UserId { get; set; }

    }
}
