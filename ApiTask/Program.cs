using Microsoft.EntityFrameworkCore;
using TaskManagement.Application;
using TaskManagement.Application.Service;
using TaskManagement.Application.Services;
using TaskManagement.Domain.IRepositoreis;
using TaskManagement.Domain.IRepositories;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositoreis;
using TaskManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TaskDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 
builder.Services.AddScoped<ITaskRepository, EfTaskRepository>(); 
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<PedidoApplication>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
