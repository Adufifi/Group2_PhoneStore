﻿

namespace PhoneStore.Api.Extension
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureBuildInExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorVm()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Path = contextRequest!.Path
                        }));

                    }
                });
            });
        }
    }
}
