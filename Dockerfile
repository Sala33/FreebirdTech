#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443
ENV ASPNETCORE_Environment=Production
#ENV ASPNETCORE_URLS http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FreebirdTech.csproj", "."]
RUN dotnet restore "./FreebirdTech.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FreebirdTech.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FreebirdTech.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FreebirdTech.dll"]