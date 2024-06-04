using Gomoku.Core.Hubs;
using Gomoku.DAL;
using Gomoku.Infrastructure.Configuration;
using Gomoku.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidIssuer = "gomokuApi", // todo: set later
            ValidAudience = "gomokuFront", // todo: set later
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTKEY")), // todo: set later
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", cors => cors
        .WithOrigins(builder.Configuration["CorsOrigins"])
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("CorsPolicy");

app.UseAuthorization();


app.MapControllers();

app.MapHub<GameHub>("/gameHub");

app.Run();
