using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.DTO
{
    public class RoleDTO
    {
        public int RoleId { get; set; }
  
        public string Name { get; set; }
        public Guid UniqueIdentifier { get; set; }
    }
}
