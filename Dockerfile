# 1. Build stage: compile the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set working directory
WORKDIR /src

# Copy solution + project files
COPY *.sln .
COPY *.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy all remaining source code
COPY . .

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# 2. Runtime stage: run the compiled app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# Set working directory
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Expose default port (change if your app uses a different port)
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "Librarymanagementsystem.dll"]
