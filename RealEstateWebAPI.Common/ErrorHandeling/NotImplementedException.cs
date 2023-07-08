using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.Common.ErrorHandeling
{
    public class NotImplementedException : Exception
    {
        public NotImplementedException(string message) : base(message) { }
    }
}
