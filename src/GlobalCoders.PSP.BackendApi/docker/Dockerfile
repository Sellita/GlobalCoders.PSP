ARG GIT_COMMIT=head
ARG BUILD_CONFIG=Release
ARG BUILD_DATE

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG GIT_COMMIT
ARG BUILD_CONFIG

WORKDIR /src

COPY /src/GlobalCoders.PSP.BackendApi/GlobalCoders.PSP.BackendApi.csproj ./src/GlobalCoders.PSP.BackendApi/

RUN dotnet restore ./src/GlobalCoders.PSP.BackendApi/GlobalCoders.PSP.BackendApi.csproj

COPY /src/GlobalCoders.PSP.BackendApi/ ./src/GlobalCoders.PSP.BackendApi/

RUN dotnet publish ./src/GlobalCoders.PSP.BackendApi/GlobalCoders.PSP.BackendApi.csproj \
    -c ${BUILD_CONFIG} \
    --self-contained false \
    --no-restore \
    --version-suffix ${GIT_COMMIT} \
    -o ./out 

FROM base AS runtime

WORKDIR /app

COPY --from=build /src/out ./

RUN useradd -u 2333 dotnetuser

RUN mkdir /app/data && chown -R dotnetuser:dotnetuser /app/data

USER dotnetuser

ENTRYPOINT ["dotnet", "GlobalCoders.PSP.BackendApi.dll"]