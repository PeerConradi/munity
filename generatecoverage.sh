rm -r ../MunityCoverageReport
rm -r tests/MUNity.BlazorServer.Tests/TestResults
rm -r tests/MUNityBaseTest/TestResults
rm -r tests/MUNityDatabaseTest/TestResults
rm -r tests/MUNitySchemaTests/TestResults
rm -r tests/MUNityServicesTest/TestResults

dotnet test MUNityCore.sln --collect:"XPlat Code Coverage"
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:tests/**/coverage.cobertura.xml -targetdir:../MunityCoverageReport -reporttypes:Html