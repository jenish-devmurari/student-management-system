using Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class TeacherRegisterDTO : BaseDTO
    {

        public Classes Class { get; set; }

        public int Subject { get; set; }

        public string Qualification { get; set; }

        public float Salary { get; set; }

    }
}
