﻿using Microsoft.EntityFrameworkCore;
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
            return await _context.Teachers.AnyAsync(t => t.ClassId == classId && t.SubjectId == subject && t.Users.IsActive == true);
        }

        public async Task<List<string>> GetTeacherEmailsByClassAsync(int classId)
        {
            return await (from teacher in _context.Teachers
                          join user in _context.Users on teacher.UserId equals user.UserId
                          where (int)teacher.ClassId == classId && user.IsActive == true
                          select user.Email).ToListAsync();
        }

        public async Task<List<Teachers>> GetAllTeacherAsync()
        {
            return await _context.Teachers.Include(s => s.Users).Where(s => s.Users.IsActive).ToListAsync();
        }

        public async Task<Teachers> GetTeacherDetailsByIdAsync(int id)
        {
            return await _context.Teachers
                                 .Include(s => s.Users)
                                 .FirstOrDefaultAsync(s => s.UserId == id);
        }

        public async Task UpdateTeacherAsync(Teachers teacher)
        {
            _context.Teachers.Update(teacher);
            _context.Users.Update(teacher.Users);
            await _context.SaveChangesAsync();
        }

       
    }
}
