# ----- BUILD -----
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY otto_api.csproj ./
RUN dotnet restore otto_api.csproj

COPY . .
RUN dotnet publish otto_api.csproj -c Release -o /app/publish /p:UseAppHost=false

# ----- RUN -----
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "otto_api.dll"]