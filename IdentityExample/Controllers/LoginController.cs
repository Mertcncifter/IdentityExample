using IdentityExample.Extension;
using IdentityExample.Models;
using IdentityExample.ViewModel;
using IdentityExample.Web.Services;
using IdentityExample.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityExample.Controllers
{
    public class LoginController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<IActionResult>  SignIn()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            returnUrl ??= Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);


            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 dakika boyunca giriş yapamazsınız." });
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string>() { $"Email veya şifre yanlış", $"Başarısız giriş sayısı = {await _userManager.GetAccessFailedCountAsync(hasUser)}" });
                return View();
            }

            if (hasUser.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, model.RememberMe, new[] { new Claim("birthdate", hasUser.BirthDate.Value.ToString()) });
            }
            return Redirect(returnUrl!);


        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.PasswordConfirm);


            if (!identityResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());

            var user = await _userManager.FindByNameAsync(request.UserName);

            var claimResult = await _userManager.AddClaimAsync(user!, exchangeExpireClaim);

            if (!claimResult.Succeeded)
            {
                ModelState.AddModelErrorList(claimResult.Errors.Select(x => x.Description).ToList());
                return View();
            }


            TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıla gerçekleşmiştir.";

            return RedirectToAction(nameof(LoginController.SignUp));

        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if(hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine sahip kullanıcı bulunmamıstır");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordRestLink = Url.Action("ResetPassword", "Login", new { userId = hasUser.Id, Token = passwordResetToken },HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordRestLink, hasUser.Email);

            TempData["SuccessMessage"] = "Şifre yenileme linki, eposta adresinize gönderilmiştir";
            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult ResetPassword(string userId,string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Kullanıcı bulunamamıştır.");
                return View();
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla yenilenmiştir";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
            }

            return View();

        }
    }
}



