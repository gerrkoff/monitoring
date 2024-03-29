using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace GerrKoff.Monitoring.Misc;

public static class UnhandledExceptionHandler
{
    public static void UseUnhandledExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment environment) =>
        app.UseUnhandledExceptionHandler(environment.IsProduction());

    public static void UseUnhandledExceptionHandler(this IApplicationBuilder app, bool isProduction)
    {
        app.UseExceptionHandler(x =>
        {
            x.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (isProduction)
                {
                    await context.Response.WriteAsJsonAsync(new { Message = "Something went wrong" });
                    return;
                }

                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandlerPathFeature == null)
                {
                    await context.Response.WriteAsJsonAsync(new { Message = "Failed to get exception" });
                    return;
                }

                var exception = exceptionHandlerPathFeature.Error;
                await context.Response.WriteAsJsonAsync(new Dictionary<string, string?>
                {
                    { "Type", exception.GetType().ToString() },
                    { "Message", exception.Message },
                    { "StackTrace", exception.StackTrace }
                });
            });
        });
    }
}
