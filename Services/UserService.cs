using System;
using App.Interfaces;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;

namespace App.Services
{
    public class UserService : IUserService
    {
        private readonly OttoDbContext _context;

        public UserService(OttoDbContext context)
        {
            _context = context;
        }

        public async Task SaveUserAsync(string email, string role)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser == null)
            {
                var user = new User
                {
                    Email = email,
                    Role = role
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"User with email {email} already exists.");
            }
        }
    }
}