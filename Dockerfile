FROM microsoft/dotnet:2.1-sdk
WORKDIR /src

# Copy local files to Docker container.
COPY [".", "."]

# Restore Nuget packages.
RUN dotnet restore "SSW.Ports.sln" --verbosity m

# Verify the library work on Linux.
RUN dotnet test "SSW.Ports.AzureStorage.Adapter.Azure.PlatformTests/SSW.Ports.AzureStorage.Adapter.Azure.PlatformTests.csproj" -r linux-x64
RUN dotnet test "SSW.Ports.AzureStorage.Adapter.InMemory.PlatformTests/SSW.Ports.AzureStorage.Adapter.InMemory.PlatformTests.csproj" -r linux-x64
