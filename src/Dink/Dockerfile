# Stage 1
FROM mcr.microsoft.com/dotnet/core-nightly/sdk:3.1.101 as build
WORKDIR /app
COPY src/Dink src/Dink
COPY native native

RUN dotnet clean src/Dink
RUN dotnet build src/Dink

# Stage 2
FROM build as publish
WORKDIR /app
RUN dotnet publish src/Dink -c Release -o /app -r linux-x64

# Stage 3
FROM mcr.microsoft.com/dotnet/core-nightly/runtime:3.1 AS runtime
RUN apt-get update
RUN apt-get install xvfb libfontconfig wkhtmltopdf -y
WORKDIR /app
COPY --from=publish /app .
CMD dotnet Dink.dll
EXPOSE 80