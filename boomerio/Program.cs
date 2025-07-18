using System.Reflection;
using boomerio.Data;
using boomerio.DTOs;
using boomerio.Middleware;
using boomerio.Repositories.CharacterRepository;
using boomerio.Repositories.FranchiseRepository;
using boomerio.Repositories.QuoteRepository;
using boomerio.Services.Cache.CharactersCache;
using boomerio.Services.Cache.FranchisesCache;
using boomerio.Services.Cache.QuotesCache;
using boomerio.Services.CharacterService;
using boomerio.Services.FranchiseService;
using boomerio.Services.QuoteService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var isDocker = builder.Environment.IsEnvironment("Docker");
                var connectionStringName = isDocker ? "DefaultConnection" : "BoomerShooterDb";
                options.UseSqlite(builder.Configuration.GetConnectionString(connectionStringName));
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors("AllowAll");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (!app.Environment.IsEnvironment("Docker"))
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            using(var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

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
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<IFranchiseRepository, FranchiseRepository>();

            services.AddScoped<QuoteService>();
            services.AddScoped<CharacterService>();
            services.AddScoped<FranchiseService>();

            services.AddScoped<IQuoteService>(provider =>
            {
                var realService = provider.GetService<QuoteService>()!;
                var cache = provider.GetRequiredService<IMemoryCache>();
                return new QuoteCacheService(realService, cache);
            });

            services.AddScoped<ICharacterService>(provider =>
            {
                var realService = provider.GetService<CharacterService>()!;
                var cache = provider.GetRequiredService<IMemoryCache>();
                return new CharactersCacheService(realService, cache);
            });

            services.AddScoped<IFranchiseService>(provider =>
            {
                var realService = provider.GetService<FranchiseService>()!;
                var cache = provider.GetRequiredService<IMemoryCache>();
                return new FranchiseCacheService(realService, cache);
            });


            // Add CORS policy to allow front end requests
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddMemoryCache();
        }
    }
}
