FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NghiaSoft.FileServerAPI/NghiaSoft.FileServerAPI.csproj", "NghiaSoft.FileServerAPI/"]
RUN dotnet restore "NghiaSoft.FileServerAPI/NghiaSoft.FileServerAPI.csproj"
COPY . .
WORKDIR "/src/NghiaSoft.FileServerAPI"
RUN dotnet build "NghiaSoft.FileServerAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NghiaSoft.FileServerAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NghiaSoft.FileServerAPI.dll"]
