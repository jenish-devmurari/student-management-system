using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ISubjectRepository 
    {
        Task<string> GetSubjectAsync(int subjectId);
    }
}
