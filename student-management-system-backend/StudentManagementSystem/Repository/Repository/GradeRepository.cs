using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _context;

        public GradeRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddMarks(Grades grades)
        {
            await _context.Grades.AddAsync(grades);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Grades>> GetAllStudentGradesOfTeacherSubject(int teacherId)
        {
            return await _context.Grades.Include(s => s.Students).ThenInclude(u => u.Users).Include(t => t.Teachers).Where(g => g.TeacherId == teacherId).ToListAsync();
        }

        public async Task<Grades> GetGradeDetailsByID(int id)
        {
            return await _context.Grades
        .Include(g => g.Students).ThenInclude(u => u.Users)
        .Include(g => g.Teachers)
        .SingleOrDefaultAsync(g => g.id == id);
        }

        public async Task UpdateGrades(Grades grades)
        {
            _context.Grades.Update(grades);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Grades>> GetGradeDetailsByUserID(int studentUserId, int teacherUserId)
        {
            return await _context.Grades.Where(g => g.Students.UserId == studentUserId && g.Teachers.UserId == teacherUserId).ToListAsync();

        }

        public async Task<List<Grades>> StudentGradesBySubject(int studentId, int subjectId)
        {
            return await _context.Grades
                .Include(g => g.Students)
                .Include(g => g.Teachers)
                .ThenInclude(t => t.Subject)
                .Where(g => g.StudentId == studentId && g.Teachers.SubjectId == subjectId).ToListAsync();
        }
    }
}











































































