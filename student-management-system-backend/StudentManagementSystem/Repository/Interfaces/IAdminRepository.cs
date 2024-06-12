using Repository.Enums;
using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task<bool> IsEmailExist(string email);
        Task AddUserAsync(Users user);
        Task AddTeacherAsync(Teachers teachers);

        Task AddStudentAsync(Students student);
        Task<bool> IsTeacherExistInClass(Classes classId, int teacherId);
        Task<Users> GetUsersAsync(string email);

        Task<bool> IsRollNumberIsExist(int number);

    }
}
