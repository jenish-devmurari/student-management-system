using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Interfaces;

namespace StudentManagementSystem.Controllers
{
   
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

            var response = await _adminService.teacherRegister(teacherRegisterDTO);

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
    }
}
