FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Sample.Api/Sample.Api.csproj", "Sample.Api/"]
COPY ["Sample.Components/Sample.Components.csproj", "Sample.Components/"]
RUN dotnet restore "Sample.Api/Sample.Api.csproj"

COPY . .
RUN dotnet publish -c Release -o /app Sample.Api/Sample.Api.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Sample.Api.dll"]
