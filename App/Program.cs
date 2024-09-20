
using App.Authorization;
using App.Dtos.CreateDtos;
using App.Dtos.QueryParams;
using App.Handlers;
using App.Services;
using App.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Reflection;
using System.Text;


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
            builder.Services.AddScoped<IUserContextService, UserContextService>();  
            builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();    
            builder.Services.AddScoped<IValidator<LoginUserDto>, LoginUserDtoValidator>();    
            builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryParamsValidator>();      
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
                
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            builder.Services.AddScoped<ExceptionHandlingMiddleware>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            var jwtOptionSection = builder.Configuration.GetRequiredSection("Jwt");
            builder.Services.Configure<JwtOptions>(jwtOptionSection);


            var jwtOptions = new JwtOptions();
            jwtOptionSection.Bind(jwtOptions);
            builder.Services.AddSingleton(jwtOptions);


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(jwtOptions =>
            {
                var configKey = jwtOptionSection["Key"];
                var key = Encoding.UTF8.GetBytes(configKey);

                             

                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptionSection["Issuer"],
                    ValidAudience = jwtOptionSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                   
                };
            });

            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder =>
                        builder.AllowAnyHeader()
                        .AllowAnyMethod()
                         .AllowAnyHeader()
                        .WithExposedHeaders("Location") 
                        .WithOrigins("http://localhost:5173")

                );
            });

            var app = builder.Build();
            var scope = app.Services.CreateScope();

            app.UseCors("FrontEndClient");

            var seeder = scope.ServiceProvider.GetRequiredService<IApplicationSeeder>();
            await seeder.SeedAsync();




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();   

            app.UseAuthorization();
         

            app.MapControllers();

            app.Run();
        }
    }
}
