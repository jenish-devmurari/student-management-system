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
    public class Teachers
    {
        [Key]
        public int TeacherId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        public Classes ClassId { get; set; }

        [ForeignKey("Subject")]
        [Required]
        public int SubjectId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Qualification { get; set; }

        [Column(TypeName = "date")]
        public DateTime ModifiedOn { get; set; }


        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        public Users Users { get; set; }
        public Subject Subject { get; set; }

    }
}
