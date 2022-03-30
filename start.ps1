$project = "Syst"

$password = "TilJuleBalINisseLand10000"

Write-Host "Starting SQL Server"
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge
$database = "EventToolDB"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password;Trusted_Connection=False;Encrypt=False"

Start-Sleep -s 40

cd ./Syst

dotnet user-secrets set "ConnectionStrings:EventToolDB" "$connectionString"

cd ../Infrastructure

dotnet user-secrets set "ConnectionStrings:EventToolDB" "$connectionString"
dotnet ef database update -s .

cd ../Syst


Write-Host "Starting App, from now on you can run with just 'dotnet run'"
dotnet run