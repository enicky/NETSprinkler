using AutoMapper.Extensions.ExpressionMapping;
using NETSprinkler.Business.DbContext.Configuration;
using NETSprinkler.Business.Helpers;
using NETSprinkler.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appSettings.json", optional:true, reloadOnChange:true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
// Add services to the container.

builder.Services.Configure<DbConfigurationOptions>(builder.Configuration.GetSection("DBConfiguration"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterSprinkler();
builder.Services.AddAutoMapper(c =>
{
    c.AddExpressionMapping();
}, typeof(MappingProfile).Assembly);


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