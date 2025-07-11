using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Net;

namespace Asm2.Exceptions
{
    public static class EceptionMiddlewareExtensions
    {
        public static void ConfigureBuildInExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextError = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();
                    if (contextRequest != null)
                    {
                        var error = new
                        {
                            StatusCode= context.Response.StatusCode,
                            Message = contextError.Error.Message,
                            Path = contextRequest.Path
                        };

                        await context.Response.WriteAsJsonAsync(error);
                    }
                });
            });
        }
    }
}
