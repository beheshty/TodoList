# Specify the .NET SDK version
ARG DOTNET_SDK_VERSION=8.0
# Specify the ASP.NET Core Runtime version
ARG ASPNET_RUNTIME_VERSION=8.0

# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK_VERSION} AS build
WORKDIR /src

# Copy solution and project files
COPY ["TodoList.sln", "./"]
COPY ["src/TodoList.API/TodoList.API.csproj", "src/TodoList.API/"]
COPY ["src/TodoList.Application/TodoList.Application.csproj", "src/TodoList.Application/"]
COPY ["src/TodoList.Domain/TodoList.Domain.csproj", "src/TodoList.Domain/"]
COPY ["src/TodoList.Infrastructure/TodoList.Infrastructure.csproj", "src/TodoList.Infrastructure/"]
# If you have test projects, copy them too if needed for restore, but usually not needed for runtime image
COPY ["tests/TodoList.Application.Tests/TodoList.Application.Tests.csproj", "tests/TodoList.Application.Tests/"]
# ... copy other projects ...

# Restore dependencies
RUN dotnet restore "TodoList.sln"

# Copy the rest of the source code
COPY . .

# Build and publish the API project
WORKDIR "/src/src/TodoList.API"
RUN dotnet build "TodoList.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoList.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Final Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:${ASPNET_RUNTIME_VERSION} AS final
WORKDIR /app

# Expose the port the app runs on (matching ASPNETCORE_URLS in docker-compose.yml)
EXPOSE 8080
# EXPOSE 8081 # If using HTTPS

# Copy the published output from the publish stage
COPY --from=publish /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "TodoList.API.dll"] 