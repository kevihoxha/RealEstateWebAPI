using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.DTO
{
    public class MessageDTO
    {
        public int MessageId { get; set; }

        [StringLength(50, ErrorMessage = "SenderName must not exceed 50 characters.")]
        public string SenderName { get; set; }

        [StringLength(50, ErrorMessage = "SenderEmail must not exceed 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string SenderEmail { get; set; }
        [StringLength(200, ErrorMessage = "Content must not exceed 200 characters.")]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int PropertyId { get; set; }
    }
}
