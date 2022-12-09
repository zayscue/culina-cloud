using Microsoft.AspNetCore.Diagnostics;
using System.Dynamic;

namespace CulinaCloud.Web.BFF.APIGateway.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                const string applicationJsonContentType = "application/json";
                const string errorCodePrefix = "webapigw-e-";
                const string emptyPayloadErrorMessage = "Unable to read the request as JSON because the request content type '' is not a known JSON content type.";

                appError.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var error = exceptionHandlerPathFeature.Error;
                    dynamic errorResponse = new ExpandoObject();
                    if (error.Message.Equals(emptyPayloadErrorMessage))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = applicationJsonContentType;
                        errorResponse.errorCode = $"{errorCodePrefix}002";
                        errorResponse.message = "Unable to read an empty request as JSON.";
                        if (env.IsDevelopment())
                        {
                            errorResponse.exception = new
                            {
                                message = error.Message,
                                stackTrace = error.StackTrace,
                                source = error.Source
                            };
                        }
                        await context.Response.WriteAsJsonAsync((object)errorResponse);
                        return;
                    }

                    switch (error)
                    {
                        case InternalServiceException ise:
                            {
                                context.Response.StatusCode = (int) ise.StatusCode;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = ise.ErrorCode ?? $"{errorCodePrefix}003";
                                errorResponse.message = ise.ErrorMessage ?? "An internal service exception has occurred.";
                                if (ise.ValidationErrors is {Count: > 0})
                                {
                                    errorResponse.validationErrors = ise.ValidationErrors;
                                }
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = ise.Message,
                                        stackTrace = ise.StackTrace,
                                        source = ise.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        default:
                            {
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}001";
                                errorResponse.message = "Internal Server Error.";
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = error.Message,
                                        stackTrace = error.StackTrace,
                                        source = error.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                    }
                });
            });
        }
    }
}
