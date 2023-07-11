using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
