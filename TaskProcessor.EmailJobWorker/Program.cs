

using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Infrastructure.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddHostedService<EmailJobWorker>();
builder.Services.AddSingleton<IMessageQueue, RabbitMqService>();
builder.Services.AddSingleton<IMongoJobRepository, MongoJobRepository>();

var host = builder.Build();
host.Run();