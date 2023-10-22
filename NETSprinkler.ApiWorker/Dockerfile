FROM  --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


ARG TARGETPLATFORM
ARG TARGETOS
ARG TARGETARCH
ARG TARGETVARIANT
ARG BUILDPLATFORM
ARG BUILDOS
ARG BUILDARCH
ARG BUILDVARIANT
RUN echo "Building on $BUILDPLATFORM, targeting $TARGETPLATFORM"
RUN echo "Building on ${BUILDOS} and ${BUILDARCH} with optional variant ${BUILDVARIANT}"
RUN echo "Targeting ${TARGETOS} and ${TARGETARCH} with optional variant ${TARGETVARIANT}"

FROM  --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG TARGETARCH
ARG TARGETPLATFORM
WORKDIR /src
COPY ["NETSprinkler.ApiWorker/NETSprinkler.ApiWorker.csproj", "NETSprinkler.ApiWorker/"]
COPY ["NETSprinkler.Common/NETSprinkler.Common.csproj", "NETSprinkler.Common/"]
COPY ["NETSprinkler.ApiWorker.Business/NETSprinkler.ApiWorker.Business.csproj", "NETSprinkler.ApiWorker.Business/"]
COPY ["NETSprinkler.Contracts/NETSprinkler.Contracts.csproj", "NETSprinkler.Contracts/"]
COPY ["NETSprinkler.Models/NETSprinkler.Models.csproj", "NETSprinkler.Models/"]

RUN echo "TargetArch ${TARGETARCH}"
RUN dotnet restore  "NETSprinkler.ApiWorker/NETSprinkler.ApiWorker.csproj" --arch $TARGETARCH
COPY . .

WORKDIR "/src/NETSprinkler.ApiWorker"


RUN if [ "$TARGETPLATFORM" = "linux/amd64" ]; then \
    RID=linux-x64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm64" ]; then \
    RID=linux-arm64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm/v7" ]; then \
    RID=linux-arm ; \
    fi \
    && echo "RID = "  $RID \
    && dotnet build "NETSprinkler.ApiWorker.csproj" -c Release --no-restore  --arch $TARGETARCH
FROM build AS publish


RUN if [ "$TARGETPLATFORM" = "linux/amd64" ]; then \
    RID=linux-x64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm64" ]; then \
    RID=linux-arm64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm/v7" ]; then \
    RID=linux-arm ; \
    fi \
    && dotnet publish "NETSprinkler.ApiWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore --no-build --arch $TARGETARCH

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NETSprinkler.ApiWorker.dll"]
