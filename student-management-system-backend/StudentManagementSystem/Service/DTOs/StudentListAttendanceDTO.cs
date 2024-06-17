using Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class StudentListAttendanceDTO
    {
        public List<StudentAttendanceDTO> Attendances { get; set; }
        public DateTime Date { get; set; }

       
    }
}
