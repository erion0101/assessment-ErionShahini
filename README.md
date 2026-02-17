# assessment-erionshahini

Video Lab – a small app for uploading, watching videos, annotations and bookmarks. Part of Internship Assessment 2026.

## What this project does

Authenticated users can:
- Register and log in
- Upload videos
- Watch videos in-app
- Create annotations (timestamp + description) on videos
- Create bookmarks (timestamp + title)
- Click on an annotation or bookmark to jump to that moment in the video

Admins can see all videos, annotations and bookmarks, create users, delete videos, and update or delete annotations/bookmarks.

## Tech stack

- **Backend:** ASP.NET Core Web API (.NET 8), EF Core, SQL Server, ASP.NET Identity, JWT
- **Frontend:** Blazor Server (.NET 7)
- **Auth:** Access token in memory (Blazor), refresh token in HttpOnly cookie and Database
- **API docs:** Swagger
- **Docker:** Optional, for API + Blazor + SQL Server

Two solutions: `assessment-erionshahini-API` (API) and `assessment-erionshahini-Layout` (Blazor UI).

## How to run it

**1. Prerequisites**
- .NET SDK 8 and .NET SDK 7+
- SQL Server (local or Docker)

**2. Configuration**

In `assessment-erionshahini-API/appsettings.json`:
- `ConnectionStrings:DefaultConnection` – SQL Server connection string
- `JwtSettings:Secret`, `Issuer`, `Audience`

In `assessment-erionshahini-Layout/appsettings.json` (or Development):
- `ApiSettings:BaseUrl` – API URL
- `ApiSettings:StreamTokenSecret` – must match `JwtSettings:Secret` in the API

Example connection string:
```
Server=localhost;Database=AssessmentErionShahiniDB;Trusted_Connection=True;TrustServerCertificate=True;
```

**3. Database**

```bash
dotnet restore
cd assessment-erionshahini-API
dotnet ef database update --context ApplicationDbContext
```

Full SQL script and seed data are in [database.md](./database.md).

**4. Run the API**

```bash
cd assessment-erionshahini-API
dotnet run
```

Swagger: `https://localhost:7294/swagger`

**5. Run the Blazor UI**

```bash
cd assessment-erionshahini-Layout
dotnet run
```

UI: `https://localhost:7039`

**6. With Docker**

From the repo root:

```bash
docker compose up -d --build
```

Wait ~30 seconds for SQL Server to accept connections, then run migrations from your machine:

**PowerShell:**
```powershell
cd assessment-erionshahini-API
$env:ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=AssessmentErionShahiniDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
dotnet ef database update
```

**Linux/macOS:**
```bash
cd assessment-erionshahini-API
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=AssessmentErionShahiniDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
dotnet ef database update
```

- Blazor UI: http://localhost:5206  
- API Swagger: http://localhost:7294/swagger  

```bash
docker compose down   # stop everything
```

## Test accounts (from seed data)

|         Email          |    Password     | Role  |
|------------------------|-----------------|------ |
| wayloadUSER@gmail.com  | wayloadUSER123. |  User |
| wayloadUSER1@gmail.com | wayloadUSER123. |  User |
| wayloadADMIN@gmail.com | wayloadADMIN123.| Admin |

## Main API endpoints

**Auth:** `POST /api/auth/Register`, `Login`, `refresh`, `logout` – `GET /api/auth/me`  
**Videos:** `POST /api/Videos/Upload`, `GET /api/Videos/GetMyVideos`, `GetById/{id}`, `Stream/{id}`, `DELETE`  
**Annotations:** `POST /api/Annotations/Create`, `GET /api/Annotations/GetByVideo/{videoId}`, `PUT /Update/{id}`, `DELETE`  
**Bookmarks:** `POST /api/Bookmarks/Create`, `GET /api/Bookmarks/GetByVideo/{videoId}`, `PUT /Update/{id}`, `DELETE`  
**Admin:** `GET /api/Admin/GetVideos`, `GetAnnotations`, `GetBookmarks`, `GetStats`, `GetRoles`, `POST CreateUser`, etc.

## Assessment requirements (checklist)

- [x] Register and login
- [x] Only authenticated users can upload/create annotations/bookmarks
- [x] Annotations with timestamp + description
- [x] Bookmarks with timestamp + title
- [x] Clicking annotation/bookmark jumps video to that timestamp
- [x] Annotations visible when video reaches timestamp (overlay)
- [x] Admin sees all videos, annotations, bookmarks
- [x] Role-based access (User, Admin)

## Assumptions and limitations

- Database: SQL Server
- No automated tests yet – manual testing only
- UI is minimal but functional
