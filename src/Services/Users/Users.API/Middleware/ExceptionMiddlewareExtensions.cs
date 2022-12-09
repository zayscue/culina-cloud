using System.Dynamic;
using CulinaCloud.BuildingBlocks.Application.Common.Exceptions;
using CulinaCloud.Users.Application.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace CulinaCloud.Users.API.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                const string applicationJsonContentType = "application/json";
                const string errorCodePrefix = "users.api-e-";
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
                        case ValidationException ve:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}003";
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
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case EntityConflictException ce:
                            {
                                context.Response.StatusCode = StatusCodes.Status409Conflict;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}004";
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
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case NotFoundException ne:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}005";
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
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case CanNotHaveMoreThanOneAuthorException canNotHaveMoreThanOneAuthorException:
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}006";
                                errorResponse.message = canNotHaveMoreThanOneAuthorException.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = canNotHaveMoreThanOneAuthorException.Message,
                                        stackTrace = canNotHaveMoreThanOneAuthorException.StackTrace,
                                        source = canNotHaveMoreThanOneAuthorException.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case CanNotModifyRecipeEntitlementException canNotModifyRecipeEntitlementException:
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}007";
                                errorResponse.message = canNotModifyRecipeEntitlementException.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = canNotModifyRecipeEntitlementException.Message,
                                        stackTrace = canNotModifyRecipeEntitlementException.StackTrace,
                                        source = canNotModifyRecipeEntitlementException.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case RecipeHasNoAuthorException recipeHasNoAuthorException:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}008";
                                errorResponse.message = recipeHasNoAuthorException.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = recipeHasNoAuthorException.Message,
                                        stackTrace = recipeHasNoAuthorException.StackTrace,
                                        source = recipeHasNoAuthorException.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case RecipeAuthorEntitlementDoesNotMatch recipeAuthorEntitlementDoesNotMatch:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}009";
                                errorResponse.message = recipeAuthorEntitlementDoesNotMatch.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = recipeAuthorEntitlementDoesNotMatch.Message,
                                        stackTrace = recipeAuthorEntitlementDoesNotMatch.StackTrace,
                                        source = recipeAuthorEntitlementDoesNotMatch.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case UserDoesNotExistException userDoesNotExistException:
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}010";
                                errorResponse.message = userDoesNotExistException.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = userDoesNotExistException.Message,
                                        stackTrace = userDoesNotExistException.StackTrace,
                                        source = userDoesNotExistException.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case CanNotChangeRecipeAuthorException canNotChangeRecipeAuthorException:
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}011";
                                errorResponse.message = canNotChangeRecipeAuthorException.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = canNotChangeRecipeAuthorException.Message,
                                        stackTrace = canNotChangeRecipeAuthorException.StackTrace,
                                        source = canNotChangeRecipeAuthorException.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case CanNotDeleteRecipeAuthorException canNotDeleteRecipeAuthorException:
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}012";
                                errorResponse.message = canNotDeleteRecipeAuthorException.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = canNotDeleteRecipeAuthorException.Message,
                                        stackTrace = canNotDeleteRecipeAuthorException.StackTrace,
                                        source = canNotDeleteRecipeAuthorException.Source
                                    };
                                }
                                await context.Response.WriteAsJsonAsync((object)errorResponse);
                                return;
                            }
                        case NoEntitlementException noEntitlementException:
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                context.Response.ContentType = applicationJsonContentType;
                                errorResponse.errorCode = $"{errorCodePrefix}012";
                                errorResponse.message = noEntitlementException.Message;
                                if (env.IsDevelopment())
                                {
                                    errorResponse.exception = new
                                    {
                                        message = noEntitlementException.Message,
                                        stackTrace = noEntitlementException.StackTrace,
                                        source = noEntitlementException.Source
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
