FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./Converter.Core/Converter.Core.csproj", "src/Converter.Core/"]
COPY ["./Converter/Converter.Main.csproj", "src/Converter.Main/"]

RUN dotnet restore "src/Converter.Main/Converter.Main.csproj"

WORKDIR "/src/Converter.Main/"

COPY . .

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT [ "dotnet", "Converter.Main.dll" ]