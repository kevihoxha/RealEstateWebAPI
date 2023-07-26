using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.DTO
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "BuyerName must not exceed 50 characters.")]
        public string BuyerName { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "SalePrice must be greater than 0.")]
        public decimal SalePrice { get; set; }
        public DateTime TransactionDate { get; set; }
        public int PropertyId { get; set; }
    }
}
