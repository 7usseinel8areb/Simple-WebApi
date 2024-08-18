
using DotNetCore.Domain.RepositoriesInterface;
using DotNetCore.Persistance.Repositories;
using DotNetCore_WebApi.Filters;
using DotNetCore_WebApi.Middlewares;

namespace DotNetCore_WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("con");
            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                //Add Any Filter globaly at any controller
                options.Filters.Add<LogActivityFilter>();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IProductRepository, ProductRepository>(provider =>
                new ProductRepository(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            //app.UseAuthentication();

            //Custom Middleware
            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseMiddleware<ProfilingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
