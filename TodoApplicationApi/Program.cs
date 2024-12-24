using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using TodoApplicationApi.Data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
	throw new InvalidOperationException("The connection string is not defined in the environment variables.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseNpgsql(connectionString));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "Todo API",
		Version = "v1",
		Description = "API for managing a to-do list",
		Contact = new Microsoft.OpenApi.Models.OpenApiContact
		{
			Name = "Oskar E. Torres O.",
			Email = "oskar.torres.1234@gmail.com"
		}
	});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
		c.RoutePrefix = "api/doc";
	});
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
