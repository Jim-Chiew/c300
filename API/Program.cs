using Microsoft.EntityFrameworkCore; //byJim
using XshapeAPI.Models;  //byJim

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<xshapesContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionMain")));  //byJim xshapeDemoContext based of xshapeDemoContext.cs.  ConnectionMain based of connection string in appSetting.json

var app = builder.Build();

// Configure the HTTP request pipeline.  Added app.Environment.IsProduction() to use Sweagger in production.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
