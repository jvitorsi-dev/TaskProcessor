FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskProcessor.EmailJobWorker/TaskProcessor.EmailJobWorker.csproj", "TaskProcessor.EmailJobWorker/"]
COPY ["TaskProcessor.Application/TaskProcessor.Application.csproj", "TaskProcessor.Application/"]
COPY ["TaskProcessor.Domain/TaskProcessor.Domain.csproj", "TaskProcessor.Domain/"]
COPY ["TaskProcessor.Infrastructure/TaskProcessor.Infrastructure.csproj", "TaskProcessor.Infrastructure/"]
RUN dotnet restore "TaskProcessor.EmailJobWorker/TaskProcessor.EmailJobWorker.csproj"

COPY . .
WORKDIR "/src/TaskProcessor.EmailJobWorker"
RUN dotnet build "TaskProcessor.EmailJobWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskProcessor.EmailJobWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskProcessor.EmailJobWorker.dll"]
