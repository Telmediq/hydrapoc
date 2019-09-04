FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore ConsoleClient.csproj
COPY . ./
RUN dotnet publish ConsoleClient.csproj -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "ConsoleClient.dll"]