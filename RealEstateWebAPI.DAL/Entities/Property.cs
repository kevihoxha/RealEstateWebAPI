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

    }
}
