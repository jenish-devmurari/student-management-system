using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAdminService
    {
        public Task<ResponseDTO> teacherRegister(TeacherRegisterDTO teacherRegisterDTO, int id);
        public Task<ResponseDTO> studentRegister(StudentRegisterDTO studentRegisterDTO, int id);

        public Task<ResponseDTO> GetAllStudentDetailsAsync();

        public Task<ResponseDTO> GetStudentDetailsByIdAsync(int id);

        public Task<ResponseDTO> GetAllTecherDetailsAsync();

        public Task<ResponseDTO> GetTecherDetailsByIdAsync(int id);

        public Task<ResponseDTO> UpdateStudent(StudentUpdateDTO studentUpdate, int id, int Id);

        public Task<ResponseDTO> DeleteStudent(int id);

        public Task<ResponseDTO> UpdateTeacher(TeacherUpdateDTO teacherUpdate, int id);

        public Task<ResponseDTO> DeleteTeacher(int id);
    }
}
