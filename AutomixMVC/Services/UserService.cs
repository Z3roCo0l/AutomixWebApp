using AutomixMVC.Data;
using AutomixMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AutomixMVC.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly AutomixDbContext _context;

        public UserService(IPasswordHasher passwordHasher, AutomixDbContext context)
        {
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Username and password must be provided");

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
                throw new ArgumentException("Invalid username or password");

            var isPasswordValid = _passwordHasher.VerifyPassword(user.PasswordHash, password);

            if (!isPasswordValid)
                throw new ArgumentException("Invalid username or password");

            return user;
        }

        public async Task<User> CreateUser(string username, string password, string role)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password must be provided");

            if (_context.Users.Any(u => u.Username == username))
                throw new ArgumentException($"Username \"{username}\" is already taken");

            var user = new User { Username = username, Role = role };
            user.PasswordHash = _passwordHasher.HashPassword(password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers() 
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUser(int id) 
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
