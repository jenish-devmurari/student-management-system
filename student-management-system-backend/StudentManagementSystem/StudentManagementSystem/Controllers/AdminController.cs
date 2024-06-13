﻿using Microsoft.AspNetCore.Authorization;
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
    }
}
