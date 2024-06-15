using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Modals
{
    public class Grades
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Students")]
        public int StudentId { get; set; }

        [ForeignKey("Teachers")]
        public int TeacherId { get; set; }

        [Required]
        public int Marks {  get; set; }

        [Required]
        public int TotalMarks { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
    

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }


        public Teachers Teachers { get; set; }
        public Students Students { get; set; }
    }
}
