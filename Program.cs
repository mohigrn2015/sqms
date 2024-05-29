using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SQMS.BLL;
using SQMS.Controllers;
using SQMS.Helper;
using SQMS.SignalRHub;
using SQMS.Utility;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders();
builder.Services.AddScoped<IUserStore<IdentityUser>, CustomUserStoreV2>();
builder.Services.AddScoped<IRoleStore<IdentityRole>, CustomRoleStore>();
builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, CustomPasswordHasher>();
builder.Services.AddScoped<CustomSignInManager>();
builder.Services.AddScoped<CustomUserManager<IdentityUser>>();
builder.Services.AddScoped<notifyDisplay>();
builder.Services.AddScoped<BLLDisplayFooter>();
builder.Services.AddScoped<BLLBranchDisplayFooter>();
builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
    );
///app.MapHub<notifyDisplay>("/signalr/hubs/signalr");
app.MapHub<notifyDisplay>("/notifyDisplay");

app.MapControllers();

app.Run();