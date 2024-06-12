﻿using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAdminService
    {
        public  Task<ResponseDTO> teacherRegister(TeacherRegisterDTO teacherRegisterDTO);
        public Task<ResponseDTO> studentRegister(StudentRegisterDTO studentRegisterDTO);
    }
}
