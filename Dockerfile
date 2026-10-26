FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY src/DevTasker.Api/DevTasker.Api.csproj DevTasker.Api/
COPY src/DevTasker.Application/DevTasker.Application.csproj DevTasker.Application/
COPY src/DevTasker.Domain/DevTasker.Domain.csproj DevTasker.Domain/
COPY src/DevTasker.Infrastructure/DevTasker.Infrastructure.csproj DevTasker.Infrastructure/

RUN dotnet restore DevTasker.Api/DevTasker.Api.csproj

COPY src/ .

RUN dotnet publish DevTasker.Api/DevTasker.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "DevTasker.Api.dll"]