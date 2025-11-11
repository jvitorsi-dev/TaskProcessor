using Application.Services;
using MongoDB.Driver;
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Application.Services;
using TaskProcessor.Domain.interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Services.Email;
using TaskProcessor.Domain.Services.Report;
using TaskProcessor.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<IMessageQueue, RabbitMqService>();

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddScoped<IMongoJobRepository, MongoJobRepository>();

builder.Services.AddScoped<EmailJobService>();
builder.Services.AddScoped<ReportJobService>();
builder.Services.AddScoped<IJobService>(provider => provider.GetRequiredService<EmailJobService>());
builder.Services.AddScoped<IJobService>(provider => provider.GetRequiredService<ReportJobService>());

builder.Services.AddScoped<IJobServiceResolver, JobServiceResolver>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
