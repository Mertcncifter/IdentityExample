using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityExample.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        
    }
}
