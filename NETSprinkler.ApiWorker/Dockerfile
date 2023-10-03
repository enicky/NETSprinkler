FROM  --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:6.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM  --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0-bookworm-slim AS build
WORKDIR /src
COPY ["NETSprinkler.ApiWorker/NETSprinkler.ApiWorker.csproj", "NETSprinkler.ApiWorker/"]
COPY ["NETSprinkler.Common/NETSprinkler.Common.csproj", "NETSprinkler.Common/"]
COPY ["NETSprinkler.ApiWorker.Business/NETSprinkler.ApiWorker.Business.csproj", "NETSprinkler.ApiWorker.Business/"]
COPY ["NETSprinkler.Contracts/NETSprinkler.Contracts.csproj", "NETSprinkler.Contracts/"]
COPY ["NETSprinkler.Models/NETSprinkler.Models.csproj", "NETSprinkler.Models/"]


RUN dotnet restore "NETSprinkler.ApiWorker/NETSprinkler.ApiWorker.csproj"
COPY . .

WORKDIR "/src/NETSprinkler.ApiWorker"


RUN dotnet build "NETSprinkler.ApiWorker.csproj" -c Debug --no-restore

FROM build AS publish
ARG TARGETPLATFORM


RUN dotnet publish "NETSprinkler.ApiWorker.csproj" -c Debug -o /app/publish /p:UseAppHost=false --no-restore --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NETSprinkler.ApiWorker.dll"]
