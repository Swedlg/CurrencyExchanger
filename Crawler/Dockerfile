FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./Crawler.Core/Crawler.Core.csproj", "src/Crawler.Core/"]
COPY ["./Crawler.Database/Crawler.Database.csproj", "src/Crawler.Database/"]
COPY ["./Crawler/Crawler.Main.csproj", "src/Crawler.Main/"]

RUN dotnet restore "src/Crawler.Main/Crawler.Main.csproj"

WORKDIR "/src/Crawler.Main/"

COPY . .

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

RUN ls -l

ENTRYPOINT [ "dotnet", "Crawler.Main.dll" ]