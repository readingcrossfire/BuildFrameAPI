using BLL;
using BLL.BLL_Drawls;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(options => {
    var configOptions = new StackExchange.Redis.ConfigurationOptions();
    string strHost = configuration.GetValue<string>("Host");
    int intPort = configuration.GetValue<int>("Port");
    configOptions.EndPoints.Add(strHost, intPort);
    configOptions.Password = configuration.GetValue<string>("Password");
    options.ConfigurationOptions = configOptions;
});

builder.Services.AddScoped<IDrawlsService, BLL_Drawls>();

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