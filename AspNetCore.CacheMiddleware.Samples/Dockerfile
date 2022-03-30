#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["AspNetCore.CacheMiddleware.Samples/AspNetCore.CacheMiddleware.Samples.csproj", "AspNetCore.CacheMiddleware.Samples/"]
RUN dotnet restore "AspNetCore.CacheMiddleware.Samples/AspNetCore.CacheMiddleware.Samples.csproj"
COPY . .
WORKDIR "/src/AspNetCore.CacheMiddleware.Samples"
RUN dotnet build "AspNetCore.CacheMiddleware.Samples.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetCore.CacheMiddleware.Samples.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetCore.CacheMiddleware.Samples.dll"]