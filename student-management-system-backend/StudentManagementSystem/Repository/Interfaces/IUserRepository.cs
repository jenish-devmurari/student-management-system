using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailExist(string email);
        Task AddUserAsync(Users user);

        Task<Users> GetUsersAsync(string email);

        Task UpdateUserAsync(Users user);

        Task<Users> GetUserByIdAsync(int id);

        Task<Users> GetUserByEmailAsync(string email);

    }
}
