FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /app

COPY Backend/TODO_LIST.csproj ./

RUN dotnet restore

COPY Backend/. ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "TODO_LIST.dll"]
