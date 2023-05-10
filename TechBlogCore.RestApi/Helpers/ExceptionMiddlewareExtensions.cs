using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace TechBlogCore.RestApi.Helpers
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/plain";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"- - - * * * 错误: {contextFeature.Error}");
                        if (contextFeature.Error is MessageException)
                            await context.Response.WriteAsync(contextFeature.Error.Message);
                        else
                            await context.Response.WriteAsync("内部服务错误");
                    }
                });
            });
        }
    }
}
