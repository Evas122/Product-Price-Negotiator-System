FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app
EXPOSE 5268

COPY PriceNegotiator.sln . 
COPY src/PriceNegotiator.Api/PriceNegotiator.Api.csproj src/PriceNegotiator.Api/
COPY src/PriceNegotiator.Domain/PriceNegotiator.Domain.csproj src/PriceNegotiator.Domain/
COPY src/PriceNegotiator.Infrastructure/PriceNegotiator.Infrastructure.csproj src/PriceNegotiator.Infrastructure/
COPY src/PriceNegotiator.Application/PriceNegotiator.Application.csproj src/PriceNegotiator.Application/
COPY tests/PriceNegotiator.Application.UnitTests/PriceNegotiator.Application.UnitTests.csproj ./tests/PriceNegotiator.Application.UnitTests/
COPY tests/PriceNegotiator.Infrastructure.UnitTests/PriceNegotiator.Infrastructure.UnitTests.csproj ./tests/PriceNegotiator.Infrastructure.UnitTests/

RUN dotnet restore

COPY . .
WORKDIR /app/src/PriceNegotiator.Api
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/src/PriceNegotiator.Api/out ./ 

ENTRYPOINT ["sh", "-c", "sleep 10 && dotnet PriceNegotiator.Api.dll"]