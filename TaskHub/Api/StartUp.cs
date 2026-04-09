using Api.Filters;
using Api.Middleware;
using Api.Services;
using Api.UseCases.Tasks;
using Api.UseCases.Tasks.Interfaces;
using Api.UseCases.Users;
using Api.UseCases.Users.Interfaces;
using Dal;
using Logic;
using Microsoft.OpenApi.Models;

namespace Api;

/// <summary>
/// Конфигурация приложения
/// </summary>
public sealed class Startup
{
    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    private IConfiguration Configuration { get; }

    /// <summary>
    /// Окружение приложения
    /// </summary>
    private IWebHostEnvironment Environment { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        Environment = env;
    }

    /// <summary>
    /// Регистрация сервисов в DI контейнере
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        // Добавляем контроллеры
        services.AddControllers();

        // Регистрация слоёв Dal и Logic
        services.AddDal();
        services.AddLogic();

        // Регистрация UseCase'ов
        services.AddScoped<IManageUserUseCase, ManageUserUseCase>();
        services.AddScoped<IManageTaskUseCase, ManageTaskUseCase>();

        // Регистрация фильтров
        services.AddScoped<StudentInfoHeadersFilter>();
        services.AddScoped<RequestLoggingFilter>();
        services.AddScoped<ValidateCreateTaskRequestFilter>();
        services.AddScoped<ValidateSetTaskTitleRequestFilter>();

        // Настройка CORS
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        // Добавляем поддержку Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TaskHub Api",
                Version = "v1"
            });
        });

        // Регистрация сервисов для демонстрации жизненных циклов
        services.AddSingleton<SingletonService1>();
        services.AddSingleton<SingletonService2>();
        services.AddScoped<ScopedService1>();
        services.AddScoped<ScopedService2>();
        services.AddTransient<TransientService1>();
        services.AddTransient<TransientService2>();
    }

    /// <summary>
    /// Конфигурация middleware пайплайна обработки запросов
    /// </summary>
    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskHub API v1");
            });
        }

        app.UseMiddleware<StudentInfo>();
        app.UseMiddleware<ResponseTime>();

        // Маршрутизация
        app.UseRouting();

        // Маппинг контроллеров
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}