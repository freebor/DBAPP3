
using DBAPP3.DataSeeding;
using DBAPP3.Repository;
using DBAPP3.Services;
using Microsoft.AspNetCore.Http;

namespace DBAPP3
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient<IThirdPartyService, ThirdPartyService>();
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddMemoryCache();
            builder.Services.AddRazorPages();


            var app = builder.Build();

            // Seed the database with initial data
            //using var scope = app.Services.CreateScope();
            //var repo = scope.ServiceProvider.GetRequiredService<ICountryRepository>();
            //await CountrySeeding.SeedAsync(repo);

            await Task.Run(async () =>
            {
                using var scope = app.Services.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ICountryRepository>();
                await CountrySeeding.SeedAsync(repo);
            });

            //Register the Razor middleware
            app.MapRazorPages();

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

            
            //static async Task SeedDatabaseAsync(WebApplication app)
            //{
            //    using var scope = app.Services.CreateScope();
            //    var repo = scope.ServiceProvider.GetRequiredService<ICountryRepository>();
            //    await CountrySeeding.SeedAsync(repo);
            //}
        }
    }
}
