using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Net.Mime;
using OrderManagementAPI.Repositories;
using OrderManagementAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Management", Version = "v1" });
});

builder.Services.AddTransient<CustomerRepository>();
builder.Services.AddTransient<OrderRepository>();
builder.Services.AddTransient<OrderDetailRepository>();
builder.Services.AddTransient<ProductRepository>();


builder.Services.AddDbContext<OrderManagementAPI.Data.OrdermanagementContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("OrderManagementDB"));
        options.EnableSensitiveDataLogging();
    }
    );

builder.Services.AddHealthChecks()
    .AddSqlServer(
    builder.Configuration.GetConnectionString("OrderManagementDB"),
    name: "sqlServer",
    timeout: TimeSpan.FromSeconds(3),
    tags: new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Management v1"));
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();


app.UseRouting();
app.MapControllers();

app.UseAuthorization();
app.Run();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = (check) => check.Tags.Contains("ready"),
        ResponseWriter = async (context, report) =>
        {
            var result = JsonSerializer.Serialize(
                new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                        duration = entry.Value.Duration.ToString()
                    })
                }
            );

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });

    endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = (_) => false
    });
});
