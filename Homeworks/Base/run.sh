#!/bin/sh

SOLUTION_FILE=src/PromoCodeFactory.sln
MAIN_PROJECT=src/PromoCodeFactory.WebHost/PromoCodeFactory.WebHost.csproj

dotnet format -v d "${SOLUTION_FILE}"

dotnet clean "${SOLUTION_FILE}" && dotnet build "${SOLUTION_FILE}"

dotnet run --project "${MAIN_PROJECT}"
