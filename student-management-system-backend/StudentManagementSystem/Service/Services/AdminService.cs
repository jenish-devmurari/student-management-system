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
using System.Threading.Tasks;

namespace Service.Services
{
    public class AdminService: IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPasswordEncryption _passwordEncryption;
        private readonly AppDbContext _context;

        public AdminService(IAdminRepository adminRepository, IPasswordEncryption passwordEncryption, AppDbContext context)
        {
            _adminRepository = adminRepository;
            _passwordEncryption = passwordEncryption;
            _context = context;
        }

        public async Task<ResponseDTO> teacherRegister(TeacherRegisterDTO teacherRegisterDTO)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
              
                // Check if the email already exists
                if (await _adminRepository.IsEmailExist(teacherRegisterDTO.Email))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Email already registered."
                    };
                }

                // Validate the email format
                if (!IsValidEmail(teacherRegisterDTO.Email))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Invalid email format."
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

                // Check if a teacher already exists in the class for the given subject
                if (await _adminRepository.IsTeacherExistInClass(teacherRegisterDTO.Class, teacherRegisterDTO.Subject))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "A teacher already exists for this class and subject."
                    };
                }

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

                await _adminRepository.AddUserAsync(user);

                var userDetail = await _adminRepository.GetUsersAsync(teacherRegisterDTO.Email);

                var teacher = new Teachers
                {
                    UserId = userDetail.UserId,
                    Qualification = teacherRegisterDTO.Qualification,
                    ClassId = (Classes)teacherRegisterDTO.Class,
                    SubjectId = teacherRegisterDTO.Subject,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Salary = teacherRegisterDTO.Salary
                };

                await _adminRepository.AddTeacherAsync(teacher);

                await transaction.CommitAsync();

                return new ResponseDTO
                {
                    Status = 200,
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

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
