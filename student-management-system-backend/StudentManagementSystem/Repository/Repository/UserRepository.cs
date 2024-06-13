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
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        
    }
}