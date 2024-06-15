using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class StudentAttendanceDetails
    {

        public string SubjectName { get; set; } 

        public bool IsPresent { get; set; }

        public DateTime  Date {  get; set; }
    }
}
