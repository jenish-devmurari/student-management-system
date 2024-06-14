using Repository.Modals;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITeacherService
    {
        public Task<ResponseDTO> GetAllStudentOfTeacherClass(int userId);

        public Task<ResponseDTO> MarkAttendance(List<StudentAttendanceDTO> attendanceDTO,int userId);
    }
}
