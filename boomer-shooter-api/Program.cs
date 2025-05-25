using boomer_shooter_api.Data;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IQuoteService, QuoteService>();
            builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
            builder.Services.AddScoped<ICharacterService, CharacterService>();
            builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BoomerShooterDb"));
            });

            var app = builder.Build();

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
    }
}
