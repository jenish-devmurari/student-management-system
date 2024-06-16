using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Interfaces;

namespace StudentManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        #region DI
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        #endregion


        #region Register Teacher
        [HttpPost("RegisterTeacher")]
        public async Task<IActionResult> teacherRegister([FromBody] TeacherRegisterDTO teacherRegisterDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var response = await _adminService.teacherRegister(teacherRegisterDTO, userId);

            if (response.Status == 200)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        #endregion

        #region Get All Teacher Details
        [HttpGet("GetAllTeacher")]
        public async Task<IActionResult> GetAllTeacherDetails()
        {
            return Ok(await _adminService.GetAllTecherDetailsAsync());
        }
        #endregion

        #region Get Teacher Details by id
        [HttpGet("GetTeacherById/{id}")]
        public async Task<IActionResult> GetTeacherDetailsById(int id)
        {

            return Ok(await _adminService.GetTecherDetailsByIdAsync(id));
        }
        #endregion

        #region Update Teacher Details by id
        [HttpPut("UpdateTeacher/{id}")]
        public async Task<IActionResult> UpdateTeacher([FromBody] TeacherUpdateDTO teacherUpdate, int id)
        {
            return Ok(await _adminService.UpdateTeacher(teacherUpdate, id));
        }
        #endregion


        #region delete teacher 
        [HttpDelete("DeleteTeacher/{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            return Ok(await _adminService.DeleteTeacher(id));
        }
        #endregion


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

        #region Get All Students Details
        [HttpGet("GetAllStudent")]
        public async Task<IActionResult> GetAllStudentDetails()
        {
            return Ok(await _adminService.GetAllStudentDetailsAsync());
        }
        #endregion

        #region Get Students Details by id
        [HttpGet("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentDetailsById(int id)
        {
            return Ok(await _adminService.GetStudentDetailsByIdAsync(id));
        }
        #endregion

        #region Update Student Details by id
        [HttpPut("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentUpdateDTO studentUpdate, int id)
        {
            int Id = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _adminService.UpdateStudent(studentUpdate, id, Id));
        }
        #endregion


        #region delete student 
        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            return Ok(await _adminService.DeleteStudent(id));
        }
        #endregion

        #region get attendance details of student using 
        [HttpGet("GetStudentAttendanceDetails/{id}")]
        public async Task<IActionResult> GetStudentAttendanceDetails(int studentId)
        {
            return Ok(await _adminService.GetStudentAttendanceDetailsById(studentId));
        }
        #endregion


        #region get grades details of student using 
        [HttpGet("GetStudentGradesDetails/{id}")]
        public async Task<IActionResult> GetStudentGradesDetails(int studentId)
        {
            return Ok(await _adminService.GetStudentGradesDetailsById(studentId));
        }
        #endregion


        #region update grades details of student using admin
        [HttpPut("UpdateStudentGradesDetails")]
        public async Task<IActionResult> UpdateStudentGradesDetails(StudentGradesDetailsDTO updateDetails)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _adminService.UpdateStudentDetailByGradeId(updateDetails, userId));
        }
        #endregion

        #region update attendance details of student using admin
        [HttpPut("UpdateStudentAttendanceDetails")]
        public async Task<IActionResult> UpdateStudentAttendanceDetails(StudentAttendanceDetailsDTO updateDetails)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _adminService.UpdateStudentDetailByAtteandanceId(updateDetails, userId));
        }
        #endregion
    }
}
