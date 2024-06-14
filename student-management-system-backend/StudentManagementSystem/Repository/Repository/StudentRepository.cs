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
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddStudentAsync(Students student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsRollNumberIsExist(int number)
        {
            return await _context.Students.AnyAsync(s => s.RollNumber == number && s.Users.IsActive == true);
        }

        public async Task<List<Students>> GetAllStudentsAsync()
        {
            return await _context.Students.Include(s => s.Users).ToListAsync();
        }

        public async Task<Students> GetStudentDetailsByIdAsync(int id)
        {
            return await _context.Students
                                 .Include(s => s.Users)
                                 .FirstOrDefaultAsync(s => s.UserId == id );
        }

        public async Task UpdateStudentAsync(Students student)
        {
            _context.Students.Update(student);
            _context.Users.Update(student.Users); 
            await _context.SaveChangesAsync();
        }
    }
}
