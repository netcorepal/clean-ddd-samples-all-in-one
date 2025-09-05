#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/RiskControl.Web/RiskControl.Web.csproj"
WORKDIR "/src/src/RiskControl.Web"
RUN dotnet build "RiskControl.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RiskControl.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RiskControl.Web.dll"]
