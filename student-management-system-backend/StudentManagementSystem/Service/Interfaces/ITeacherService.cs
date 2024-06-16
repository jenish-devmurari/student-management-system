using Repository.Modals;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITeacherService
    {
        public Task<ResponseDTO> GetAllStudentOfTeacherClass(int userId);

        public Task<ResponseDTO> MarkAttendance(List<StudentAttendanceDTO> attendanceDTO, int userId);

        public Task<ResponseDTO> AttendancHistory(int userId);

        public Task<ResponseDTO> editAttendancHistory(int id, int userId, StudentAttendanceHistoryDTO attendance);

        public Task<ResponseDTO> AddMarks(StudentMarksDTO marksDetails, int userId);

        public Task<ResponseDTO> GetAllStudentGrades(int userId);

        public Task<ResponseDTO> UpdateStudentGrades(StudentMarksDTO updateMarks, int userId);


        public Task<ResponseDTO> GetStudentGradeDetailsByStudentID(int studentUserID, int userID);
    }
}
