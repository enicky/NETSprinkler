FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NETSprinkler.ApiWorker/NETSprinkler.ApiWorker.csproj", "NETSprinkler.ApiWorker/"]
RUN dotnet restore "NETSprinkler.ApiWorker/NETSprinkler.ApiWorker.csproj"
COPY . .
WORKDIR "/src/NETSprinkler.ApiWorker"
RUN dotnet build "NETSprinkler.ApiWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NETSprinkler.ApiWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NETSprinkler.ApiWorker.dll"]
