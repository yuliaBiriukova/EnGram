using EnGram.DB.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using FluentValidation;
using CatalogService.Infrastructure.Pipeline;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceDiscovery(o => o.UseEureka());

builder.Services.AddDbContext<EnGramDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EnGramDbConnection"), b => b.MigrationsAssembly("EnGram.DB")));

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseFluentValidationExceptionHandler();

app.MapControllers();

app.Run();
