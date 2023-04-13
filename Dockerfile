#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Warships.API/Warships.API.csproj", "Warships.API/"]
COPY ["Warships.Game/Warships.Game.csproj", "Warships.Game/"]
COPY ["Warships.Setup/Warships.Setup.csproj", "Warships.Setup/"]
COPY ["Warships.Configuration/Warships.Configuration.csproj", "Warships.Configuration/"]
RUN dotnet restore "Warships.API/Warships.API.csproj"
COPY . .
WORKDIR "/src/Warships.API"
RUN dotnet build "Warships.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Warships.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Warships.API.dll"]