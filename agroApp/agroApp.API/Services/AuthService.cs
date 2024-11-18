using agroApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agroApp.Domain.Entities;
using agroApp.API.DTOs;
using Microsoft.EntityFrameworkCore;
using agroApp.Infra.Data.Repositories;
using System.Text.RegularExpressions;
using agroApp.Infra.Data.Context;

namespace agroApp.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRoleRepository _roleRepository; 
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context; 

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IRoleRepository roleRepository, IUserRepository userRepository, AppDbContext context)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IdentityResult> RegisterAsync(string username, string email, string password)
        {
            var user = new User { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password); //Use UserManager for password hashing
            
            if (result.Succeeded)
            {
                var defaultRole = await _roleRepository.GetByNameAsync("User");
                if (defaultRole == null)
                {
                    //Use Guid.NewGuid() directly, no need to convert to string here.
                   defaultRole = await _roleRepository.AddAsync(new Role { Name = "User" });
                }

                await _context.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = defaultRole.Id });
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            else
            {
                return result;
            }
        }

        public async Task<SignInResult> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return SignInResult.Failed;
            }

           return SignInResult.Success;
        }

        // Método para gerar um token JWT
        // Aplication.Services/AuthService.cs
        public async Task<string> GenerateTokenAsync(User user)
        {
            var roles = (await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Include(ur => ur.Role) //Include is now correctly used
                .Select(ur => ur.Role.Name)
                .ToListAsync());
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new Exception("Usuário não encontrado.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // TODO: Implementar envio de email com o token aqui.
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }
    }
}
