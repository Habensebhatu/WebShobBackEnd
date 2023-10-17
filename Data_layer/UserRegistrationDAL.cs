using System;
using Data_layer.Data;
using Data_layer.Context;
using Data_layer.Context.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class UserRegistrationDAL
    {
        private readonly MyDbContext _context;
        public UserRegistrationDAL()
        {
            _context = new MyDbContext();
        }

        public async Task<UserRegistrationEntityModel> AddUser(UserRegistrationEntityModel user)
        {
            _context.UserRegistration.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<UserRegistrationEntityModel>> GetAllUsers()
        {
            return await _context.UserRegistration.ToListAsync();
        }


        public async Task<UserRegistrationEntityModel> GetUserByEmail(string email)
        {
            return await _context.UserRegistration.FirstOrDefaultAsync(u => u.Email == email);
        }




    }
}

