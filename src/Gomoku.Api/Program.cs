using Gomoku.Core.Hubs;
using Gomoku.DAL;
using Gomoku.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.AddServices();
builder.Services.AddDbContext<GomokuDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GomokuDb")));
builder.Services.AddControllers();
builder.Services.AddSignalR()
    .AddJsonProtocol();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthorization();


app.MapControllers();

app.MapHub<GameHub>("/gameHub");

app.Run();
