#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sport_and_joy_back_dotnet/sport_and_joy_back_dotnet.csproj", "sport_and_joy_back_dotnet/"]
RUN dotnet restore "sport_and_joy_back_dotnet/sport_and_joy_back_dotnet.csproj"
COPY . .
WORKDIR "/src/sport_and_joy_back_dotnet"
RUN dotnet build "sport_and_joy_back_dotnet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "sport_and_joy_back_dotnet.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sport_and_joy_back_dotnet.dll"]