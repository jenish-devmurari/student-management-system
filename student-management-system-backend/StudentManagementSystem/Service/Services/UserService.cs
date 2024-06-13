using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using Repository.Modals;
using Service.DTOs;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordEncryption _passwordEncryption;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository,IPasswordEncryption passwordEncryption,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordEncryption = passwordEncryption;
            _configuration = configuration;
        }

        public async Task<ResponseDTO> Login(LoginDTO login)
        {
            try
            {
                Users user = await _userRepository.GetUsersAsync(login.Email);

                if (user != null && _passwordEncryption.VerifyPassword(login.Password, user.Password))
                {
                    string token = GenerateToken(user);
                    return new ResponseDTO
                    {
                        Status = 200,
                        Data = new
                        {
                            Token = token,
                            Expiration = DateTime.UtcNow.AddHours(1),
                            IsPasswordReset = user.IsPasswordReset
                        },
                        Message = "Login successful"
                    };
                }
                return new ResponseDTO { Status = 401, Message = "Invalid credentials" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Message = "An error occurred while processing your request" };
            }
        }

        public string GenerateToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ResponseDTO> ChangePassword(string newPassword, string email)
        {

            Users user = await _userRepository.GetUsersAsync(email);

            if (user == null)
            {
                return new ResponseDTO
                {
                    Status = 404,
                    Message = "User not found."
                };
            }

            string hashedPassword = _passwordEncryption.HashPassword(newPassword);

            user.Password = hashedPassword;
            user.IsPasswordReset = true;
            await _userRepository.UpdateUserAsync(user);

            return new ResponseDTO
            {
                Status = 200,
                Message = "Password has been reset successfully."
            };
        }
    }
}
