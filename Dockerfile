# 使用官方的 ASP.NET Core 運行時映像作為基礎映像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000
# 設置環境變數
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

# 使用 SDK 映像來構建專案
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApiWithRoleAuthentication.csproj", "./"]
RUN dotnet restore "WebApiWithRoleAuthentication.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebApiWithRoleAuthentication.csproj" -c Release -o /app/build

# 發布專案
FROM build AS publish
RUN dotnet publish "WebApiWithRoleAuthentication.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 最終映像
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh
ENTRYPOINT ["dotnet", "WebApiWithRoleAuthentication.dll"]