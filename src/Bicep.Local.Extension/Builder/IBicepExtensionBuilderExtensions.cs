// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;
using Bicep.Local.Extension.Host;
using Bicep.Local.Extension.Host.Extensions;
using Bicep.Local.Extension.Host.Handlers;
using Bicep.Local.Extension.Types;

namespace Microsoft.Extensions.DependencyInjection;

public static class IBicepExtensionBuilderExtensions
{
    /// <summary>
    /// Registers a resource handler that manages a specific resource type in your Bicep extension.
    /// </summary>
    public static IBicepExtensionBuilder WithResourceHandler<T>(this IBicepExtensionBuilder builder)
        where T : class, IResourceHandler
    {
        builder.Services.AddSingleton<IResourceHandler, T>();

        return builder;
    }
    
    /// <summary>
    /// Registers a resource handler that manages a specific resource type in your Bicep extension.
    /// </summary>
    public static IBicepExtensionBuilder WithResourceHandler(this IBicepExtensionBuilder builder, IResourceHandler resourceHandler)
    {
        builder.Services.AddSingleton<IResourceHandler>(resourceHandler);

        return builder;
    }

    public static IBicepExtensionBuilder WithSettings(this IBicepExtensionBuilder builder, string name, string version, bool isSingleton)
    {
        builder.Services.AddSingleton(new Settings
        {
            Name = name,
            Version = version,
            IsSingleton = isSingleton
        });

        return builder;
    }

    public record HandlerAssemblies(Assembly[] Assemblies);
    public static IBicepExtensionBuilder WithHandlerAssembly(this IBicepExtensionBuilder builder, Assembly handlerAssembly)
        => WithHandlerAssembly(builder, [handlerAssembly]);

    public static IBicepExtensionBuilder WithHandlerAssembly(this IBicepExtensionBuilder builder, Assembly[] handlerAssembly)
    {
        ArgumentNullException.ThrowIfNull(handlerAssembly, nameof(handlerAssembly));
        var handlers = handlerAssembly
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IResourceHandler).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            .Select(handlerType => builder.Services.AddSingleton(typeof(IResourceHandler), handlerType))
            .ToArray();


        return builder;
    }

    public record TypeAssemblies(Assembly[] Assemblies);

    public static IBicepExtensionBuilder WithTypesAssembly(this IBicepExtensionBuilder builder, Assembly typeAssembly)
        => WithTypesAssembly(builder, [typeAssembly]);

    public static IBicepExtensionBuilder WithTypesAssembly(this IBicepExtensionBuilder builder, Assembly[] typeAssembly)
    {
        ArgumentNullException.ThrowIfNull(typeAssembly, nameof(typeAssembly));
        if (typeAssembly.Length == 0)
        {
            throw new ArgumentException("At least one assembly must be provided.", nameof(typeAssembly));
        }
        builder.Services.AddSingleton(new TypeAssemblies(typeAssembly));
        return builder;
    }

    public static IBicepExtensionBuilder WithConfiguration<T>(this IBicepExtensionBuilder builder)
        where T : class
    {
        builder.Services.AddKeyedSingleton("configurationType", typeof(T));
        return builder;
    }

    public static IBicepExtensionBuilder WithTypeProvider(this IBicepExtensionBuilder builder, ITypeProvider typeProvider)
    {
        builder.Services.AddSingleton<ITypeProvider>(typeProvider);
        return builder;
    }

    public static IBicepExtensionBuilder WithTypeDefinitionBuilder(this IBicepExtensionBuilder builder, ITypeDefinitionBuilder typeDefinitionBuilder)
    {
        builder.Services.AddSingleton<ITypeDefinitionBuilder>(typeDefinitionBuilder);
        return builder;
    }


}

