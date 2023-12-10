using LifeQuality.DataContext;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options=> options.UseNpgsql(connectionString));
builder.Services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());

builder.Services.AddScoped(typeof(IDataRepository<User>), typeof(DataRepository<User>));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
