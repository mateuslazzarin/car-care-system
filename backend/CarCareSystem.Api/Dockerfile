FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CarCareSystem.Domain/CarCareSystem.Domain.csproj", "CarCareSystem.Domain/"]
COPY ["CarCareSystem.Application/CarCareSystem.Application.csproj", "CarCareSystem.Application/"]
COPY ["CarCareSystem.Infrastructure/CarCareSystem.Infrastructure.csproj", "CarCareSystem.Infrastructure/"]
COPY ["CarCareSystem.Api/CarCareSystem.Api.csproj", "CarCareSystem.Api/"]

RUN dotnet restore "CarCareSystem.Api/CarCareSystem.Api.csproj"

COPY . .

WORKDIR "/src/CarCareSystem.Api"
RUN dotnet build "CarCareSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarCareSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarCareSystem.Api.dll"]
