using System;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class LoginDAL
    {
        private readonly MyDbContext _context;
        public LoginDAL()
        {
            _context = new MyDbContext();
        }

        public async Task<LoginEnitiyModel> GetUserByEmail(string username)
        {
            return await _context.Login.FirstOrDefaultAsync(u => u.useName == username);
        }
    }
}

