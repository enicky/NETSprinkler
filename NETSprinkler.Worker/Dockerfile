FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NETSprinkler.Worker/NETSprinkler.Worker.csproj", "NETSprinkler.Worker/"]
RUN dotnet restore "NETSprinkler.Worker/NETSprinkler.Worker.csproj"
COPY . .
WORKDIR "/src/NETSprinkler.Worker"
RUN dotnet build "NETSprinkler.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NETSprinkler.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NETSprinkler.Worker.dll"]
