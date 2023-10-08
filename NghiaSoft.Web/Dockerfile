FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NghiaSoft.Web/NghiaSoft.Web.csproj", "NghiaSoft.Web/"]
RUN dotnet restore "NghiaSoft.Web/NghiaSoft.Web.csproj"
COPY . .
WORKDIR "/src/NghiaSoft.Web"
RUN dotnet build "NghiaSoft.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NghiaSoft.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NghiaSoft.Web.dll"]
