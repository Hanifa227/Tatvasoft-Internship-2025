using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mission.Entities.context;
using Mission.Repositories.IRepositories;
using Mission.Repositories.Repositories;
using Mission.Services.Helpers;
using Mission.Services.IServices;
using Mission.Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MissionDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<JwtService, JwtService>();
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
app.UseRouting();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
