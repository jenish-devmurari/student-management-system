using Microsoft.EntityFrameworkCore;
using Repository.Enums;
using Repository.Interfaces;
using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _context;

        public TeacherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTeacherAsync(Teachers teachers)
        {
            await _context.Teachers.AddAsync(teachers);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsTeacherExistInClass(Classes classId, int subject)
        {
            return await _context.Teachers.AnyAsync(t => t.ClassId == classId && t.SubjectId == subject);
        }

        public async Task<List<string>> GetTeacherEmailsByClassAsync(int classId)
        {
            return await (from teacher in _context.Teachers
                          join user in _context.Users on teacher.UserId equals user.UserId
                          where (int)teacher.ClassId == classId
                          select user.Email).ToListAsync();
        }

    }
}
