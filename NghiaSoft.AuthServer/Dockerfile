FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NghiaSoft.AuthServer/NghiaSoft.AuthServer.csproj", "NghiaSoft.AuthServer/"]
RUN dotnet restore "NghiaSoft.AuthServer/NghiaSoft.AuthServer.csproj"
COPY . .
WORKDIR "/src/NghiaSoft.AuthServer"
RUN dotnet build "NghiaSoft.AuthServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NghiaSoft.AuthServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NghiaSoft.AuthServer.dll"]
