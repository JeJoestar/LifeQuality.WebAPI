using Hangfire;
using Hangfire.PostgreSql;
using LifeQuality.Core;
using LifeQuality.Core.Services;
using LifeQuality.DataContext;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options=> options.UseNpgsql(connectionString));
builder.Services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped(typeof(IDataRepository<User>), typeof(DataRepository<User>));

builder.Services.AddHangfire(configuration => configuration
        .UsePostgreSqlStorage(connectionString));

builder.Services.AddControllers();

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
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LifeQuality", Version = "v1" });
});

var app = builder.Build();

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

app.Run();
