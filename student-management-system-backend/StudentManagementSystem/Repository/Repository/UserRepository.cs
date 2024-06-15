using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using Repository.Modals;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Users> GetUsersAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsActive == true);
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.IsActive == true);
        }

        public async Task UpdateUserAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return await _context.Users
            .Include(u => u.Student)
            .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}