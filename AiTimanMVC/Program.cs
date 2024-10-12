using AiTimanMVC.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using AiTiman_API.Services.Interfaces;
using AiTiman_API.Services.Respositories;
using AiTiman_API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AiTimanDatabaseSettings>(
         builder.Configuration.GetSection("AiTimanDatabaseSettings"));

builder.Services.AddTransient<VerificationService>();
builder.Services.AddHttpClient();


builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IUsers, UsersRepository>();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // The path where users are redirected for login
        options.LogoutPath = "/Account/Logout"; // The path for logging out
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Session expiration time
        options.SlidingExpiration = true; // Renew cookie if the user is active
    });

var app = builder.Build();

var smtpClient = new SmtpClient("smtp.gmail.com")
{
    Port = 587,
    Credentials = new NetworkCredential("aitiman.soc@gmail.com", "ekjil fbgd pued hzsm"),
    EnableSsl = true,
};

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}/{id?}");

app.Run();
