using IdentityExample.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace IdentityExample.Models
{
    public class AppUser: IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }

        public List<UserPermission> UserPermissions { get; set; }
    }
}
