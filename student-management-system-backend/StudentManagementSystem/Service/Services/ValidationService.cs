using Repository.Interfaces;
using Service.DTOs;
using Service.Interfaces;
using System.Text.RegularExpressions;

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
            ResponseDTO nameValidation = ValidateName(teacherRegisterDTO.Name);
            if (nameValidation.Status != 200) return nameValidation;

            ResponseDTO emailValidation = await ValidateEmailAsync(teacherRegisterDTO.Email);
            if (emailValidation.Status != 200) return emailValidation;

            ResponseDTO dobValidation = ValidateDateOfBirth(teacherRegisterDTO.DateOfBirth);
            if (dobValidation.Status != 200) return dobValidation;

            ResponseDTO doeValidation = ValidateDateOfEnrollment(teacherRegisterDTO.DateOfEnrollment, teacherRegisterDTO.DateOfBirth);
            if (doeValidation.Status != 200) return doeValidation;

            ResponseDTO classValidation = ValidateClass((int)teacherRegisterDTO.Class);
            if (classValidation.Status != 200) return classValidation;

            // Teacher existence validation
            if (await _teacherRepository.IsTeacherExistInClass(teacherRegisterDTO.Class, teacherRegisterDTO.Subject))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "A teacher already exists for this class and subject."
                };
            }

            ResponseDTO salaryValidation = ValidateSalary(teacherRegisterDTO.Salary);
            if (salaryValidation.Status != 200) return salaryValidation;

            return new ResponseDTO { Status = 200 };
        }


       

        public async Task<ResponseDTO> ValidateTeacherUpdateAsync(TeacherUpdateDTO teacherUpdateDTO)
        {
            ResponseDTO nameValidation = ValidateName(teacherUpdateDTO.Name);
            if (nameValidation.Status != 200) return nameValidation;

            ResponseDTO dobValidation = ValidateDateOfBirth(teacherUpdateDTO.DateOfBirth);
            if (dobValidation.Status != 200) return dobValidation;

            ResponseDTO doeValidation = ValidateDateOfEnrollment(teacherUpdateDTO.DateOfEnrollment, teacherUpdateDTO.DateOfBirth);
            if (doeValidation.Status != 200) return doeValidation;

            ResponseDTO salaryValidation = ValidateSalary(teacherUpdateDTO.Salary);
            if (salaryValidation.Status != 200) return salaryValidation;

            return new ResponseDTO { Status = 200 };
        }

        

        public async Task<ResponseDTO> ValidateStudentRegistrationAsync(StudentRegisterDTO studentRegisterDTO)
        {
            ResponseDTO nameValidation = ValidateName(studentRegisterDTO.Name);
            if (nameValidation.Status != 200) return nameValidation;

            ResponseDTO emailValidation = await ValidateEmailAsync(studentRegisterDTO.Email);
            if (emailValidation.Status != 200) return emailValidation;

            ResponseDTO dobValidation = ValidateDateOfBirth(studentRegisterDTO.DateOfBirth);
            if (dobValidation.Status != 200) return dobValidation;

            ResponseDTO doeValidation = ValidateDateOfEnrollment(studentRegisterDTO.DateOfEnrollment, studentRegisterDTO.DateOfBirth);
            if (doeValidation.Status != 200) return doeValidation;

            ResponseDTO classValidation = ValidateClass((int)studentRegisterDTO.Class);
            if (classValidation.Status != 200) return classValidation;

            ResponseDTO rollNumberValidation = await ValidateRollNumberAsync(studentRegisterDTO.RollNumber);
            if (rollNumberValidation.Status != 200) return rollNumberValidation;

            return new ResponseDTO { Status = 200 };
        }


       

        public async Task<ResponseDTO> ValidateStudentUpdateAsync(StudentUpdateDTO studentUpdateDTO)
        {
            ResponseDTO nameValidation = ValidateName(studentUpdateDTO.Name);
            if (nameValidation.Status != 200) return nameValidation;

            ResponseDTO dobValidation = ValidateDateOfBirth(studentUpdateDTO.DateOfBirth);
            if (dobValidation.Status != 200) return dobValidation;

            ResponseDTO doeValidation = ValidateDateOfEnrollment(studentUpdateDTO.DateOfEnrollment, studentUpdateDTO.DateOfBirth);
            if (doeValidation.Status != 200) return doeValidation;

            ResponseDTO classValidation = ValidateClass((int)studentUpdateDTO.ClassId);
            if (classValidation.Status != 200) return classValidation;

            return new ResponseDTO { Status = 200 };
        }

        private ResponseDTO ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Name is required."
                };
            }
            return new ResponseDTO { Status = 200 };
        }

        private async Task<ResponseDTO> ValidateEmailAsync(string email)
        {
            if (await _userRepository.IsEmailExist(email))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Email already registered."
                };
            }

            if (string.IsNullOrEmpty(email?.Trim()) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Invalid email format."
                };
            }
            return new ResponseDTO { Status = 200 };
        }

        private ResponseDTO ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth >= DateTime.Now.Date)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of birth must be before the current date."
                };
            }
            return new ResponseDTO { Status = 200 };
        }

        private ResponseDTO ValidateDateOfEnrollment(DateTime dateOfEnrollment, DateTime dateOfBirth)
        {
            if (dateOfEnrollment <= dateOfBirth)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of enrollment must be after the date of birth."
                };
            }
            if (dateOfEnrollment >= DateTime.Now.Date)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Date of enrollment cannot be a future date."
                };
            }
            return new ResponseDTO { Status = 200 };
        }

        private ResponseDTO ValidateClass(int classId)
        {
            if (classId > 12 || classId < 1)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Only 1 to 12 classes are available."
                };
            }
            return new ResponseDTO { Status = 200 };
        }


        private ResponseDTO ValidateSalary(float salary)
        {
            if (salary < 0)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Salary cannot be negative."
                };
            }
            return new ResponseDTO { Status = 200 };
        }

        private async Task<ResponseDTO> ValidateRollNumberAsync(int rollNumber)
        {
            if (rollNumber <= 0)
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Roll number must be a positive value."
                };
            }

            if (await _studentRepository.IsRollNumberIsExist(rollNumber))
            {
                return new ResponseDTO
                {
                    Status = 400,
                    Message = "Roll number is already registered with a student."
                };
            }
            return new ResponseDTO { Status = 200 };
        }
    }
}

