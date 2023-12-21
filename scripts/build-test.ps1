param(
    [string]$solution
)
dotnet restore $solution
dotnet build $solution
dotnet-coverage collect "dotnet test $solution --no-build" -f xml -o "coverage.xml"
dotnet-coverage report -r html -o "coverage-report"
