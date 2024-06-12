using Repository.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Modals
{
    public class Attendance
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Students")]
        public int StudentId { get; set; }

        [ForeignKey("Teachers")]
        public int TeacherId { get; set; }

        [Required]
        public Classes ClassId { get; set; }

        [ForeignKey("Subject")]

        public int SubjectId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsPresent { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }

        public Teachers Teachers { get; set; }
        public Subject Subjects { get; set; }
        public Students Students { get; set; }

    }
}
