# assessment-erionshahini

Internship Assessment 2026 - Video Lab application (ASP.NET Core API + Blazor Server UI).

## Overview

This project allows authenticated users to:

- Register and sign in
- Upload videos
- Watch videos inside the application
- Create annotations (timestamp + description)
- Create bookmarks (timestamp + title)
- Jump to saved timestamps from annotations/bookmarks

Admin users can access global overview pages for videos, annotations, and bookmarks.

## Architecture

- `assessment-erionshahini-API`: .NET 8 Web API, EF Core, SQL Server, Identity, JWT
- `assessment-erionshahini-Layout`: Blazor Server (.NET 7) UI
- Auth model:
  - Access token kept in memory on the Blazor side
  - Refresh token kept in DB and managed via HttpOnly cookie on API side
- Video streaming model:
  - Blazor serves `/stream/{id}?t=...&st=...` as a same-origin proxy (cache token `t` + signed stream JWT `st`)
  - Proxy forwards the stream JWT as `Authorization: Bearer` to the API and forwards `Range` requests for seek support
  - API `GET /api/Videos/Stream/{id}` accepts only (1) a valid stream token (JWT with claim `vid`, signed with same secret as API) or (2) an authenticated user. Direct call without token or user returns 401.
  - `GET /api/Videos/GetById/{id}` requires authentication.

## Tech Stack

- Backend: ASP.NET Core Web API (.NET 8)
- Frontend: Blazor Server (.NET 7)
- Database: SQL Server + EF Core
- Auth: ASP.NET Identity + JWT
- API docs: Swagger/OpenAPI
- Optional infrastructure: Docker

## Requirement Mapping (Assessment Checklist)

- [x] User account creation and login
- [x] Only authenticated users can upload/add annotations/add bookmarks
- [x] Annotations include timestamp + description
- [x] Bookmarks include timestamp + title
- [x] Clicking bookmark/annotation seeks to saved timestamp
- [x] Annotation visibility while playback reaches related timestamp (overlay on video for 5s at matching time; Plyr player with markers on progress bar)
- [x] Admin view for all videos/annotations/bookmarks
- [x] Role-based access (`User`, `Admin`)

## How to Run

### 1) Prerequisites

- .NET SDK 8 (API) and .NET SDK 7+ (Blazor project)
- SQL Server instance (local or Docker)

### 2) Configuration

In `assessment-erionshahini-API/appsettings.json`, configure:

- `ConnectionStrings:DefaultConnection`
- `JwtSettings:Secret`
- `JwtSettings:Issuer`
- `JwtSettings:Audience`
- Optional CORS origins if needed

In `assessment-erionshahini-Layout/appsettings.json` or `appsettings.Development.json`, set:

- `ApiSettings:BaseUrl` (API base URL)
- `ApiSettings:StreamTokenSecret` — **must match** the API’s `JwtSettings:Secret` (used to sign stream JWTs so the API can validate them)

Example connection string:

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

Swagger: `https://localhost:7294/swagger` (port may vary)

### 5) Run Blazor UI

```bash
cd assessment-erionshahini-Layout
dotnet run
```

UI URL is typically `https://localhost:7039`.

### 6) Run with Docker (API + Blazor + SQL Server)

**Prerequisites:** Docker and Docker Compose installed.

From the **repository root**:

```bash
# Optional: copy .env.example to .env and set MSSQL_SA_PASSWORD / JWT_SECRET
docker compose up -d --build
```

Wait ~30 seconds for SQL Server to accept connections, then apply migrations **from your machine** (one time).

**PowerShell (Windows):**
```powershell
cd assessment-erionshahini-API
$env:ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=AssessmentErionShahiniDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
dotnet ef database update
```

**CMD (Windows):**
```cmd
cd assessment-erionshahini-API
set ConnectionStrings__DefaultConnection=Server=localhost,1433;Database=AssessmentErionShahiniDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True
dotnet ef database update
```

**Linux/macOS:**
```bash
cd assessment-erionshahini-API
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=AssessmentErionShahiniDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
dotnet ef database update
```

If the API container exits with a DB connection error, restart it after SQL is up:

```bash
docker compose restart api
```

- **Blazor UI:** http://localhost:5206  
- **API Swagger:** http://localhost:7294/swagger  
- **SQL Server:** localhost:1433 (user `sa`, password from `MSSQL_SA_PASSWORD` or default `YourStrong@Passw0rd`)

Stop everything:

```bash
docker compose down
```

## Main Endpoints

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
- `GET /api/Videos/Stream/{id}` (served via short-lived Blazor proxy token in-app)
  - `DELETE /api/Videos/{id}` (owner/admin)
- Annotations:
  - `POST /api/Annotations/Create`
  - `GET /api/Annotations/GetByVideo/{videoId}`
  - `DELETE /api/Annotations/Delete/{id}`
- Bookmarks:
  - `POST /api/Bookmarks/Create`
  - `GET /api/Bookmarks/GetByVideo/{videoId}`
  - `DELETE /api/Bookmarks/Delete/{id}`
- Admin:
  - `GET /api/Admin/GetVideos`
  - `GET /api/Admin/GetAnnotations`
  - `GET /api/Admin/GetBookmarks`

## Manual Test Plan

1. Register and login as a user.
2. Upload a video and open it in `/user/watch/{id}`.
3. Add one annotation and one bookmark using `Now`.
4. Click saved timestamps and verify seek works.
5. Confirm annotation overlay appears during playback at the saved timestamp.
6. Logout and verify protected pages redirect/deny access.
7. Login as admin and verify admin pages list all videos/annotations/bookmarks.
8. Try direct API stream URL without auth: should return **401** (API accepts only valid stream token or authenticated user).

## Assumptions, Shortcuts, and Limitations

- Database engine is SQL Server.
- Role creation/management is API-based (register defaults to `User` when role not specified).
- Blazor Server + cookie auth requires careful browser/server flow handling; current implementation follows project constraints.
- No full automated test suite included yet (manual validation checklist provided).
- UI is intentionally lightweight but focused on assessment requirements and behavior clarity.

## Development Process (No History Rewrite)

- Existing commit history is preserved as-is (no rewrite/amend of old public history).
- From this stage onward, changes are organized in small, meaningful commits (security, UX, docs).
- Final phase focused on:
  - stream authorization hardening,
  - watch UX improvements (annotation overlay),
  - README accuracy and assessment clarity.

## Demo Assets (optional)

Before submission you can add 4–6 screenshots or short GIFs, for example:

- Login / Register
- Upload
- Watch page with seek and annotation overlay
- Annotation active at timestamp
- Admin view (videos / annotations / bookmarks)

Place them in a `docs/screenshots` folder or link them in this section.

## Final Submission Checklist

A one-page **copy-paste checklist** is in [SUBMISSION_CHECKLIST.md](./SUBMISSION_CHECKLIST.md). Use it before sending the repo link to verify security, functionality, README, and commits.

## Suggested Next Improvements

- Add edit/update support for annotations and bookmarks.
- Add automated tests (service + controller integration tests).
- Add rate limiting and stronger input/file validation.
- Add CI workflow for build/lint/tests on push.


