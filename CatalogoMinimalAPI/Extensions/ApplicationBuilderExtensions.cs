using static System.Net.Mime.MediaTypeNames;

namespace CatalogoMinimalAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseExecptionHandling(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if(environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // using static System.Net.Mime.MediaTypeNames;  
                context.Response.ContentType = Text.Plain;

                var exceptionHandlerPathFeature =
                   context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

                if (exceptionHandlerPathFeature?.Error is not null)
                {
                    await context.Response.WriteAsync(exceptionHandlerPathFeature?.Error.Message);
                }

            });
        });

        return app;
    }

    public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
    {
        app.UseCors(p =>
        {
            p.AllowAnyOrigin();
            p.WithMethods("GET");
            p.AllowAnyHeader();
        });

        return app;
    }

    public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app; 
    }
}
