using Microsoft.AspNetCore.Identity;

namespace UserManagementApp.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime LastLoginTime { get; set; } = DateTime.UtcNow; // Always store in UTC
    }
}
