using IdentityExample.Enums;

namespace IdentityExample.Models
{
    public class UserPermission
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public Permission Permission { get; set; }
        public bool State { get; set; }
        public AppUser AppUser { get; set; }
    }
}
