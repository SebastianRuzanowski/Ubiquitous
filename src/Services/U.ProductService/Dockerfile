FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /source
ARG NUGET_CONFIG_FILE

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY U.ProductService.Application/*.csproj ./U.ProductService.Application
COPY U.ProductService.Domain/*.csproj ./U.ProductService.Domain
COPY U.ProductService.Persistance/*.csproj ./U.ProductService.Persistance
COPY U.ProductService.BackgroundService/*.csproj ./U.ProductService.BackgroundService
COPY U.ProductService.DomainTests/*.csproj ./U.ProductService.DomainTests
COPY U.ProductService.ApplicationTests/*.csproj ./U.ProductService.ApplicationTests
COPY U.ProductService.IntegrationTests/*.csproj ./U.ProductService.IntegrationTests
COPY . .
RUN dotnet restore "./U.ProductService.csproj" /property:Configuration=Release --configfile "${NUGET_CONFIG_FILE}"

# copy everything else and build app
WORKDIR /source
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build /app ./

# environment variables
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "U.ProductService.dll"]