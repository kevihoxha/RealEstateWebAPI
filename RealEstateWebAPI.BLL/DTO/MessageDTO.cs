using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.DTO
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int PropertyId { get; set; }
    }
}
