using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class BaseDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfEnrollment { get; set; }
    }
}
