# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
  
# Copy everything else and build
COPY . .
RUN dotnet publish ./GuessTheGameApi -c Release -o out
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 7290
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GuessTheGameApi.dll"]