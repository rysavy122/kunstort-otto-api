using System;
using App.Interfaces;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using System.Net.Http.Headers;

namespace App.Services
{
    public class UserService : IUserService
    {
        private readonly OttoDbContext _context;
        private readonly IAuth0Service _auth0Service;


        public UserService(OttoDbContext context, IAuth0Service auth0Service)
        {
            _context = context;
            _auth0Service = auth0Service;
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

        // TODO DELETE USER CHECK AUTH0 MANAGEMENT API
       public async Task DeleteUserAsync(string email, string role)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
        {
            // Delete from Auth0
            var token = await _auth0Service.GetManagementApiTokenAsync();
            var domain = "dev-z3z23qam2lr3gsku.us.auth0.com"; // without https://

            var auth0UserId = $"auth0|{email}"; // or better: store the user_id in your DB

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var deleteResponse = await client.DeleteAsync($"https://{domain}/api/v2/users/{Uri.EscapeDataString(auth0UserId)}");

                if (!deleteResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to delete Auth0 user: {deleteResponse.StatusCode}");
                }
            }

            // Delete from your DB
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine($"User with email {email} not found.");
        }
    }
    }
}
