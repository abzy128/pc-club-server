FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln .
COPY pc-club-server/*.csproj ./pc-club-server/
RUN dotnet restore

COPY pc-club-server/. ./pc-club-server/
WORKDIR /source/pc-club-server
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "pc-club-server.dll"]