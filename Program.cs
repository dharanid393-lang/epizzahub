using ePizza.API.HealthCheck;
using ePizza.API.Middleware;
using ePizza.Application;
using ePizza.Infrastructure;
using ePizza.Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;
using System.Text.Json;



namespace ePizza.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // start of serilog configuration
            Serilog.Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
                
            builder.Host.UseSerilog();
            // end of serilog configuration

            // for the in memory caching
            builder.Services.AddMemoryCache();

            // Add services to the container.



            // because we are using the Http client inside the constructor and it convey the dependency injection container that its having the httpclient in it
            builder.Services.AddHttpClient<RegresApiHealthCheck>();

            builder.Services.AddHealthChecks()
                .AddCheck<RegresApiHealthCheck>("Regres API status check")
               .AddCheck<SecondApiHealthCheck>("second API status check");

            //if we use the map word then it will used only if Execute based on the requirement in the pipeline request or if we use the keyword use it will usd automatically at every request.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                        ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
                        ValidAudience = builder.Configuration["Jwt:Audience"]!,
                        ClockSkew = TimeSpan.Zero,
                        //ValidateIssuer = true,
                        //ValidateAudience = true
                    };
                });


            // added reference of Application Layer- for using serviceregisteringpage
            builder.Services.AddAutoMapper(typeof(ApplicationAssemblyMarker).Assembly);
            builder.Services.AddApplication();

            builder.Services.AddSwaggerGen();



            // adding reference of Infra layer -- for using serviceregisteringpage
            builder.Services.AddAutoMapper(typeof(InfraAssemblyMarker).Assembly);
            builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DatabaseConnection")!);

            builder.Services.AddDbContext<ePizzaDbContext>(options =>
                     options.UseSqlServer(
            builder.Configuration.GetConnectionString("DatabaseConnection"),
            sql => sql.EnableRetryOnFailure()
    ));


            var app = builder.Build();

            // written the below code to customize the detailed response to the end user.
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, message) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(new
                    {
                        status = message.Status.ToString(),
                        checks = message.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            description = entry.Value.Description
                        }),
                        timeTaken = message.TotalDuration
                    });

                    await context.Response.WriteAsync(result);
                }
            });


            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<CommonResponseMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
