using AML.TransactionTracker.API.Middleware;
using AML.TransactionTracker.API.TransactionValidator;
using AML.TransactionTracker.Application;
using AML.TransactionTracker.Infrastructure;
using AML.TransactionTracker.Infrastructure.RabbitMQ;
using AML.TransactionTracker.Infrastructure.SQLite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddDbContext<SQLiteContext>(o => o.UseSqlite(configuration.GetConnectionString("SQLiteDb")));
builder.Services.Configure<RabbitMqOptions>(c => configuration.GetSection(nameof(RabbitMqOptions)).Bind(c));

builder.Services.AddHostedService<WorkerHostedService>();

var app = builder.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionHandling>();

await app.RunAsync();
