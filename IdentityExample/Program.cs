

using IdentityExample.ClaimProvider;
using IdentityExample.Enums;
using IdentityExample.Models;
using IdentityExample.Requirements;
using IdentityExample.Web.Extenisons;
using IdentityExample.Web.OptionsModels;
using IdentityExample.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

builder.Services.AddIdentityWithExt();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();

builder.Services.AddAuthorization(options =>
{
   
    var permissionList = Enum.GetValues(typeof(Permission)).Cast<Permission>().ToList();

    foreach (var item in permissionList)
    {
        var name = item.GetClaimValue();
        options.AddPolicy(name, policy => {
            policy.RequireClaim("permission",name);
        });
    }




    options.AddPolicy("AnkaraPolicy", policy => {
        policy.RequireClaim("city", "istanbul", "ankara");
    });


    options.AddPolicy("ExchangePolicy", policy => {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });

    options.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });
    });

});


builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "IdentityCookie";
    opt.LoginPath = new PathString("/Login/SignIn");
    opt.LogoutPath = new PathString("/Home/logout");
    opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default", 
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
