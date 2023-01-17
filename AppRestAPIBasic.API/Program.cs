using AppRestAPIBasic.API.Configurations;
using AppRestAPIBasic.API.Extensions;
using AppRestAPIBasic.Data.Context;
using HealthChecks.UI.Core.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfig();

builder.Services.ResolveDependencies();
//builder.Services.AddHealthChecks()
//    .AddCheck("Products", new SqlServerHealthCheck(builder.Configuration.GetConnectionString("DefaultConnection")))
//    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), "SqlDatabase");
//builder.Services.AddHealthChecksUI().AddSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var healthChecksDb = scope.ServiceProvider.GetRequiredService<HealthChecksDb>();
//    healthChecksDb.Database.Migrate();
//}

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
app.UseApiConfiguration(app.Environment);

app.UseSwaggerConfig(apiVersionDescriptionProvider);

app.Run();
