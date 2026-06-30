# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy props
COPY Directory.Build.props .
COPY Directory.Packages.props .

# Copy csproj files
COPY ECE.Api/ECE.Api.csproj ECE.Api/
COPY ECE.Application/ECE.Application.csproj ECE.Application/
COPY ECE.Infrastructure/ECE.Infrastructure.csproj ECE.Infrastructure/
COPY ECE.Domain/ECE.Domain.csproj ECE.Domain/

# Restore
RUN dotnet restore ECE.Api/ECE.Api.csproj

# Copy everything
COPY . .

# Publish
RUN dotnet publish ECE.Api/ECE.Api.csproj -c Release -o /app/publish

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Timezone
ENV TZ=Africa/Cairo

WORKDIR /app

COPY --from=build /app/publish .
COPY docker-entrypoint.sh /docker-entrypoint.sh

RUN chmod +x /docker-entrypoint.sh


ENTRYPOINT ["/docker-entrypoint.sh"]

