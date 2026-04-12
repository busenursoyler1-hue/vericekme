#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
## Dosyayı bulması için klasör yolunu açıkça belirtiyoruz
#COPY ["vericekme/vericekme.csproj", "vericekme/"]
#RUN dotnet restore "vericekme/vericekme.csproj"
#
#COPY . .
#WORKDIR "/src/vericekme"
#RUN dotnet build "vericekme.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "vericekme.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "vericekme.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# --- BAŞLANGIÇ: CHROME KURULUMU (Bozmadan Eklendi) ---
RUN apt-get update && apt-get install -y wget gnupg
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list'
RUN apt-get update && apt-get install -y google-chrome-stable --no-install-recommends
# --- BİTİŞ: CHROME KURULUMU ---

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Klasör yolunu belirttiğin orijinal satırların:
COPY ["vericekme/vericekme.csproj", "vericekme/"]
RUN dotnet restore "vericekme/vericekme.csproj"

COPY . .
WORKDIR "/src/vericekme"
RUN dotnet build "vericekme.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "vericekme.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vericekme.dll"]