using Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class StudentRegisterDTO : BaseDTO
    {
            public Classes Class { get; set; }

            public int RollNumber { get; set; }

    }
}
