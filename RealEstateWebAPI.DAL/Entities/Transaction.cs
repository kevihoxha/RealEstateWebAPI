using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string BuyerName { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime TransactionDate { get; set; }
        
        [ForeignKey(nameof(Property))]
        public int PropertyId { get; set; }
        public Property Property { get; set; }

    }
}
