# assessment-erionshahini (Backend)

API for the video application (assessment – Internship 2026): auth, roles, video/annotations/bookmarks. README is updated with each commit.

---

## Project status

- [x] Auth: Register, Login, Logout, Refresh token, GET /me
- [x] Roles: Identity roles (User, Admin), CRUD for roles
- [x] DB: EF Core, SQL Server, migrations in `Repository/Migrations`
- [ ] Video: upload, list (planned)
- [ ] Annotations / Bookmarks (planned)
- [ ] Admin: endpoint for all videos/annotations/bookmarks (planned)

---

## How to run

**Requirements:** .NET 8 SDK, SQL Server (local or Docker), optional Docker for the API.

1. **Clone and restore**
   ```bash
   git clone <repo-url>
   cd assessment-erionshahini-API
   dotnet restore
   ```

2. **Connection string**  
   In `assessment-erionshahini-API/appsettings.json` set `ConnectionStrings:DefaultConnection` for your SQL Server.
   - Local: `Server=localhost;Database=AssessmentErionShahiniDB;Trusted_Connection=True;TrustServerCertificate=True;`
   - SQL Server in Docker:
     ```bash
     docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Pass" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
     ```
     Connection string: `Server=localhost,1433;Database=AssessmentErionShahiniDB;User Id=sa;Password=YourStrong@Pass;TrustServerCertificate=True;`

3. **Migrations**
   ```bash
   cd assessment-erionshahini-API
   dotnet ef database update --context ApplicationDbContext
   ```

4. **Run**
   ```bash
   cd assessment-erionshahini-API
   dotnet run
   ```
   Swagger: `https://localhost:7294/swagger` (port may vary).

5. **Docker (API)**  
   The database must be reachable from the container (e.g. SQL Server on the same network).
   ```bash
   docker build -t assessment-api -f assessment-erionshahini-API/Dockerfile .
   docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="<connection-string>" -e JwtSettings__Secret="<secret>" assessment-api
   ```

---

## How to use

- **Swagger:** `/swagger` – Use Authorize with JWT (token only, no "Bearer " prefix).
- **Auth**
  - `POST /api/auth/Register` – body: `{ "email", "password", "roleId" }`
  - `POST /api/auth/Login` – body: `{ "email", "password" }` → accessToken in body, refreshToken in cookie
  - `POST /api/auth/refresh` – returns new access token (uses refresh token from cookie)
  - `POST /api/auth/logout` – header `Authorization: Bearer <token>`
  - `GET /api/auth/me` – returns `{ "id", "email", "roles" }`
- **Roles:** first call `POST /api/Roles/CreateRole` with body `"User"` or `"Admin"`, then use the returned `roleId` in Register.

---

## Technology

- **Language:** C#
- **Framework:** .NET 8, ASP.NET Core Web API
- **DB:** Entity Framework Core 8, SQL Server (Docker or local)
- **Auth:** ASP.NET Core Identity (User/Role, Guid), JWT Bearer, refresh token in HttpOnly cookie
- **API docs:** Swagger (Swashbuckle)
- **Docker:** for API and/or SQL Server
- **Structure:** Controller → Service → Repository; Entities/dbContext/Migrations under `Repository/`

---

## Assumptions and limitations

- DB: SQL Server only (connection string in appsettings).
- Auth and roles are done; video/annotations/bookmarks and admin view will be added in later commits.
- Docker is used to run the API and the database (SQL Server) when working with containers.

---

*Assessment – Erion Shahini | Internship 2026*
