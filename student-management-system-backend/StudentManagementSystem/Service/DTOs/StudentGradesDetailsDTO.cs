using Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class StudentGradesDetailsDTO
    {
        public int gradeId { get; set; }

        public string Name { get; set; }

        public Classes ClassId { get; set; }

        public string SubjectName { get; set; }

        public int Marks { get; set; }

        public int TotalMarks { get; set; }

        public DateTime Date { get; set; }
    }
}
