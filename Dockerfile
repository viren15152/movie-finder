# Use the official image as the base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MovieFinder.csproj", "./"]  # Copy csproj from the root directory
RUN dotnet restore "./MovieFinder.csproj"  # Restore dependencies

# Copy the rest of the files into /src folder
COPY . /src/
WORKDIR "/src"
RUN dotnet build "MovieFinder.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "MovieFinder.csproj" -c Release -o /app/publish

# Final stage, copy the published app and set the entrypoint
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieFinder.dll"]
