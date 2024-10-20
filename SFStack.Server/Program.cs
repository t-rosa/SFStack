using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SFStack.Server;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Services
        // DB Context
        builder.Services.AddDbContext<SFStackContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Authentication
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(options =>
                        {
                            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                            options.Cookie.HttpOnly = true;
                            options.Events = new CookieAuthenticationEvents
                            {
                                OnRedirectToLogin = context =>
                                {
                                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                    return Task.CompletedTask;
                                },
                            };
                        });

        // Error messages
        builder.Services.AddProblemDetails();

        // Controllers
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<FluentValidationFilter>();
        });

        // Open API
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Mapper
        builder.Services.AddAutoMapper(typeof(Program));

        // Validation
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}