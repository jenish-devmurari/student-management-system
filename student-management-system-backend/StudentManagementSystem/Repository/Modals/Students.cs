using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Enums;

namespace Repository.Modals
{
    public class Students
    {
        [Key]
        public int StudentId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        public Classes ClassId { get; set; }

        [Required]
        public string RollNumber { get; set; }


        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }

        public Users Users { get; set; }

    }
}
