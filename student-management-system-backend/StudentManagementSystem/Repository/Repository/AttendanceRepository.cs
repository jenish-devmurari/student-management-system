using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public async Task<List<Attendance>> GetAttendanceDetailsOfStudent(int teacherId)
        {
            return await _context.Attendance
                .Include(a => a.Students)
                    .ThenInclude(s => s.Users)
                .Include(a => a.Subjects)
                .Where(a => a.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<Attendance> GetAttendanceAsync(int id)
        {
            return await _context.Attendance.FirstOrDefaultAsync(a => a.id == id);
        }

        public async Task<bool> IsAttendenceDone(DateTime date, int teacherId)
        {
            return await _context.Attendance.AnyAsync(a => a.Date == date && a.TeacherId == teacherId);
        }
        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            _context.Attendance.Update(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Attendance>> GetStudentAllAttendance(int studentId)
        {
            return await _context.Attendance
                .Include(a => a.Students)
                .Include(a => a.Subjects)
                .Where(a => a.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Attendance>> GetAttedanceOfStudent(int studentId)
        {
            return await _context.Attendance.Include(s => s.Students).ThenInclude(u => u.Users).Include(s => s.Subjects).Where(s => s.StudentId == studentId).ToListAsync();
        }

        public async Task<List<Attendance>> GetAttendanceDetailsByDate(DateTime date, int teacherId)
        {
            return await _context.Attendance.Include(s => s.Students).ThenInclude(u => u.Users).Include(s => s.Subjects).Where(a => a.Date ==  date && a.TeacherId == teacherId).ToListAsync();
        }
    }
}
