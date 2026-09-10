using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using DiagnosisRepositoryApi.Data;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Entities;
using DiagnosisRepositoryApi.GraphQL;
using DiagnosisRepositoryApi.Handlers;
using DiagnosisRepositoryApi.Services;
using Scalar.AspNetCore;

namespace DiagnosisRepositoryApi.Configuration;

public static class ProgramExtension
{
    public static IServiceCollection ServicesSetUp(this IServiceCollection services)
    {
        services.AddOpenApi();

        services.AddControllers();

        services.AddHttpContextAccessor();
        services.AddHttpClient<RejestrHandler>(client =>
            client.BaseAddress = new Uri("http://localhost:5274")
        );

        var ConnectionString = "Data Source=Rejestracja.db";
        services.AddDbContext<MyDbContext>(option =>
        option.UseSqlite(ConnectionString));

        var json = File.ReadAllText("Documentation/ValueSet.json");
        var valueSet = JsonSerializer.Deserialize<ValueSet>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })
        ?? throw new InvalidOperationException("Value Set is invalid or contains errors");

        services.AddSingleton(valueSet);

        services.AddScoped<ICommandHandler<RegisterRequest, DTOs.Result<RegisterResponse>>, RegisterHandler>();
        services.Decorate<ICommandHandler<RegisterRequest, DTOs.Result<RegisterResponse>>, Decorators.Register.ValidationDecorator>();

        services.AddScoped<ICommandHandler<RejestrRequest, HttpResponseMessage>>(sp =>
            sp.GetRequiredService<RejestrHandler>());
        services.AddScoped<RetryService>();

        services.AddScoped<IQueryHandler<object, List<CollectionResponse>>, CollectionHandler>();

        services.AddScoped<IQueryHandler<ReadRecognitionRequest, DTOs.Result<List<ReadRecognitionResponse>>>, RecognitionReadHandler>();
        services.Decorate<IQueryHandler<ReadRecognitionRequest, DTOs.Result<List<ReadRecognitionResponse>>>, Decorators.Recognition.ValidationDecorator>();

        services.AddScoped<ICommandHandler<PatchClinicalStatusRequest, DTOs.Result<Object>>, PatchClinicalStatusHandler>();
        services.Decorate<ICommandHandler<PatchClinicalStatusRequest, DTOs.Result<Object>>, Decorators.ClinicalStatusPatch.ValidationDecorator>();

        services.AddTransient<MiddlewarePipeline.RequestLoggingMiddleware>();
        services.AddTransient<MiddlewarePipeline.ExceptionHandlingMiddleware>();

        services
            .AddGraphQLServer()
            .ModifyRequestOptions(options =>
            {
                options.IncludeExceptionDetails = true;
            })
            .AddQueryType<DiagnosesQuery>()
            .AddMutationType<DiagnosesMutation>();

        return services;
    }

    public static WebApplication GetApplications(this WebApplication app)
    {

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseMiddleware<MiddlewarePipeline.RequestLoggingMiddleware>();
        app.UseMiddleware<MiddlewarePipeline.ExceptionHandlingMiddleware>();

        app.MapControllers();
        app.MapGraphQL();

        return app;
    }

}