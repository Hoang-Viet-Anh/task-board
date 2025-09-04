FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY TaskBoard/*.csproj ./TaskBoard/
COPY TaskBoard.Application/*.csproj ./TaskBoard.Application/
COPY TaskBoard.Domain/*.csproj ./TaskBoard.Domain/
COPY TaskBoard.Infrastructure/*.csproj ./TaskBoard.Infrastructure/

RUN dotnet restore ./TaskBoard/TaskBoard.csproj

# Copy everything else
COPY . .

RUN dotnet publish ./TaskBoard/TaskBoard.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "TaskBoard.dll"] 

