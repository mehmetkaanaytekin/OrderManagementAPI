using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Net.Mime;
using OrderManagementAPI.Repositories;
using OrderManagementAPI.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddHealthChecks()
    .AddSqlServer(
            connectionString: builder.Configuration["Data:ConnectionStrings:OrderManagementDB"],
            healthQuery: "SELECT 1;",
            name: "sql",
            failureStatus: HealthStatus.Degraded,
            tags: new string[] { "db", "sql", "sqlserver" });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderManagementAPI", Version = "v1" });
});

builder.Services.AddTransient<CustomerRepository>();
builder.Services.AddTransient<OrderRepository>();
builder.Services.AddTransient<OrderDetailRepository>();
builder.Services.AddTransient<ProductRepository>();


builder.Services.AddDbContext<OrdermanagementContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("OrderManagementDB"));
        options.EnableSensitiveDataLogging();
    }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderManagementAPI v1"));
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

    endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    endpoints.MapHealthChecks("/healthz/live", new HealthCheckOptions
    {
        Predicate = _ => false
    });
});
