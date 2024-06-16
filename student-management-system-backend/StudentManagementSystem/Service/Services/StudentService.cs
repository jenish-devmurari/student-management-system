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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IGradeRepository _gradeRepository;
        public StudentService(IStudentRepository studentRepository, IAttendanceRepository attendanceRepository, IGradeRepository gradeRepository)
        {
            _studentRepository = studentRepository;
            _attendanceRepository = attendanceRepository;
            _gradeRepository = gradeRepository;
        }

        #region get all attendance of student
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
        #endregion



        #region get student grades by subject
        public async Task<ResponseDTO> GetStudentSubjectGrades(int subjectId,int userId)
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
                };

                // Fetch the grades for the student and subject
                List<Grades> grades = await _gradeRepository.StudentGradesBySubject(students.StudentId, subjectId);

                var gradesList = grades.Select(g => new StudentMarksDTO
                {
                    Date =g.Date,
                    Marks = g.Marks,
                    TotalMarks = g.TotalMarks
                }).ToList();

                // Check if grades are found
                if (grades == null || grades.Count == 0)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "No grades found for the specified subject."
                    };
                }

                // Return the grades in the response
                return new ResponseDTO
                {
                    Status = 200,
                    Data = gradesList,
                    Message = "Fetched grades successfully."
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
