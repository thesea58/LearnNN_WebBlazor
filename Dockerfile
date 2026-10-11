# ==============================================================================
# Multi-stage Dockerfile for LearnNN_WebBlazor (.NET 10)
# ==============================================================================

# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies (layer cached)
COPY ["Source/LearnNN_WebBlazor.csproj", "Source/"]
RUN dotnet restore "Source/LearnNN_WebBlazor.csproj"

# Copy full source and publish Release
COPY Source/ Source/
WORKDIR "/src/Source"
RUN dotnet publish "LearnNN_WebBlazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Production Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Expose Kestrel port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

# Persistent volume directory for SQLite database
RUN mkdir -p /app/data
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/LearnNN_VocabDB.db"

# Copy published binaries from build stage
COPY --from=build /app/publish .

# Application entry point
ENTRYPOINT ["dotnet", "LearnNN_WebBlazor.dll"]
