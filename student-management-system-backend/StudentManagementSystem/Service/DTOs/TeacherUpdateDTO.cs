using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class TeacherUpdateDTO : BaseUpdateDTO
    {
        public string Qualification { get; set; }

        public float Salary { get; set; }
    }
}
