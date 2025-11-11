# BASE IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# BUILD IMAGE
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskProcessor/TaskProcessor.csproj", "TaskProcessor/"]
COPY ["TaskProcessor.Application/TaskProcessor.Application.csproj", "TaskProcessor.Application/"]
COPY ["TaskProcessor.Domain/TaskProcessor.Domain.csproj", "TaskProcessor.Domain/"]
COPY ["TaskProcessor.Infrastructure/TaskProcessor.Infrastructure.csproj", "TaskProcessor.Infrastructure/"]
RUN dotnet restore "TaskProcessor/TaskProcessor.csproj"

COPY . .
WORKDIR "/src/TaskProcessor"
RUN dotnet build "TaskProcessor.csproj" -c Release -o /app/build

# PUBLISH IMAGE
FROM build AS publish
RUN dotnet publish "TaskProcessor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# FINAL IMAGE
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskProcessor.dll"]
