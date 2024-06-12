using Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class TeacherRegisterDTO
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public Classes Class { get; set; }

        public int Subject { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfEnrollment { get; set; }

        public string Qualification { get; set; }

        public float Salary { get; set; }

    }
}
