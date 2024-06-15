using Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class StudentAttendanceDTO
    {
        public int StudentId { get; set; }

        public DateTime Date { get; set; }

        public  bool IsPresent { get; set; }
    }
}
