using System.Text;
using BLL.BLL_Authen;
using BLL.BLL_Drawls;
using BLL.BLL_FireBase;
using BLL.BLL_Logs;
using BLL.BLL_MenuTypes;
using BLL.BLL_WMS;
using CONNECTION.DapperConnection;
using DAL.DAL_Logs;
using EntityFramework;
using FIREBASE.SendNotification;
using FIREBASE.SendNotification.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ML.Entities;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .AddDbContext<NewsAppDbContext>(config => config.UseSqlServer(configuration.GetConnectionString("DB_NEWSAPI")))
    .AddIdentity<NewsAppUser, NewsAppRole>()
    .AddEntityFrameworkStores<NewsAppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(config =>
    {
        config.SaveToken = true;
        config.RequireHttpsMetadata = false;
        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                new string []{}
            }
        }
    );
});

builder.Services.AddScoped<IDrawlsService, BLL_Drawls>();
builder.Services.AddScoped<IFireBaseService, BLL_FireBase>();
builder.Services.AddScoped<IWMSLogsService, BLL_WMSLogs>();
builder.Services.AddScoped<ILogsService, BLL_Logs>();
builder.Services.AddScoped<DAL_Logs>();

builder.Services.AddScoped<IAuthenService, BLL_Authen>();
builder.Services.AddScoped<ISendNotification, SendNotification>();
builder.Services.AddScoped<IMenuTypesService, BLL_MenuTypes>();

builder.Services.AddSingleton<IDapperConnection, DapperConnection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();