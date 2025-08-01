// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bicep.Local.Extension.Host;

public record Settings
{
    public required string Name
    {
        get => _name;
        init => _name = !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException("Name cannot be null or whitespace.", nameof(value));
    }

    public required string Version
    {
        get => _version;
        init => _version = !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException("Version cannot be null or whitespace.", nameof(value));
    }

    public required bool IsSingleton { get; init; }

    
    private readonly string _name = string.Empty;    
    private readonly string _version = string.Empty;
}
