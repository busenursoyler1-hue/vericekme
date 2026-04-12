# .NET 8 veya 9 kullanıyorsan buradaki 10.0 yazılarını 8.0 veya 9.0 yapabilirsin
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Proje dosyasını kopyala (Klasör yapına göre burayı düzenledim)
COPY ["vericekme.csproj", "."]
RUN dotnet restore "./vericekme.csproj"

# Geri kalan her şeyi kopyala
COPY . .
RUN dotnet build "vericekme.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "vericekme.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vericekme.dll"]