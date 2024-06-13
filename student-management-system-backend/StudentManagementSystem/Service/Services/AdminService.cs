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
                await _emailService.SendEmailAsync(studentRegisterDTO.Email,"Thank You For Registration",studentRegisterDTO.Email,password,teacherList);

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
