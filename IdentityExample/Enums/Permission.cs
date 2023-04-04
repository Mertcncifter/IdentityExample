using System.ComponentModel;

namespace IdentityExample.Enums
{
    public enum Permission : short
    {
        CustomerView = 1,
        CustomerCreate = 2,
        CustomerDelete = 3,
        CustomerUpdate = 4,
    }

    public static class PermissionExtension
    {
        public static string GetClaimValue(this Permission permission)
        {
            switch (permission)
            {
                case Permission.CustomerView:
                    return "Permissions.Customer.View";
                case Permission.CustomerCreate:
                    return "Permissions.Customer.Create";
                case Permission.CustomerUpdate:
                    return "Permissions.Customer.Update";
                case Permission.CustomerDelete:
                    return "Permissions.Customer.Delete";
                default:
                    return "";
            }
        }
    }
}
