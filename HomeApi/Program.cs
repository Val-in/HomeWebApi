using System.Reflection;
using FluentValidation.AspNetCore;
using HomeApi;
using HomeApi.Configuration;
using HomeApi.Contracts.Validation;
using HomeApi.Data;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args); // 1. Создаём билдер приложения

// 2. Конфигурация: доступ к appsettings.json уже встроен в builder.Configuration
// Не нужно вручную создавать ConfigurationBuilder

// 3. Регистрация сервисов (ранее в Startup.ConfigureServices)
var assembly = Assembly.GetAssembly(typeof(MappingProfile));
builder.Services.AddAutoMapper(assembly); //Регистрация AutoMapper с переданным сборкой.

builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>(); //Регистрируем репозитории как Singleton (один экземпляр на весь сервис).
builder.Services.AddSingleton<IRoomRepository, RoomRepository>();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HomeApiContext>(options => options.UseNpgsql(connection), ServiceLifetime.Singleton);

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());

builder.Services.Configure<HomeOptions>(builder.Configuration); //Привязываем класс настроек к конфигурации.
builder.Services.Configure<Address>(builder.Configuration.GetSection("Address"));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => 
{ 
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" }); 
});

// 4. Создаём приложение
var app = builder.Build();

// 5. Конфигурация middleware (ранее в Startup.Configure)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeApi v1"));
}

app.UseHttpsRedirection(); //Перенаправление HTTP → HTTPS.
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // маршрутизация к контроллерам

app.Run(); // запускаем приложение