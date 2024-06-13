using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<ResponseDTO> Login(LoginDTO login);

        Task<ResponseDTO> ChangePassword(string newPassword, string email);
    }
}
