
using App.Handlers;
using App.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Reflection;
using System.Text.Json.Serialization;

namespace App
{
    public class Program
    {
        public static async Task Main(string[] args)    
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/error-.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
                .WriteTo.File("logs/all-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IApplicationSeeder, ApplicationSeeder>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IUserService, UserService>();    
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


            builder.Services.AddScoped<ExceptionHandlingMiddleware>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            var app = builder.Build();
            var scope = app.Services.CreateScope();


            var seeder = scope.ServiceProvider.GetRequiredService<IApplicationSeeder>();
            await seeder.SeedAsync();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();   

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
