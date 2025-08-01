// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Local.Extension.Host.Extensions;
using Bicep.Local.Extension.Mock;
using Bicep.Local.Extension.Mock.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder();

builder.AddBicepExtension(args)
    .WithSettings("MockExtension", "0.0.1", isSingleton: true)
    .WithConfiguration<Configuration>()
    .WithTypesAssembly(typeof(Program).Assembly)
    .WithHandlerAssembly(typeof(Program).Assembly);

var app = builder.Build();

app.MapBicepExtension();

await app.RunAsync();
