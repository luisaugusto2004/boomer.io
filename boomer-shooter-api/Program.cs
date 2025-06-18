using boomer_shooter_api.Data;
using boomer_shooter_api.Middleware;
using boomer_shooter_api.Repositories.CharacterRepository;
using boomer_shooter_api.Repositories.QuoteRepository;
using boomer_shooter_api.Services.CharacterService;
using boomer_shooter_api.Services.QuoteService;
using Microsoft.EntityFrameworkCore;
using SwaggerThemes;

namespace boomer_shooter_api
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(Theme.UniversalDark);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BoomerShooterDb")));
        }
    }
}
