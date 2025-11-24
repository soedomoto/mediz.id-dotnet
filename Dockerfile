FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MedizID.API/MedizID.API.csproj", "MedizID.API/"]
RUN dotnet restore "MedizID.API/MedizID.API.csproj"

COPY . .
WORKDIR "/src/MedizID.API"
RUN dotnet build "MedizID.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MedizID.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Load environment variables from .env if it exists in the image
# Otherwise, they can be provided at runtime
ENTRYPOINT ["dotnet", "MedizID.API.dll"]
