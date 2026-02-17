# Run this AFTER "docker compose up -d" and waiting ~30 sec for SQL Server.
# Creates the AssessmentErionShahiniDB database and tables in the Docker SQL Server.

$conn = "Server=localhost,1433;Database=AssessmentErionShahiniDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true"
$env:ConnectionStrings__DefaultConnection = $conn

Set-Location $PSScriptRoot\assessment-erionshahini-API
dotnet ef database update

if ($LASTEXITCODE -eq 0) {
    Write-Host "Done. Database ready. Open http://localhost:5206 and http://localhost:7294/swagger" -ForegroundColor Green
} else {
    Write-Host "Migration failed. Is SQL Server running? Run: docker compose ps" -ForegroundColor Red
}
