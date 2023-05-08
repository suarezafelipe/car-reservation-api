FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /build

# Copy the entire solution
COPY . .

# Restore and publish the API project
RUN dotnet restore API/API.csproj
RUN dotnet publish API/API.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "API.dll"]
