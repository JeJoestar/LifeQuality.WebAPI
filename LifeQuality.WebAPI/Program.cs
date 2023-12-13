using Hangfire;
using Hangfire.PostgreSql;
using LifeQuality.Core;
using LifeQuality.Core.Services;
using LifeQuality.DataContext;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.Hubs;
using LifeQuality.WebAPI.Mappers;
using LifeQuality.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options=> options.UseNpgsql(connectionString));
builder.Services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped(typeof(IDataRepository<User>), typeof(DataRepository<User>));
builder.Services.AddScoped(typeof(IDataRepository<Patient>), typeof(DataRepository<Patient>));
builder.Services.AddScoped(typeof(IDataRepository<Doctor>), typeof(DataRepository<Doctor>));
builder.Services.AddScoped(typeof(IDataRepository<Recomendation>), typeof(DataRepository<Recomendation>));
builder.Services.AddScoped(typeof(IDataRepository<Sensor>), typeof(DataRepository<Sensor>));
builder.Services.AddScoped(typeof(IDataRepository<BloodAnalysisData>), typeof(DataRepository<BloodAnalysisData>));

builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddTransient<AnalyticsService>();
builder.Services.AddTransient<HangfireService>();
builder.Services.AddTransient<SensorClient>();
builder.Services.AddTransient<BloodAndAnalysisService>();

builder.Services.AddHangfire(configuration => configuration
        .UsePostgreSqlStorage(connectionString));

builder.Services.AddSingleton<IBackgroundJobClient>(provider => new BackgroundJobClient());

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Constants.jwtIssuer,
        ValidAudience = Constants.jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Constants.salt)
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            if (context.AuthenticateFailure is not null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                if (context.AuthenticateFailure is SecurityTokenExpiredException)
                {
                    return context.Response.WriteAsync("The token is expired. Use refresh token or reauthenticate the user.");
                }

                return context.Response.WriteAsync("Not Authorized");
            }

            return Task.CompletedTask;
        }
    };
});




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LifeQuality", Version = "v1" });
    c.AddSecurityDefinition(Constants.BearerAuth, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = Constants.BearerAuth,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new ()
            {
                Reference = new ()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = Constants.BearerAuth
                },
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<IDataContext>().Database.MigrateAsync();

if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
    app.UseHangfireServer();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LifeQuality v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<MainHub>("/hubs/main");

app.Run();
