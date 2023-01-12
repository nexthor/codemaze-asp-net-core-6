using CompanyEmployees.Api.Extensions;
using CompanyEmployees.Extensions;
using CompanyEmployees.Helpers;
using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
$"/nlog.{builder.Environment.EnvironmentName}.config"));

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // suppressing a default model state validation that is implemented
    // due to the existence of the [ApiController] attribute in all
    // API controllers
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader= true;
    // display 406 code error if the format requested is invalid
    config.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters()
.AddCustomCSVFormatter() // Custom CSV formatter
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// custom exception handler (API > Extensions > ExceptionMiddlewareExtensions.cs
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors(Constants.CorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
