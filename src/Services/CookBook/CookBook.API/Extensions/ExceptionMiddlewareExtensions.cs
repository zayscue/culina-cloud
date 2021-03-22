using System.Dynamic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Culina.CookBook.Application.Common.Exceptions;

namespace Culina.CookBook.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                const string ApplicationJSONContentType = "application/json";
                const string ErrorCodePrefix = "cookbook.api-e-";
                const string EmptyPayloadErrorMessage = "Unable to read the request as JSON because the request content type '' is not a known JSON content type.";

                appError.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var error = exceptionHandlerPathFeature.Error;
                    dynamic errorResponse = new ExpandoObject();
                    if (error.Message.Equals(EmptyPayloadErrorMessage))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = ApplicationJSONContentType;
                        errorResponse.errorCode = $"{ErrorCodePrefix}002";
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
                        await context.Response.WriteAsJsonAsync(errorResponse as object);
                        return;
                    }

                    if (error is ValidationException ve)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = ApplicationJSONContentType;
                        errorResponse.errorCode = $"{ErrorCodePrefix}003";
                        errorResponse.message = ve.Message;
                        errorResponse.validationErrors = ve.Errors;
                        if (env.IsDevelopment())
                        {
                            errorResponse.exception = new
                            {
                                message = ve.Message,
                                stackTrace = ve.StackTrace,
                                source = ve.Source
                            };
                        }
                        await context.Response.WriteAsJsonAsync(errorResponse as object);
                        return;
                    }

                    if (error is EntityConflictException ce)
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        context.Response.ContentType = ApplicationJSONContentType;
                        errorResponse.errorCode = $"{ErrorCodePrefix}004";
                        errorResponse.message = ce.Message;
                        if (env.IsDevelopment())
                        {
                            errorResponse.exception = new
                            {
                                message = ce.Message,
                                stackTrace = ce.StackTrace,
                                source = ce.Source
                            };
                        }
                        await context.Response.WriteAsJsonAsync(errorResponse as object);
                        return;
                    }

                    if (error is NotFoundException ne)
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        context.Response.ContentType = ApplicationJSONContentType;
                        errorResponse.errorCode = $"{ErrorCodePrefix}005";
                        errorResponse.message = ne.Message;
                        if (env.IsDevelopment())
                        {
                            errorResponse.exception = new
                            {
                                message = ne.Message,
                                stackTrace = ne.StackTrace,
                                source = ne.Source
                            };
                        }
                        await context.Response.WriteAsJsonAsync(errorResponse as object);
                        return;
                    }


                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = ApplicationJSONContentType;
                    errorResponse.errorCode = $"{ErrorCodePrefix}001";
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
                    await context.Response.WriteAsJsonAsync(errorResponse as object);
                });
            });
        }    
    }
}
