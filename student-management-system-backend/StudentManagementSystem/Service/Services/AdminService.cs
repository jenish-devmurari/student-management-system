using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Enums;
using Repository.Interfaces;
using Repository.Modals;
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
    public class AdminService : IAdminService
    {

        #region DI
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPasswordEncryption _passwordEncryption;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public AdminService(IPasswordEncryption passwordEncryption, AppDbContext context, IStudentRepository studentRepository, ITeacherRepository teacherRepository, IUserRepository userRepository)
        {
            _studentRepository = studentRepository;
            _passwordEncryption = passwordEncryption;
            _context = context;
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region teacher register
        public async Task<ResponseDTO> teacherRegister(TeacherRegisterDTO teacherRegisterDTO, int id)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                #region validation for teacher registration
                // name validation
                if (string.IsNullOrEmpty(teacherRegisterDTO.Name.Trim()))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "name is required"
                    };
                }

                // Check if the email already exists
                if (await _userRepository.IsEmailExist(teacherRegisterDTO.Email))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Email already registered."
                    };
                }


                // Validate the email format
                if (string.IsNullOrEmpty(teacherRegisterDTO.Email?.Trim()) || !Regex.IsMatch(teacherRegisterDTO.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Invalid email format."
                    };
                }

                // validate date of birth 
                if (teacherRegisterDTO.DateOfBirth >= DateTime.Now.Date)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Date of birth must be before the current date."
                    };
                }

                // Validate the date of enrollment
                if (teacherRegisterDTO.DateOfEnrollment <= teacherRegisterDTO.DateOfBirth)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Date of enrollment must be after the date of birth."
                    };
                }

                // class validation
                if ((int)teacherRegisterDTO.Class > 12 || (int)teacherRegisterDTO.Class < 1)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Only 1 to 12 classes are available"
                    };
                }

                // Check if a teacher already exists in the class for the given subject
                if (await _teacherRepository.IsTeacherExistInClass(teacherRegisterDTO.Class, teacherRegisterDTO.Subject))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "A teacher already exists for this class and subject."
                    };
                }

                // validation of salary
                if(teacherRegisterDTO.Salary < 0)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "A salary is not negative"
                    };
                }

                #endregion


                var password = $"{teacherRegisterDTO.Name}@{teacherRegisterDTO.DateOfBirth.Year}";
                var hashedPassword = _passwordEncryption.HashPassword(password);

                var user = new Users
                {
                    Name = teacherRegisterDTO.Name,
                    Email = teacherRegisterDTO.Email,
                    Role = Roles.Teacher,
                    Password = hashedPassword,
                    DateOfBirth = teacherRegisterDTO.DateOfBirth,
                    DateOfEnrollment = teacherRegisterDTO.DateOfEnrollment,
                    IsActive = true,
                    IsPasswordReset = true,
                };

                await _userRepository.AddUserAsync(user);

                var userDetail = await _userRepository.GetUsersAsync(teacherRegisterDTO.Email);

                var teacher = new Teachers
                {
                    UserId = userDetail.UserId,
                    Qualification = teacherRegisterDTO.Qualification,
                    ClassId = teacherRegisterDTO.Class,
                    SubjectId = teacherRegisterDTO.Subject,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Salary = teacherRegisterDTO.Salary
                };

                await _teacherRepository.AddTeacherAsync(teacher);

                await transaction.CommitAsync();

                return new ResponseDTO
                {
                    Status = 201,
                    Data = userDetail.UserId,
                    Message = "Teacher is register"
                };

            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();

                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }


        }
        #endregion  

        #region student register
        public async Task<ResponseDTO> studentRegister(StudentRegisterDTO studentRegisterDTO, int id)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                #region validation for student registration
                // check if name is not empty
                if (string.IsNullOrEmpty(studentRegisterDTO.Name.Trim()))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "name is required"
                    };
                }
                   

                // Check if the email already exists
                if (await _userRepository.IsEmailExist(studentRegisterDTO.Email))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Email already registered."
                    };
                }

                // Validate the email format
                if(string.IsNullOrEmpty(studentRegisterDTO.Email?.Trim()) || !Regex.IsMatch(studentRegisterDTO.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Invalid email format."
                    };
                }


                // validate date of birth 
                if (studentRegisterDTO.DateOfBirth >= DateTime.Now.Date)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Date of birth must be before the current date."
                    };
                }

                // Validate the date of enrollment
                if (studentRegisterDTO.DateOfEnrollment <= studentRegisterDTO.DateOfBirth)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Date of enrollment must be after the date of birth."
                    };
                }

                // class validation
                if((int)studentRegisterDTO.Class > 12 && (int)studentRegisterDTO.Class < 1)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Only 1 to 12 class is availabel"
                    };
                }

                // Role number validation
                if(await _studentRepository.IsRollNumberIsExist(studentRegisterDTO.RollNumber))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "RollNumber is already register with student"
                    };
                }
                #endregion

                var password = $"{studentRegisterDTO.Name}@{studentRegisterDTO.DateOfBirth.Year}";
                var hashedPassword = _passwordEncryption.HashPassword(password);

                var user = new Users
                {
                    Name = studentRegisterDTO.Name,
                    Email = studentRegisterDTO.Email,
                    Role = Roles.Student,
                    Password = hashedPassword,
                    DateOfBirth = studentRegisterDTO.DateOfBirth,
                    DateOfEnrollment = studentRegisterDTO.DateOfEnrollment,
                    IsActive = true,
                    IsPasswordReset = false,
                };

                await _userRepository.AddUserAsync(user);

                var userDetail = await _userRepository.GetUsersAsync(studentRegisterDTO.Email);

                var student = new Students
                {
                    UserId = userDetail.UserId,
                    ClassId = studentRegisterDTO.Class,
                    RollNumber = studentRegisterDTO.RollNumber,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    CreatedBy = id, // here admin or teacher id who is created (get with token)
                    ModifiedBy = id // here admin or teacher id who is modified (get with token)
                };

                await _studentRepository.AddStudentAsync(student);

                await transaction.CommitAsync();

                return new ResponseDTO
                {
                    Status = 201,
                    Data = userDetail.UserId,
                    Message = "Student is register"
                };

            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();

                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion

    }
}
