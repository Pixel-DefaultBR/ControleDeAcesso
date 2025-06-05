using ControleDeAcesso.Model;
using Microsoft.EntityFrameworkCore;

namespace ControleDeAcesso.Data.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<AuthModel> CreateUserAsync(AuthModel user)
        {
            await _context.AuthModels.AddAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            _context.AuthModels.Remove(user);
        }

        public async Task<AuthModel> GetUserByEmailAsync(string email)
        {
            var user = await _context.AuthModels.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<AuthModel> GetUserByIdAsync(int id)
        {
            var user = await _context.AuthModels.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<AuthModel> UpdateUserAsync(int id, AuthModel user)
        {
            var existingUser = await GetUserByIdAsync(id);
            if (existingUser == null) throw new KeyNotFoundException();

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;

            _context.AuthModels.Update(existingUser);
            await _context.SaveChangesAsync();

            return existingUser;
        }
    }
}
