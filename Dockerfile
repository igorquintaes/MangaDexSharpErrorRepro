FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-amd64 AS base
WORKDIR /app
RUN apk add --no-cache dotnet6-runtime

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ConsoleApp12/ConsoleApp12.csproj ./ConsoleApp12/
RUN dotnet restore "./ConsoleApp12/ConsoleApp12.csproj"
COPY . .
WORKDIR /src/ConsoleApp12

RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

From base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp12.dll"]
