using RealEstateWebAPI.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL.Entities
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }


    }
}
