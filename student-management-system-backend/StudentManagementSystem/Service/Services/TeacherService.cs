using Repository.Interfaces;
using Repository.Modals;
using Repository.Repository;
using Service.DTOs;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        public TeacherService(ITeacherRepository teacherRepository, IStudentRepository studentRepository,IAttendanceRepository attendanceRepository)
        {
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _attendanceRepository = attendanceRepository;
        }

        #region Get All Students list of teacher
        public async Task<ResponseDTO> GetAllStudentOfTeacherClass(int userId)
        {
            try
            {
                Teachers teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher Not Found"
                    };
                }

                List<Students> students = await _studentRepository.GetAllStudentByTeacherClass((int)teacher.ClassId);

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
                    Data = studentDetailDTOs,
                    Message = "Get all student data of teacher class."
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

        public async Task<ResponseDTO> MarkAttendance(List<StudentAttendanceDTO> attendanceDTOs, int userId)
        {
            try
            {
                Teachers teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher Not Found"
                    };
                }

                var attendances = attendanceDTOs.Select(a => new Attendance
                {
                    StudentId = a.StudentId,
                    TeacherId = teacher.TeacherId,
                    SubjectId = teacher.SubjectId,
                    ClassId = teacher.ClassId,
                    Date = DateTime.Now,
                    IsPresent = a.IsPresent,
                    CreatedBy = teacher.Users.UserId,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = teacher.Users.UserId,
                    ModifiedOn = DateTime.Now,
                }).ToList();

                await _attendanceRepository.AddAttendancesAsync(attendances);

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Attendance marked successfully."
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



    }
}
