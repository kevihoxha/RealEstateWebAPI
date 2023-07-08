using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.DTO
{

    public class AuthRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
