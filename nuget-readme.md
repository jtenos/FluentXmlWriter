# Publishing to NuGet

This document provides instructions for publishing the FluentXmlWriter package to NuGet.org.

## Prerequisites

1. **NuGet Account**: Create a free account at [nuget.org](https://www.nuget.org/) if you don't already have one.

2. **API Key**: Generate an API key from your NuGet account:
   - Log in to [nuget.org](https://www.nuget.org/)
   - Go to your account settings
   - Navigate to "API Keys"
   - Click "Create" and select appropriate permissions (at minimum: Push new packages and package versions)
   - Copy the generated API key (you won't be able to see it again)

3. **.NET SDK**: Ensure you have .NET SDK 8.0 or later installed.

## Building the Package

To create the NuGet package, run the following command from the repository root:

```bash
dotnet pack FluentXmlWriterCore/FluentXmlWriterCore.csproj --configuration Release --output ./nupkgs
```

This will:
- Build the project in Release configuration
- Create a `.nupkg` file in the `./nupkgs` directory
- The package file will be named `FluentXmlWriter.{version}.nupkg` where `{version}` is specified in the `.csproj` file

## Versioning

Before publishing, update the version number in `FluentXmlWriterCore/FluentXmlWriterCore.csproj`:

```xml
<Version>1.0.0</Version>
```

Follow [Semantic Versioning](https://semver.org/) guidelines:
- **MAJOR** version when you make incompatible API changes
- **MINOR** version when you add functionality in a backward compatible manner
- **PATCH** version when you make backward compatible bug fixes

## Publishing to NuGet.org

### Option 1: Using .NET CLI (Recommended)

1. **Set your API key** (only needed once per machine):
   ```bash
   dotnet nuget setapikey YOUR_API_KEY --source https://api.nuget.org/v3/index.json
   ```
   Replace `YOUR_API_KEY` with your actual NuGet API key. The key will be stored securely for future use.

2. **Push the package**:
   ```bash
   dotnet nuget push ./nupkgs/FluentXmlWriter.{version}.nupkg --source https://api.nuget.org/v3/index.json
   ```

   Replace `{version}` with the actual version number. The API key set in step 1 will be used automatically.

### Option 2: Using NuGet.org Web Interface

1. Navigate to [nuget.org/packages/manage/upload](https://www.nuget.org/packages/manage/upload)
2. Sign in to your account
3. Click "Browse" and select the `.nupkg` file from the `./nupkgs` directory
4. Click "Upload"
5. Review the package information and click "Submit"

## Verifying the Package

After publishing:

1. **Check package page**: Visit `https://www.nuget.org/packages/FluentXmlWriter/` to verify the package appears correctly.

2. **Test installation**: Try installing the package in a test project:
   ```bash
   dotnet new console -n TestProject
   cd TestProject
   dotnet add package FluentXmlWriter
   ```

3. **Check indexing**: It may take a few minutes for the package to be fully indexed and available for search.

## Updating the Package

To publish a new version:

1. Update the `<Version>` in `FluentXmlWriterCore/FluentXmlWriterCore.csproj`
2. Build the package using `dotnet pack`
3. Push the new package to NuGet.org

**Note**: Once a specific version is published to NuGet.org, it cannot be deleted or modified. You can only "unlist" it, which hides it from search results but keeps it available for existing users.

## Best Practices

- **Test thoroughly** before publishing
- **Update release notes** for each version
- **Tag releases** in Git to match NuGet versions:
  ```bash
  git tag -a v1.0.0 -m "Release version 1.0.0"
  git push origin v1.0.0
  ```
- **Keep API key secure** - never commit it to source control
- **Consider symbols package** for debugging support (use `--include-symbols` flag with `dotnet pack`)

## Troubleshooting

### Package Already Exists
If you see "The package already exists" error, you need to increment the version number in the `.csproj` file.

### Authentication Failed
Verify your API key is correct and has the necessary permissions. You may need to generate a new API key.

### Package Validation Failed
Review the error messages carefully. Common issues include:
- Missing required metadata
- Invalid package ID format
- License information issues

For more information, visit the [official NuGet documentation](https://learn.microsoft.com/en-us/nuget/).
