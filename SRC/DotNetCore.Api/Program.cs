
using DotNetCore.Application.Handlers;
using DotNetCore.Infrastructure.Options;
using DotNetCore.Domain.RepositoriesInterface;
using DotNetCore.Persistance.Repositories;
using DotNetCore_WebApi.Filters;
using DotNetCore_WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace DotNetCore_WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
            
            builder.Configuration.AddJsonFile("config.json");

            #region Options
            //Get object From type AttachmentOptions
            /*//1
            var attachmentOptions = builder.Configuration.GetSection("Attachments")
                .Get<AttachmentOptions>();
            */
            /*//2
            AttachmentOptions attachmentOptions = new();
            builder.Configuration.GetSection("Attachments").Bind(attachmentOptions);
            */
            //builder.Services.AddSingleton(attachmentOptions);
            
            //Options pattern
            //Basic
            //3
            builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));

            #endregion


            var connectionString = builder.Configuration.GetConnectionString("con");
            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                //Add Any Filter globaly at any controller
                options.Filters.Add<LogActivityFilter>();
                options.Filters.Add<PermessionBasedAuthorizationFilter>();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            
            builder.Services.AddScoped<IProductRepository, ProductRepository>(provider =>
                new ProductRepository(connectionString));

            builder.Services.AddScoped<IConfigrationsRepository, ConfigurationsRepository>();
            
            builder.Services.AddScoped<IUsersRepository, UsersRepository>(provider =>
                new UsersRepository(jwtOptions, connectionString));

            builder.Services.AddSingleton(jwtOptions);

            #region Basic Authentication
            /*builder.Services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);*/
            #endregion


            builder.Services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
                {
                    //if the token is valid it will be saved at AuthenticationProerties to be accessed from the request
                    options.SaveToken = true;


                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = 
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                    };

                });

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
