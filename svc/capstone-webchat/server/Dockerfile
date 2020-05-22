FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY *.sln .
COPY MorumOSSWebChat/*.csproj ./MorumOSSWebChat/
RUN dotnet restore

COPY MorumOSSWebChat/. ./MorumOSSWebChat/
WORKDIR /app/MorumOSSWebChat
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/MorumOSSWebChat/out ./
ENTRYPOINT ["dotnet", "MorumOSSWebChat.dll"]