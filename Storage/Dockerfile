FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./Storage.Core/Storage.Core.csproj", "src/Storage.Core/"]
COPY ["./Storage.Database/Storage.Database.csproj", "src/Storage.Database/"]
COPY ["./Storage/Storage.Main.csproj", "src/Storage.Main/"]

RUN dotnet restore "src/Storage.Main/Storage.Main.csproj"

WORKDIR "/src/Storage.Main/"

COPY . .

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT [ "dotnet", "Storage.Main.dll" ]