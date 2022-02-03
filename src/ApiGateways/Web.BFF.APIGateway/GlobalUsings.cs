﻿global using System.Net;
global using System.Net.Http.Headers;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using Microsoft.Extensions.Options;
global using Amazon.SecretsManager;
global using Microsoft.AspNetCore.Mvc;
global using CulinaCloud.BuildingBlocks.CurrentUser;
global using CulinaCloud.BuildingBlocks.Common.Interfaces;
global using CulinaCloud.BuildingBlocks.Authentication.Auth0.Secrets.Providers;
global using CulinaCloud.BuildingBlocks.Authentication.Abstractions;
global using CulinaCloud.BuildingBlocks.Authentication.Auth0;
global using CulinaCloud.BuildingBlocks.Authentication.Auth0.Settings;
global using CulinaCloud.BuildingBlocks.CurrentUser.Abstractions;
global using CulinaCloud.BuildingBlocks.Application.Common.Models;
global using CulinaCloud.Web.BFF.APIGateway.Services;
global using CulinaCloud.Web.BFF.APIGateway.Interfaces;
global using CulinaCloud.Web.BFF.APIGateway.Models;
global using CulinaCloud.Web.BFF.APIGateway.Settings;
global using CulinaCloud.Web.BFF.APIGateway.Exceptions;