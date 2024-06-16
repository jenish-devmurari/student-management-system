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
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IPasswordEncryption _passwordEncryption;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IValidationService _validationService;
        private readonly AppDbContext _context;

        public AdminService(IPasswordEncryption passwordEncryption, AppDbContext context, IStudentRepository studentRepository, ITeacherRepository teacherRepository, IUserRepository userRepository, IEmailService emailService, IValidationService validationService, IAttendanceRepository attendanceRepository, IGradeRepository gradeRepository)
        {
            _studentRepository = studentRepository;
            _passwordEncryption = passwordEncryption;
            _context = context;
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _validationService = validationService;
            _attendanceRepository = attendanceRepository;
            _gradeRepository = gradeRepository;
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
                    IsPasswordReset = false,
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

                await _emailService.SendEmailTeacherAsync(teacherRegisterDTO.Email);

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
                await _emailService.SendEmailToStudentAsync(studentRegisterDTO, teacherList);

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
            try
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
                    DateOfEnrollment = student.Users.DateOfEnrollment,
                    IsActive = student.Users.IsActive,
                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "all Students data retrieved successfully",
                    Data = studentDetailDTOs
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion


        #region get student data by id
        public async Task<ResponseDTO> GetStudentDetailsByIdAsync(int id)
        {
            try
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
                    DateOfEnrollment = student.Users.DateOfEnrollment,
                    IsActive = student.Users.IsActive,
                };

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Student retrieved successfully",
                    Data = studentDetailDTO
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion

        #region delete student by id 
        public async Task<ResponseDTO> DeleteStudent(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "User not found."
                    };
                }

                if (user.Role != Roles.Student)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "This User is not a student"
                    };
                }

                user.IsActive = false;
                await _userRepository.UpdateUserAsync(user);
                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Student Deleted successfully",
                    Data = user.UserId
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }

        }
        #endregion

        #region update student by id 
        public async Task<ResponseDTO> UpdateStudent(StudentUpdateDTO studentUpdate, int id, int Id)
        {
            try
            {
                ResponseDTO validationResponse = await _validationService.ValidateStudentUpdateAsync(studentUpdate);
                if (validationResponse.Status != 200)
                {
                    return validationResponse;
                }

                var student = await _studentRepository.GetStudentDetailsByIdAsync(id);

                if (student == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "User not found."
                    };
                }

                if (student.RollNumber != studentUpdate.RollNumber)
                {
                    // Roll number validation
                    if (await _studentRepository.IsRollNumberIsExist(studentUpdate.RollNumber))
                    {
                        return new ResponseDTO
                        {
                            Status = 400,
                            Message = "Roll number is already registered with a student."
                        };
                    }
                }

                // Update the user-related details
                student.Users.Name = studentUpdate.Name;
                student.Users.DateOfBirth = studentUpdate.DateOfBirth;
                student.Users.DateOfEnrollment = studentUpdate.DateOfEnrollment;

                // Update the student-specific details
                student.ClassId = studentUpdate.ClassId;
                student.RollNumber = studentUpdate.RollNumber;
                student.ModifiedOn = DateTime.Now;
                student.ModifiedBy = Id;

                await _studentRepository.UpdateStudentAsync(student);
                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Student Updated successfully",
                    Data = student.UserId
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }

        }
        #endregion

        #region get all teacher data
        public async Task<ResponseDTO> GetAllTecherDetailsAsync()
        {
            try
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
                    Salary = teacher.Salary,
                    IsActive = teacher.Users.IsActive

                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "all Teachers data retrieved successfully",
                    Data = teacherDetailDTOs
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion


        #region get teacher data by id
        public async Task<ResponseDTO> GetTecherDetailsByIdAsync(int id)
        {
            try
            {
                var teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(id);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher not found"
                    };
                }

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
                    Salary = teacher.Salary,
                    IsActive = teacher.Users.IsActive
                };

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Teacher retrieved successfully",
                    Data = teacherDetailDTOs
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion


        #region delete teacher by id 
        public async Task<ResponseDTO> DeleteTeacher(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "User not found."
                    };
                }

                if (user.Role != Roles.Teacher)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "This User is not a Teacher"
                    };
                }

                user.IsActive = false;
                await _userRepository.UpdateUserAsync(user);
                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Teacher Deleted successfully",
                    Data = user.UserId
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }

        }
        #endregion

        #region update teacher by id 
        public async Task<ResponseDTO> UpdateTeacher(TeacherUpdateDTO teacherUpdate, int id)
        {
            try
            {
                ResponseDTO validationResponse = await _validationService.ValidateTeacherUpdateAsync(teacherUpdate);
                if (validationResponse.Status != 200)
                {
                    return validationResponse;
                }

                var teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(id);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "User not found."
                    };
                }

                // Update the user-related details
                teacher.Users.Name = teacherUpdate.Name;
                teacher.Users.DateOfBirth = teacherUpdate.DateOfBirth;
                teacher.Users.DateOfEnrollment = teacherUpdate.DateOfEnrollment;

                // Update the teacher-specific details
                teacher.Salary = teacherUpdate.Salary;
                teacher.Qualification = teacherUpdate.Qualification;
                teacher.ModifiedOn = DateTime.Now;


                await _teacherRepository.UpdateTeacherAsync(teacher);
                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Teacher Updated successfully",
                    Data = teacher.UserId
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }

        }
        #endregion

        #region  Get Student Attendance Details
        public async Task<ResponseDTO> GetStudentAttendanceDetailsById(int studentId)
        {
            try
            {
                List<Attendance> attendances = await _attendanceRepository.GetAttedanceOfStudent(studentId);

                if (attendances == null || !attendances.Any())
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "No attendance records found for the student.",
                        Data = null
                    };
                }

                List<StudentAttendanceDetailsDTO> attendanceDetails = attendances.Select(a => new StudentAttendanceDetailsDTO
                {
                    Id = a.id,
                    Name = a.Students.Users.Name,
                    SubjectName = a.Subjects.SubjectName,
                    classId = a.ClassId,
                    IsPresent = a.IsPresent,
                    Date = a.Date,
                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Student attendance retrieved successfully",
                    Data = attendanceDetails
                };
            }
            catch (Exception ex)
            {

                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        #endregion


        #region  Get Student Grades Details
        public async Task<ResponseDTO> GetStudentGradesDetailsById(int studentId)
        {
            try
            {
                List<Grades> grades = await _gradeRepository.GetGradesOfStudent(studentId);

                if (grades == null || !grades.Any())
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "No Grades records found for the student.",
                        Data = null
                    };
                }

                List<StudentGradesDetailsDTO> gradesDetails = grades.Select(a => new StudentGradesDetailsDTO
                {
                    gradeId = a.id,
                    Name = a.Students.Users.Name,
                    SubjectName = a.Teachers.Subject.SubjectName,
                    ClassId = a.Students.ClassId,
                    Marks = a.Marks,
                    TotalMarks = a.TotalMarks,
                    Date = a.Date,
                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Student grades retrieved successfully",
                    Data = gradesDetails
                };
            }
            catch (Exception ex)
            {

                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion


        //#region update student details on attendance page 
        //public async Task<ResponseDTO> UpdateStudentDetailByAtteandanceId(int attendanceId)
        //{
        //    try
        //    {
        //        List<Attendance> attendances = await _attendanceRepository.GetAttedanceOfStudent(studentId);

        //        if (attendances == null || !attendances.Any())
        //        {
        //            return new ResponseDTO
        //            {
        //                Status = 404,
        //                Message = "No attendance records found for the student.",
        //                Data = null
        //            };
        //        }

        //        StudentAttendanceDetailsDTO attendanceDetails = attendances.Select(a => new StudentAttendanceDetailsDTO
        //        {
        //            Id = a.id,
        //            Name = a.Students.Users.Name,
        //            SubjectName = a.Subjects.SubjectName,
        //            classId = a.ClassId,
        //            IsPresent = a.IsPresent,
        //            Date = a.Date,
        //        }).ToList();

        //        return new ResponseDTO
        //        {
        //            Status = 200,
        //            Message = "Student update successfully",
        //            Data = attendanceDetails
        //        };
        //    }
        //    catch (Exception ex)
        //    {

        //        return new ResponseDTO
        //        {
        //            Status = 500,
        //            Message = $"An error occurred: {ex.Message}"
        //        };
        //    }
        //}

        //#endregion

    }
}
