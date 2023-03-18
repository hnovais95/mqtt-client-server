FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore Server/Server.csproj 
RUN dotnet restore Common/Common.csproj
RUN dotnet restore Mqtt/Mqtt.csproj
RUN dotnet restore USBDriver/USBDriver.csproj

RUN dotnet build --configuration Release --no-restore ./Server/Server.csproj
RUN dotnet build --configuration Release --no-restore ./Common/Common.csproj
RUN dotnet build --configuration Release --no-restore ./Mqtt/Mqtt.csproj
RUN dotnet build --configuration Release --no-restore ./USBDriver/USBDriver.csproj

RUN dotnet publish Server/Server.csproj --configuration Release --no-build --output /app/Server/out
RUN dotnet publish Common/Common.csproj --configuration Release --no-build --output /app/Common/out
RUN dotnet publish Mqtt/Mqtt.csproj --configuration Release --no-build --output /app/Mqtt/out
RUN dotnet publish USBDriver/USBDriver.csproj --configuration Release --no-build --output /app/USBDriver/out

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app

COPY --from=build /app/Server/out ./Server
COPY --from=build /app/Common/out ./Common
COPY --from=build /app/Mqtt/out ./Mqtt
COPY --from=build /app/USBDriver/out ./USBDriver

WORKDIR /app/Server
ENTRYPOINT ["dotnet", "Server.dll"]
