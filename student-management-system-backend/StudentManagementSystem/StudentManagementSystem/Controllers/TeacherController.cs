using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Interfaces;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = ("Teacher"))]
    public class TeacherController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly ITeacherService _teacherService;
        public TeacherController(IAdminService adminService, ITeacherService teacherService)
        {
            _adminService = adminService;
            _teacherService = teacherService;
        }

        #region Register Student
        [HttpPost("RegisterStudent")]
        public async Task<IActionResult> studentRegister([FromBody] StudentRegisterDTO studentRegisterDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            var response = await _adminService.studentRegister(studentRegisterDTO, userId);

            if (response.Status == 201)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        #endregion


        #region get all student which is teacher class
        [HttpGet("GetAllStudentOfTeacherClass")]
        public async Task<IActionResult> GetAllStudentOfTeacherClass()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.GetAllStudentOfTeacherClass(userId));
        }

        #endregion

        #region Mark Attendance of student
        [HttpPost("MarkAttendance")]
        public async Task<IActionResult> MarkAttendance(StudentListAttendanceDTO attendanceDTO)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.MarkAttendance(attendanceDTO, userId));
        }
        #endregion

        #region Attendance History of student
        [HttpGet("AttendanceHistory")]
        public async Task<IActionResult> AttendanceHistory()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.AttendancHistory(userId));
        }
        #endregion

        #region edit Attendance History of student
        [HttpPut("EditAttendanceHistory/{id}")]
        public async Task<IActionResult> EditAttendanceHistory(int id, StudentAttendanceHistoryDTO attendance)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.editAttendancHistory(id, userId, attendance));
        }
        #endregion


        #region add marks of student by teacher
        [HttpPost("AddMarks")]
        public async Task<IActionResult> AddMarks(StudentMarksDTO marksDetails)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.AddMarks(marksDetails, userId));
        }
        #endregion

        #region Get AllStudents Grades of teacher
        [HttpGet("GetAllStudentGrades")]
        public async Task<IActionResult> GetAllStudentGrades()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.GetAllStudentGrades(userId));
        }
        #endregion

        #region update marks of student
        [HttpPut("UpdateMarksOfStudent")]
        public async Task<IActionResult> UpdateMarksOfStudent([FromBody] StudentMarksDTO updateMarks)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.UpdateStudentGrades(updateMarks, userId));
        }
        #endregion


        #region get student details by teacher with user id of student

        [HttpGet("GetStudentDetailById/{id}")]
        public async Task<IActionResult> GetStudentDetailsWithGradeByTeacher(int id)
        {
            return Ok(await _adminService.GetStudentDetailsByIdAsync(id));
        }

        #endregion

        #region get student grade details of teacher subject by user id
        [HttpGet("GetStudentGradesDetailById/{id}")]
        public async Task<IActionResult> GetStudentGradeDetailsByTeacher(int studentUserId)
        {
            var teacherUserID = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.GetStudentGradeDetailsByStudentID(studentUserId, teacherUserID));
        }
        #endregion

        #region get Attendance data of monts's date 
        [HttpGet("GetStudentAttendanceDetailByDate/{date}")]
        public async Task<IActionResult> GetStudentGradeDetailsByTeacher(DateTime date)
        {
            var teacherUserID = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.GetAttendanceDetailsByDate(date, teacherUserID));
        }

        #endregion

    }
}
