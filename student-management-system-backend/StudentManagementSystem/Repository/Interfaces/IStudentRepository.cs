using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IStudentRepository
    {
        Task<bool> IsRollNumberIsExist(int number);

        Task AddStudentAsync(Students student);
    }
}
