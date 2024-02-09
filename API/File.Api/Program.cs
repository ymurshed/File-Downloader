using File.Api.Models;
using File.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

#region Bind appSettings.json file
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();
#endregion;

#region Add DbContext
builder.Services.AddDbContextFactory<ReportingContext>(); // 30 min


//builder.Services.AddDbContext<ReportingContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ReportingConnection"),
//                         sqlServerOptions => sqlServerOptions.CommandTimeout(1800)); // 30 min
//});

//builder.Services.AddDbContext<ReportingContext>(options =>
//        options.UseSqlServer(builder.Configuration.GetConnectionString("ReportingConnection")),
//        ServiceLifetime.Transient);
#endregion

#region Add lib services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Add app services
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddScoped<ITsrService, TsrServicecs>();
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(op => op.AllowAnyOrigin());
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
#endregion