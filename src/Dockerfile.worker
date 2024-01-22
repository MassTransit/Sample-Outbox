FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Sample.Worker/Sample.Worker.csproj", "Sample.Worker/"]
COPY ["Sample.Components/Sample.Components.csproj", "Sample.Components/"]
RUN dotnet restore "Sample.Worker/Sample.Worker.csproj"

COPY . .
RUN dotnet publish -c Release -o /app Sample.Worker/Sample.Worker.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Sample.Worker.dll"]
