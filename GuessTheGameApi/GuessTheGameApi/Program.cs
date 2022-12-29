using GuessTheGameApi.Converter.Converters;
using GuessTheGameApi.DataAccess.Context;
using GuessTheGameApi.DataAccess.Repositories;
using GuessTheGameApi.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var MyAllowSpecificOrigins = "_MyAllowSubdomainPolicy";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddControllers();

builder.Services.AddScoped<IGameDomain, GameDomain>();
builder.Services.AddScoped<IGameRepository, GameRepository>(); 
builder.Services.AddScoped<IJwtAndHashGenerator, JwtAndHashGenerator>();

builder.Services.AddSingleton<ICurrentLevelConverter, CurrentLevelConverter>();
builder.Services.AddSingleton<IGameConverter, GameConverter>();
builder.Services.AddSingleton<ICredentialsConverter, CredentialsConverter>();

builder.Services.AddDbContext<GuessTheGameDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            //options.Authority = builder.Configuration["Jwt:Issuer"];
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
scope.ServiceProvider.GetService<GuessTheGameDBContext>().Database.Migrate();

app.Run();
