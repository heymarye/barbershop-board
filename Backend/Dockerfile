#Define base image
FROM mcr.microsoft.com/dotnet/sdk:6.0.201-alpine3.15 AS builder

#Copy project files
WORKDIR /source
COPY ./*.csproj .

#Restore
RUN dotnet restore

#Copy all source code
COPY . .

#Publish
WORKDIR /source
RUN dotnet publish "Backend.csproj" -c Release -o /publish

#Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0.3-alpine3.15
WORKDIR /app
COPY --from=builder /publish .

EXPOSE 80

RUN addgroup -S runner && adduser -S runner -G runner
USER runner
ENTRYPOINT ["dotnet", "BackEnd.dll"]
