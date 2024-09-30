using AiTiman_API.Models;
using AiTiman_API.Services.Interfaces;
using AiTiman_API.Services.Repositories;
using AiTiman_API.Services.Respositories;

namespace AiTiman_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // AiTiman Database.
            builder.Services.Configure<AiTimanDatabaseSettings>(
           builder.Configuration.GetSection("AiTimanDatabaseSettings"));


            // Add Scope of Interface and Repository
            builder.Services.AddScoped<IAppointment, AppointmentRepository>();
            builder.Services.AddScoped<IUsers, UsersRepository>();
            builder.Services.AddScoped<IBooked, BookedRepository>();
            builder.Services.AddSingleton<IUsers, UsersRepository>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
