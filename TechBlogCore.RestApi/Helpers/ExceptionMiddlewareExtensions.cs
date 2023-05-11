using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text;

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
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"- - - * * * 错误: {contextFeature.Error}");
                        if (contextFeature.Error is MessageException)
                            await context.Response.WriteAsync(contextFeature.Error.Message, Encoding.UTF8);
                        else
                            await context.Response.WriteAsync("内部服务错误", Encoding.UTF8);
                    }
                });
            });
        }
    }
}
