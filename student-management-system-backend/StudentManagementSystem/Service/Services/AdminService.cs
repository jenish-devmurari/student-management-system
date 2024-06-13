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
using System.Xml.Linq;

namespace Service.Services
{
    public class AdminService : IAdminService
    {

        #region DI
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPasswordEncryption _passwordEncryption;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IValidationService _validationService;
        private readonly AppDbContext _context;

        public AdminService(IPasswordEncryption passwordEncryption, AppDbContext context, IStudentRepository studentRepository, ITeacherRepository teacherRepository, IUserRepository userRepository, IEmailService emailService, IValidationService validationService)
        {
            _studentRepository = studentRepository;
            _passwordEncryption = passwordEncryption;
            _context = context;
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _validationService = validationService;
        }
        #endregion

        #region teacher register
        public async Task<ResponseDTO> teacherRegister(TeacherRegisterDTO teacherRegisterDTO, int id)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                // Validate the teacher registration data
                ResponseDTO validationResponse = await _validationService.ValidateTeacherRegistrationAsync(teacherRegisterDTO);
                if (validationResponse.Status != 200)
                {
                    return validationResponse;
                }

                string password = $"{teacherRegisterDTO.Name}@{teacherRegisterDTO.DateOfBirth.Year}";
                string hashedPassword = _passwordEncryption.HashPassword(password);

                Users user = new Users
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

                Users userDetail = await _userRepository.GetUsersAsync(teacherRegisterDTO.Email);

                Teachers teacher = new Teachers
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

                await _emailService.SendEmailAsync(teacherRegisterDTO.Email, "Thank You For Registration", teacherRegisterDTO.Email, password);

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

                ResponseDTO validationResponse = await _validationService.ValidateStudentRegistrationAsync(studentRegisterDTO);
                if (validationResponse.Status != 200)
                {
                    return validationResponse;
                }


                string password = $"{studentRegisterDTO.Name}@{studentRegisterDTO.DateOfBirth.Year}";
                string hashedPassword = _passwordEncryption.HashPassword(password);

                Users user = new Users
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

                Users userDetail = await _userRepository.GetUsersAsync(studentRegisterDTO.Email);

                Students student = new Students
                {
                    UserId = userDetail.UserId,
                    ClassId = studentRegisterDTO.Class,
                    RollNumber = studentRegisterDTO.RollNumber,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    CreatedBy = id,
                    ModifiedBy = id
                };

                await _studentRepository.AddStudentAsync(student);

                await transaction.CommitAsync();

                List<string> teacherList = await _teacherRepository.GetTeacherEmailsByClassAsync((int)studentRegisterDTO.Class);
                await _emailService.SendEmailAsync(studentRegisterDTO.Email, "Thank You For Registration", studentRegisterDTO.Email, password, teacherList);

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


        #region get all student data
        public async Task<ResponseDTO> GetAllStudentDetailsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            var studentDetailDTOs = students.Select(student => new StudentDetailDTO
            {
                Id = student.Users.UserId,
                StudentId = student.StudentId,
                Name = student.Users.Name,
                Email = student.Users.Email,
                ClassId = student.ClassId,
                RollNumber = student.RollNumber,
                DateOfBirth = student.Users.DateOfBirth,
                DateOfEnrollment = student.Users.DateOfEnrollment
            }).ToList();

            return new ResponseDTO
            {
                Status = 200,
                Message = "all Students data retrieved successfully",
                Data = studentDetailDTOs
            };
        }
        #endregion


        #region get student data by id
        public async Task<ResponseDTO> GetStudentDetailsByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentDetailsByIdAsync(id);
            if (student == null)
            {
                return new ResponseDTO
                {
                    Status = 404,
                    Message = "Student not found"
                };
            }

            StudentDetailDTO studentDetailDTO = new StudentDetailDTO
            {
                Id = student.Users.UserId,
                StudentId = student.StudentId,
                Name = student.Users.Name,
                Email = student.Users.Email,
                ClassId = student.ClassId,
                RollNumber = student.RollNumber,
                DateOfBirth = student.Users.DateOfBirth,
                DateOfEnrollment = student.Users.DateOfEnrollment
            };

            return new ResponseDTO
            {
                Status = 200,
                Message = "Student retrieved successfully",
                Data = studentDetailDTO
            };
        }
        #endregion


        #region get all teacher data
        public async Task<ResponseDTO> GetAllTecherDetailsAsync()
        {
            var teachers = await _teacherRepository.GetAllTeacherAsync();
            var teacherDetailDTOs = teachers.Select(teacher => new TeacherDetailDTO
            {
                id = teacher.Users.UserId,
                TeacherId = teacher.TeacherId,
                SubjectId = teacher.SubjectId,
                Name = teacher.Users.Name,
                Email = teacher.Users.Email,
                ClassId = teacher.ClassId,
                DateOfBirth = teacher.Users.DateOfBirth,
                DateOfEnrollment = teacher.Users.DateOfEnrollment,
                Qualification = teacher.Qualification,
                Salary = teacher.Salary

            }).ToList();

            return new ResponseDTO
            {
                Status = 200,
                Message = "all Teachers data retrieved successfully",
                Data = teacherDetailDTOs
            };
        }
        #endregion


        #region get teacher data by id
        public async Task<ResponseDTO> GetTecherDetailsByIdAsync(int id)
        {
            var teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(id);
            TeacherDetailDTO teacherDetailDTOs = new TeacherDetailDTO
            {
                id = teacher.Users.UserId,
                TeacherId = teacher.TeacherId,
                SubjectId = teacher.SubjectId,
                Name = teacher.Users.Name,
                Email = teacher.Users.Email,
                ClassId = teacher.ClassId,
                DateOfBirth = teacher.Users.DateOfBirth,
                DateOfEnrollment = teacher.Users.DateOfEnrollment,
                Qualification = teacher.Qualification,
                Salary = teacher.Salary

            };

            return new ResponseDTO
            {
                Status = 200,
                Message = "Teacher retrieved successfully",
                Data = teacherDetailDTOs
            };
        }
        #endregion
    }
}
