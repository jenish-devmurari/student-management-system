using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Modals
{
    public class Subject
    {
        [Key] 
        public int SubjectId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string SubjectName { get; set; }

    }
}
