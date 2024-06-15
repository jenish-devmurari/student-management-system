using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _context;

        public SubjectRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> GetSubjectAsync(int subjectId)
        {
            var subject = await _context.Subjects
                                        .Where(s => s.SubjectId == subjectId)
                                        .Select(s => s.SubjectName) 
                                        .FirstOrDefaultAsync();

            return subject;
        }
    }
}
