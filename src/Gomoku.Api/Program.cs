using Gomoku.DAL;
using Microsoft.EntityFrameworkCore;
using Gomoku.Infrastructure.Configuration;

namespace Gomoku.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddConfiguration(builder.Configuration);

        // Add services to the container.

        builder.Services.AddDbContext<GomokuDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("GomokuDb")));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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
    }
}
