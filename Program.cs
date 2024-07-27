using Microsoft.EntityFrameworkCore;
using RecolimentoAtraso.Database.Context;
using RecolimentoAtraso.Model.Entity;
using RecolimentoAtraso.Model.Metodos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<TabelaJuros>();
builder.Services.AddScoped<Metodos>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=CalculaJurosDb;User Id=User;Password=@Admin123;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicinando o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
