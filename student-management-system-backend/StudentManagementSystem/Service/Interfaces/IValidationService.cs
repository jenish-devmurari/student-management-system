using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IValidationService
    {
        Task<ResponseDTO> ValidateTeacherRegistrationAsync(TeacherRegisterDTO teacherRegisterDTO);

        Task<ResponseDTO> ValidateStudentRegistrationAsync(StudentRegisterDTO studentRegisterDTO);

        Task<ResponseDTO> ValidateStudentUpdateAsync(StudentUpdateDTO studentUpdate);

        Task<ResponseDTO> ValidateTeacherUpdateAsync(TeacherUpdateDTO teacherRegisterDTO);
    }
}
