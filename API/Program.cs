using BLL;
using BLL.BLL_DrawlsFolder.Interface;
using BLL.BLL_Logs;
using BLL.BLL_Logs.Interface;
using CONNECTION;
using CONNECTION.Interface;
using DAL.DAL_Logs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["RedisCacheUrl"]; });

builder.Services.AddScoped<IDrawlsService, BLL_Drawls>();
builder.Services.AddScoped<ILogsService, BLL_Logs>();
builder.Services.AddScoped<IDapperConnectionDI, DapperConnectionDI>();
builder.Services.AddScoped<DAL_Logs>();

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