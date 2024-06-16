using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAttendanceRepository
    {
        Task AddAttendancesAsync(List<Attendance> attendances);
        Task<List<Attendance>> GetAttendanceDetailsOfStudent(int classId);

        Task<Attendance> GetAttendanceAsync(int id);

        Task<bool> IsAttendenceDone(DateTime date, int teacherId);

        Task UpdateAttendanceAsync(Attendance attendance);

        Task<List<Attendance>> GetStudentAllAttendance(int studentId);

        Task<List<Attendance>> GetAttedanceOfStudent(int studentId);
    }
}
