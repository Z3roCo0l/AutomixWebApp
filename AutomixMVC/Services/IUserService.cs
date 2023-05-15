using AutomixMVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AutomixMVC.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> CreateUser(string username, string password, string role);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(int id);
    }
}
