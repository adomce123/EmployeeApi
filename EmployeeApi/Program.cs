using EmployeeApi.Controllers.Models.Validators;
using EmployeeApi.Core.EmployeesService;
using EmployeeApi.Core.EmployeesService.Interfaces;
using EmployeeApi.Infrastructure;
using EmployeeApi.Infrastructure.Repositories;
using EmployeeApi.Infrastructure.Repositories.Interfaces;
using EmployeeApi.Middleware;
using EmployeeApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

var connectionString = configuration.GetConnectionString("EmployeesDb") ??
    throw new ArgumentException("Connection string was not specified");

services.AddDbContext<EmployeesDbContext>(
    opt => opt.UseSqlServer(connectionString));

services.AddControllers();
services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
services.AddValidatorsFromAssemblyContaining<EmployeeCreationValidator>();
services.AddValidatorsFromAssemblyContaining<EmployeeSearchRequestValidator>();
services.AddValidatorsFromAssemblyContaining<EmployeeSalaryValidator>();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IEmployeesRepository, EmployeesRepository>();
services.AddScoped<IEmployeesService, EmployeesService>();

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
