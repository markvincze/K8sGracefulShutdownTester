FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

COPY . .
RUN dotnet restore

WORKDIR /app/src/K8sGracefulShutdownTester
RUN dotnet publish -c Release -o out
COPY ./preStop.sh ./out

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/src/K8sGracefulShutdownTester/out ./
ENTRYPOINT ["dotnet", "K8sGracefulShutdownTester.dll"]