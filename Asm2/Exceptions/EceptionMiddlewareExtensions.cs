using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft;
using Newtonsoft.Json;
using System;

namespace Asm2.Exceptions
{
    public static class EceptionMiddlewareExtensions
    {
        public static void ConfigureBuildInExceptionHandler(this IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory)
        {
            applicationBuilder.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var logger = loggerFactory.CreateLogger(nameof(ConfigureBuildInExceptionHandler));

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextError = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();
                    if (contextRequest != null)
                    {
                        var error = new
                        {
                            TimeStamp = DateTime.Now,
                            StatusCode= context.Response.StatusCode,
                            Message = contextError.Error.Message,
                            //Message = "Something wrong was happened!",
                            Path = contextRequest.Path
                        };

                        // write log to database
                        logger.LogError(JsonConvert.SerializeObject(error));

                        error = new
                        {
                            TimeStamp = DateTime.Now,
                            StatusCode = context.Response.StatusCode,
                            Message = "Something wrong was happened!",
                            Path = contextRequest.Path
                        };

                        // write to file
                        await context.Response.WriteAsJsonAsync(error);
                    }
                });
            });
        }
    }
}
