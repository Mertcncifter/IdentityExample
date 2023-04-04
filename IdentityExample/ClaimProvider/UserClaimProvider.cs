using IdentityExample.Enums;
using IdentityExample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityExample.ClaimProvider
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public UserClaimProvider(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            var identityUser = principal.Identity as ClaimsIdentity;

            var currentUser = await _userManager.FindByNameAsync(identityUser!.Name!);

            var userPermissionClaims = _appDbContext.UserPermissions.Where(x => x.AppUserId == currentUser.Id && x.State).ToList();

            foreach (var item in userPermissionClaims)
            {
                Claim permission = new Claim("permission", item.Permission.GetClaimValue());
                identityUser.AddClaim(permission);
            }

            if (String.IsNullOrEmpty(currentUser!.City))
            {
                return principal;
            }

            if (principal.HasClaim(x => x.Type != "city"))
            {
                Claim cityClaim = new Claim("city", currentUser.City);
                identityUser.AddClaim(cityClaim);
            }

            return principal;
        }
    }
}
