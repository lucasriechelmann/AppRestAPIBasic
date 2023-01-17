using AppRestAPIBasic.API.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

namespace AppRestAPIBasic.API.Configurations;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(2, 0);
            options.ReportApiVersions= true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl= true;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;

        });

        services.AddCors(options =>
        {
            options.AddPolicy("Development",
                    builder =>
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            //options.AddPolicy("Production",
            //        builder =>
            //            builder
            //                .WithMethods("GET")
            //                .WithOrigins("http://test.ie")
            //                .SetIsOriginAllowedToAllowWildcardSubdomains()
            //                //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
            //                .AllowAnyHeader());
        });

        return services;
    }

    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("Development");
        }
        else
        {
            app.UseHsts();
            app.UseCors("Production");
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            //endpoints.MapHealthChecks("/api/hc", new HealthCheckOptions()
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});
            //endpoints.MapHealthChecksUI(options =>
            //{
            //    options.UIPath = "/api/hc-ui";
            //    options.ResourcesPath = "/api/hc-ui-resources";

            //    options.UseRelativeApiPath = false;
            //    options.UseRelativeResourcesPath = false;
            //    options.UseRelativeWebhookPath = false;
            //});
        });

        return app;
    }
}