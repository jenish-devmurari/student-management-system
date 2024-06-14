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
        public async Task<IActionResult> MarkAttendance(List<StudentAttendanceDTO> attendanceDTO)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _teacherService.MarkAttendance(attendanceDTO, userId));
        }
        #endregion

    }
}
