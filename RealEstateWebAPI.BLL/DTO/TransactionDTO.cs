using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.DTO
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public string BuyerName { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime TransactionDate { get; set; }
        public int PropertyId { get; set; }
    }
}
