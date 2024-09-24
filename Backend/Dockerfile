FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY TODO_LIST.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "TODO_LIST.dll"]
