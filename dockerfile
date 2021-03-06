FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic
RUN apt-get update 
RUN apt-get install -y libfreetype6 
RUN apt-get install -y libfontconfig1

WORKDIR /app

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "linkedread.api.images.dll"]