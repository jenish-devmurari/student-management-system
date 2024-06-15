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
    public  class StudentService :IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        public StudentService(IStudentRepository studentRepository,IAttendanceRepository attendanceRepository)
        {
            _studentRepository = studentRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<ResponseDTO> GetAllAttendance(int userId)
        {
            try
            {
                Students students = await _studentRepository.GetStudentDetailsByIdAsync(userId);

                if (students == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Student Not Found"
                    };
                }

                List<Attendance> attendances = await _attendanceRepository.GetStudentAllAttendance(students.StudentId);

                var attendanceDetails = attendances.Select(attendance => new StudentAttendanceDetails
                {
                    Date = attendance.Date, 
                    SubjectName = attendance.Subjects.SubjectName, 
                    IsPresent = attendance.IsPresent, 
                }).ToList();


                return new ResponseDTO
                {
                    Status = 200,
                    Data = attendanceDetails,
                    Message = "Fetched all attendance records successfully."
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
    }
}
