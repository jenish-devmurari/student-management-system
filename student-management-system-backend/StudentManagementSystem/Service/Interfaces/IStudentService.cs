using Repository.Interfaces;
using Repository.Modals;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IStudentService
    {
        public Task<ResponseDTO> GetAllAttendance(int userId);

        public Task<ResponseDTO> GetStudentSubjectGrades(int subjectId, int userId);

    }
}
