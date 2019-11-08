FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /build

COPY ["src/DotNetKafka.Producer/DotNetKafka.Producer.csproj", "src/DotNetKafka.Producer/"]
RUN dotnet restore "src/DotNetKafka.Producer/DotNetKafka.Producer.csproj"
COPY . .
WORKDIR /build/src/DotNetKafka.Producer

FROM build AS publish
RUN dotnet publish "DotNetKafka.Producer.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS app
EXPOSE 80
WORKDIR /app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "DotNetKafka.Producer.dll"]
