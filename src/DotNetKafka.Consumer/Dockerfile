FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /build
COPY ["src/DotNetKafka.Consumer/DotNetKafka.Consumer.csproj", "src/DotNetKafka.Consumer/"]
RUN dotnet restore "src/DotNetKafka.Consumer/DotNetKafka.Consumer.csproj"
COPY . .
WORKDIR /build/src/DotNetKafka.Consumer

FROM build AS publish
RUN dotnet publish "DotNetKafka.Consumer.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS app
RUN sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = DEFAULT@SECLEVEL=1/' /etc/ssl/openssl.cnf
WORKDIR /app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "DotNetKafka.Consumer.dll"]
