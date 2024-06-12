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
    public class AdminRepository :IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTeacherAsync(Teachers teachers)
        {
            await _context.Teachers.AddAsync(teachers);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddStudentAsync(Students student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task<Users> GetUsersAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsTeacherExistInClass(Classes classId, int subject)
        {
            return await _context.Teachers.AnyAsync(t => t.ClassId == classId && t.SubjectId == subject);
        }

        public async Task<bool> IsRollNumberIsExist(int number)
        {
            return await _context.Students.AnyAsync(s => s.RollNumber == number);
        }


    }
}
