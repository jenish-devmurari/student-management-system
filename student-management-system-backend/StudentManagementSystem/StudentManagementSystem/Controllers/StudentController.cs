using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace StudentManagementSystem.Controllers
{

    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        #region Get All attendance of student 
        [HttpGet("GetAllAttendence")]
        public async Task<IActionResult> GetAllAttendance()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _studentService.GetAllAttendance(userId));
        }
        #endregion

        #region Get student grades details particular subject
        [HttpGet("GetStudentGradesDetailsBasedOnSubject/{id}")]
        public async Task<IActionResult> GetStudentGradesBySubject(int subjectId)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _studentService.GetStudentSubjectGrades(subjectId, userId));
        }
        #endregion
    }
}
