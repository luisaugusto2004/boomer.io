using System.Reflection;
using boomerio.Data;
using boomerio.DTOs;
using boomerio.Middleware;
using boomerio.Repositories.CharacterRepository;
using boomerio.Repositories.FranchiseRepository;
using boomerio.Repositories.QuoteRepository;
using boomerio.Services.CharacterService;
using boomerio.Services.FranchiseService;
using boomerio.Services.QuoteService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace boomerio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Boomer Shooter API",
                        Version = "v1",
                        Description = "API for Boomer Shooter Quotes, Characters and Franchises",
                    }
                );
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    return new BadRequestObjectResult(
                        new ApiError("BadRequest", 400, "One or more validation errors occurred.")
                    );
                };
            });
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IFranchiseRepository, FranchiseRepository>();
            services.AddScoped<IFranchiseService, FranchiseService>();
            // Add CORS policy to allow front end requests
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BoomerShooterDb"))
            );
        }
    }
}
