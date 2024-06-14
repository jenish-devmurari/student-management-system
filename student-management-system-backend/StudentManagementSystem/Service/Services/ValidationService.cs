using Repository.Interfaces;
using Repository.Repository;
using Service.DTOs;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ValidationService : IValidationService
    {

        private readonly IUserRepository _userRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;

        public ValidationService(IUserRepository userRepository, ITeacherRepository teacherRepository, IStudentRepository studentRepository)
        {
            _userRepository = userRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
        }
        public async Task<ResponseDTO> ValidateTeacherRegistrationAsync(TeacherRegisterDTO teacherRegisterDTO)
        {
            // Name validation
            if (string.IsNullOrEmpty(teacherRegisterDTO.Name.Trim()))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Name is required."
                };
            }

            // Email validation
            if (await _userRepository.IsEmailExist(teacherRegisterDTO.Email))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Email already registered."
                };
            }

            if (string.IsNullOrEmpty(teacherRegisterDTO.Email?.Trim()) || !Regex.IsMatch(teacherRegisterDTO.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Invalid email format."
                };
            }

            // Date of birth validation
            if (teacherRegisterDTO.DateOfBirth >= DateTime.Now.Date)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of birth must be before the current date."
                };
            }

            // Date of enrollment validation
            if (teacherRegisterDTO.DateOfEnrollment <= teacherRegisterDTO.DateOfBirth)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of enrollment must be after the date of birth."
                };
            }

            // Class validation
            if ((int)teacherRegisterDTO.Class > 12 || (int)teacherRegisterDTO.Class < 1)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Only 1 to 12 classes are available."
                };
            }

            // Teacher existence validation
            if (await _teacherRepository.IsTeacherExistInClass(teacherRegisterDTO.Class, teacherRegisterDTO.Subject))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "A teacher already exists for this class and subject."
                };
            }

            // Salary validation
            if (teacherRegisterDTO.Salary < 0)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Salary cannot be negative."
                };
            }

            return new ResponseDTO { Status = 200 };
        }


        public async Task<ResponseDTO> ValidateTeacherUpdateAsync(TeacherUpdateDTO teacherRegisterDTO)
        {
            // Name validation
            if (string.IsNullOrEmpty(teacherRegisterDTO.Name.Trim()))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Name is required."
                };
            }


            // Date of birth validation
            if (teacherRegisterDTO.DateOfBirth >= DateTime.Now.Date)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of birth must be before the current date."
                };
            }

            // Date of enrollment validation
            if (teacherRegisterDTO.DateOfEnrollment <= teacherRegisterDTO.DateOfBirth)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of enrollment must be after the date of birth."
                };
            }


            // Salary validation
            if (teacherRegisterDTO.Salary < 0)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Salary cannot be negative."
                };
            }

            return new ResponseDTO { Status = 200 };
        }

        public async Task<ResponseDTO> ValidateStudentRegistrationAsync(StudentRegisterDTO studentRegisterDTO)
        {
            // Name validation
            if (string.IsNullOrEmpty(studentRegisterDTO.Name.Trim()))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Name is required."
                };
            }

            // Email validation
            if (await _userRepository.IsEmailExist(studentRegisterDTO.Email))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Email already registered."
                };
            }

            if (string.IsNullOrEmpty(studentRegisterDTO.Email?.Trim()) || !Regex.IsMatch(studentRegisterDTO.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Invalid email format."
                };
            }

            // Date of birth validation
            if (studentRegisterDTO.DateOfBirth >= DateTime.Now.Date)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of birth must be before the current date."
                };
            }

            // Date of enrollment validation
            if (studentRegisterDTO.DateOfEnrollment <= studentRegisterDTO.DateOfBirth)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of enrollment must be after the date of birth."
                };
            }

            // Class validation
            if ((int)studentRegisterDTO.Class > 12 || (int)studentRegisterDTO.Class < 1)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Only 1 to 12 classes are available."
                };
            }


            // Roll number validation
            if (studentRegisterDTO.RollNumber < 0)
            {
                if (await _studentRepository.IsRollNumberIsExist(studentRegisterDTO.RollNumber))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Roll number is already registered with a student."
                    };
                }

                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Roll number is not negative"
                };
            }
            return new ResponseDTO { Status = 200 };
        }


        public async Task<ResponseDTO> ValidateStudentUpdateAsync(StudentUpdateDTO studentUpdate)
        {
            // Name validation
            if (string.IsNullOrEmpty(studentUpdate.Name.Trim()))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Name is required."
                };
            }


            // Date of birth validation
            if (studentUpdate.DateOfBirth >= DateTime.Now.Date)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of birth must be before the current date."
                };
            }

            // Date of enrollment validation
            if (studentUpdate.DateOfEnrollment <= studentUpdate.DateOfBirth)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of enrollment must be after the date of birth."
                };
            }

            // Class validation
            if ((int)studentUpdate.ClassId > 12 || (int)studentUpdate.ClassId < 1)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Only 1 to 12 classes are available."
                };
            }

            return new ResponseDTO { Status = 200 };
        }
    }
}

