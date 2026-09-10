using DiagnosisRepositoryApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ServicesSetUp();

var app = builder.Build();

app.GetApplications();

app.Run();