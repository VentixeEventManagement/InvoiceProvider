
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Presentation.Services;
using Swashbuckle.AspNetCore.Filters;

try
{

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            //policy.WithOrigins("http://localhost:5175") // frontend-port
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
    });
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen(o =>
    {
        o.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Invoice Service API",
            Description = "Official documentation for Event Service Provider API."
        });

        o.EnableAnnotations();
        o.ExampleFilters();
    });

    builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

    builder.Services.AddScoped<IInvoiceService, InvoiceService>();
    builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

    var app = builder.Build();
    app.UseCors("AllowAll");

    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice Service API v.1.0");
        c.RoutePrefix = string.Empty;
    });
    app.UseHttpsRedirection();



    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Startar en enkel webserver som svarar på alla requests med felmeddelandet
    var errorApp = WebApplication.CreateBuilder().Build();

    errorApp.Run(async context =>
    {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync($@"
            <html>
                <head><title>Startup Error</title></head>
                <body style='font-family:Arial; padding:2rem;'>
                    <h1 style='color:red;'>🚨 Fel vid uppstart av applikationen</h1>
                    <p>{ex.Message}</p>
                    <pre>{ex.StackTrace}</pre>
                </body>
            </html>");
    });

    errorApp.Run();
}