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
          _teacherService = _teacherService;
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
            //var userId = 1;

            var response = await _adminService.studentRegister(studentRegisterDTO,userId);

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
    }
}
