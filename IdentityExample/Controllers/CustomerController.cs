using IdentityExample.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityExample.Controllers
{
    public class CustomerController : BaseController
    {
        [Authorize(Policy = "Permissions.Customer.View")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Permissions.Customer.Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "Permissions.Customer.Update")]
        public IActionResult Update()
        {
            return View();
        }

        [Authorize(Policy = "Permissions.Customer.Delete")]
        public IActionResult Delete()
        {
            return View();
        }
    }
}
