# using a multi-stage build process
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-image

WORKDIR /App

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet build -o out

# use Release config
RUN dotnet publish -c Release -o /publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime-image
WORKDIR /App/out
COPY --from=build-image /publish .
EXPOSE 8080
ENTRYPOINT [ "dotnet", "TaskApi.dll" ]