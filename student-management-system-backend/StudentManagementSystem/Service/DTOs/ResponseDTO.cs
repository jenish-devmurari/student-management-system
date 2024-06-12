using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class ResponseDTO
    {
        public int Status { get; set; }
        public object? Data { get; set; }
        public string Message { get; set; }
        public string? Error { get; set; }
    }
}
