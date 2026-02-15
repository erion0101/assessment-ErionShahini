# assessment-erionshahini

Internship Assessment 2026 - Video Lab (API + Blazor Server UI).

This project allows authenticated users to upload/watch videos, create annotations and bookmarks with timestamps, and use an admin view for global data.

## Current status

- [x] Authentication: Register, Login, Logout, Refresh token, `GET /api/auth/me`
- [x] Authorization: JWT + role-based access (`User`, `Admin`)
- [x] Video flow: Upload, My Videos, Watch (stream + seek support)
- [x] Annotations: create/list/delete by video and user
- [x] Bookmarks: create/list/delete by video and user
- [x] Bookmark click seek to timestamp (frontend)
- [x] Annotation playback visibility/highlight while video plays (frontend)
- [x] Admin API: all videos/annotations/bookmarks
- [x] Admin UI page connected to API (counts + lists)

## Tech stack

- Backend: ASP.NET Core Web API (.NET 8), EF Core, SQL Server, ASP.NET Identity, JWT
- Frontend: Blazor Server (.NET 7)
- Docs/testing: Swagger
- Optional: Docker (API and SQL Server)

## How to run

### 1) Prerequisites

- .NET SDK installed (8 for API, 7+ for Blazor project)
- SQL Server (local or Docker)

### 2) Configure database connection

In `assessment-erionshahini-API/appsettings.json`, set:

- `ConnectionStrings:DefaultConnection`
- `JwtSettings:Secret`, `JwtSettings:Issuer`, `JwtSettings:Audience`

Example local connection:

`Server=localhost;Database=AssessmentErionShahiniDB;Trusted_Connection=True;TrustServerCertificate=True;`

### 3) Restore and migrate

```bash
dotnet restore
cd assessment-erionshahini-API
dotnet ef database update --context ApplicationDbContext
```

### 4) Run API

```bash
cd assessment-erionshahini-API
dotnet run
```

Swagger available at:

- `https://localhost:7294/swagger` (port may vary)

### 5) Run Blazor UI

In another terminal:

```bash
cd assessment-erionshahini-Layout
dotnet run
```

Open the UI URL shown in terminal (typically `https://localhost:7039`).

## Main API endpoints

- Auth:
  - `POST /api/auth/Register`
  - `POST /api/auth/Login`
  - `POST /api/auth/refresh`
  - `POST /api/auth/logout`
  - `GET /api/auth/me`
- Videos:
  - `POST /api/Videos/Upload`
  - `GET /api/Videos/GetMyVideos`
  - `GET /api/Videos/GetById/{id}`
  - `GET /api/Videos/Stream/{id}`
- Annotations:
  - `POST /api/Annotations/Create`
  - `GET /api/Annotations/GetByVideo/{videoId}`
  - `DELETE /api/Annotations/Delete/{id}`
- Bookmarks:
  - `POST /api/Bookmarks/Create`
  - `GET /api/Bookmarks/GetByVideo/{videoId}`
  - `DELETE /api/Bookmarks/Delete/{id}`
- Admin (Admin role):
  - `GET /api/Admin/GetVideos`
  - `GET /api/Admin/GetAnnotations`
  - `GET /api/Admin/GetBookmarks`

## Assumptions and limitations

- Database engine is SQL Server.
- Role creation/management is API-based; register defaults to `User` role if role id is not provided.
- Stream endpoint is currently anonymous at API level; Blazor app uses a short-lived proxy token for in-app watch route.
- UI language is mixed Albanian/English in some labels.

## Suggested next improvements

- Add pagination/filter/search in admin lists for large datasets.
- Add stronger server-side video MIME validation and optional antivirus scanning.
- Add automated tests (unit/integration) for auth and notes flows.
