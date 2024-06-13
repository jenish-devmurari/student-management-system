using Repository.Enums;
using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITeacherRepository
    {
        Task AddTeacherAsync(Teachers teachers);
        Task<bool> IsTeacherExistInClass(Classes classId, int teacherId);

        Task<List<string>> GetTeacherEmailsByClassAsync(int classId);

    }
}
