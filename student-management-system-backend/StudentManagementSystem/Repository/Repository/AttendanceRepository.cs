using Repository.Interfaces;
using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAttendancesAsync(List<Attendance> attendances)
        {
            _context.Attendance.AddRange(attendances);
            await _context.SaveChangesAsync();
        }
    }
}
