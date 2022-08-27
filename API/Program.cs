using BLL.BLL_Drawls;
using BLL.BLL_Logs;
using BLL.BLL_MenuTypes;
using DAL.DAL_Logs;
using DAL.DAL_MenuTypes;
using EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options =>
{
    var configOptions = new StackExchange.Redis.ConfigurationOptions();
    string strHost = configuration.GetValue<string>("Host");
    int intPort = configuration.GetValue<int>("Port");
    configOptions.EndPoints.Add(strHost, intPort);
    configOptions.Password = configuration.GetValue<string>("Password");
    options.ConfigurationOptions = configOptions;
});

builder.Services.AddScoped<IDrawlsService, BLL_Drawls>();
builder.Services.AddScoped<ILogsService, BLL_Logs>();
builder.Services.AddScoped<IMenuTypesService, BLL_MenuTypes>();
builder.Services.AddScoped<DAL_Logs>();
builder.Services.AddScoped<DAL_MenuTypes>();

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