namespace AutomixMVC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } 
        public string PasswordHash { get; set; } 
        public string? Role { get; set; }

        public User(string username, string passwordHash, string? role)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash)); ;
            Role = role;
        }
    }
}
