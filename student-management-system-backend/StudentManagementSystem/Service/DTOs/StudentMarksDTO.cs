using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class StudentMarksDTO 
    {
        public int userId {  get; set; }
        public int GradeId { get; set; }
        public string Email { get; set; }

        public int SubjectId { get; set; }

        public int Marks { get; set; }

        public int TotalMarks { get; set; }

        public DateTime  Date { get; set; }
    }
}

