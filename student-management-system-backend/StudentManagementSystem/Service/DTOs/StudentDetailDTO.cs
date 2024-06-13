using Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class StudentDetailDTO :BaseDTO
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Classes ClassId { get; set; }
        public int RollNumber { get; set; }
 
    }
}
