# Database Script – AssessmentErionShahiniDB

Ky skedar përmban skriptin SQL për krijimin e bazës së të dhënave dhe të dhënat fillestare.

**Shënime:**
- Shtigjet e skedarëve (`FILENAME`) janë specifike për makinën aktuale. Në një makinë tjetër, ndryshoni shtigjet në `CREATE DATABASE` (ose përdorni `dotnet ef database update`).
- Përdoruesit test: Admin (`wayloadADMIN@gmail.com`), User1 (`wayloadUSER1@gmail.com`), User2 (`wayloadUSER@gmail.com`). Fjalëkalimet duhet dokumentuar në README.

---

```sql
USE [master]
GO
/****** Object:  Database [AssessmentErionShahiniDB]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE DATABASE [AssessmentErionShahiniDB]
 CONTAINMENT = NONE
 ON  PRIMARY
( NAME = N'AssessmentErionShahiniDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AssessmentErionShahiniDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON
( NAME = N'AssessmentErionShahiniDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AssessmentErionShahiniDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AssessmentErionShahiniDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET ANSI_NULLS OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET ANSI_PADDING OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET ARITHABORT OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET AUTO_CLOSE ON
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET  ENABLE_BROKER
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET RECOVERY SIMPLE
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET  MULTI_USER
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET DB_CHAINING OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF )
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET TARGET_RECOVERY_TIME = 60 SECONDS
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET DELAYED_DURABILITY = DISABLED
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET ACCELERATED_DATABASE_RECOVERY = OFF
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [AssessmentErionShahiniDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Annotations]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Annotations](
	[Id] [uniqueidentifier] NOT NULL,
	[VideoId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[TimestampSeconds] [float] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Annotations] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[RefreshTokenExpiry] [datetime2](7) NULL,
	[RefreshTokenIp] [nvarchar](50) NULL,
	[RefreshTokenUserAgent] [nvarchar](500) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [uniqueidentifier] NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bookmarks]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bookmarks](
	[Id] [uniqueidentifier] NOT NULL,
	[VideoId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[TimestampSeconds] [float] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Bookmarks] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Videos]    Script Date: 2/16/2026 6:10:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Videos](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[ContentType] [nvarchar](max) NULL,
	[FileSizeBytes] [bigint] NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UploadedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Videos] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260213170752_InitialCreate', N'8.0.0')
GO
INSERT [dbo].[Annotations] ([Id], [VideoId], [UserId], [TimestampSeconds], [Description], [CreatedAt]) VALUES (N'7155daa2-5de5-47a6-8883-06fc1f63f5f6', N'919f2311-06c2-4639-aa40-72c0f16c285b', N'ba8eea65-a726-4216-aeeb-ce16b120bf8f', 2, N'Entry', CAST(N'2026-02-15T18:13:13.6281218' AS DateTime2))
INSERT [dbo].[Annotations] ([Id], [VideoId], [UserId], [TimestampSeconds], [Description], [CreatedAt]) VALUES (N'95062a3c-3619-4ef0-9f01-0d38a6e56420', N'919f2311-06c2-4639-aa40-72c0f16c285b', N'ba8eea65-a726-4216-aeeb-ce16b120bf8f', 6.5, N'End', CAST(N'2026-02-15T18:14:15.7913817' AS DateTime2))
INSERT [dbo].[Annotations] ([Id], [VideoId], [UserId], [TimestampSeconds], [Description], [CreatedAt]) VALUES (N'b1cd4e20-76d5-4148-a016-434fab2acd9b', N'ea50c766-d110-4836-bb7a-771dd500343d', N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', 7, N'Perfundimi', CAST(N'2026-02-15T00:57:26.0327321' AS DateTime2))
INSERT [dbo].[Annotations] ([Id], [VideoId], [UserId], [TimestampSeconds], [Description], [CreatedAt]) VALUES (N'1771d8f6-a542-4631-abb7-5df54363c60a', N'ea50c766-d110-4836-bb7a-771dd500343d', N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', 5, N'Permbajtja', CAST(N'2026-02-15T00:57:13.5678643' AS DateTime2))
INSERT [dbo].[Annotations] ([Id], [VideoId], [UserId], [TimestampSeconds], [Description], [CreatedAt]) VALUES (N'1cd1f191-6d25-4657-967e-d2fbda290f88', N'ea50c766-d110-4836-bb7a-771dd500343d', N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', 2, N'Hyrje', CAST(N'2026-02-15T00:57:00.8141397' AS DateTime2))
INSERT [dbo].[Annotations] ([Id], [VideoId], [UserId], [TimestampSeconds], [Description], [CreatedAt]) VALUES (N'7d814b9b-09b4-4247-b8c3-da35575a0df0', N'919f2311-06c2-4639-aa40-72c0f16c285b', N'ba8eea65-a726-4216-aeeb-ce16b120bf8f', 3, N'Content', CAST(N'2026-02-15T18:13:49.7283672' AS DateTime2))
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'fee7860e-0fd1-4cc1-888b-446bb85005f0', N'User', N'USER', N'5b802b65-e46d-4d6b-8f17-7f3ad19bfcd9')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3c228a67-1885-411a-8339-939f8fdfba56', N'Admin', N'ADMIN', N'29f0580b-d942-4dcb-82be-cd6f0e5b319f')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'ba8eea65-a726-4216-aeeb-ce16b120bf8f', N'fee7860e-0fd1-4cc1-888b-446bb85005f0')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', N'fee7860e-0fd1-4cc1-888b-446bb85005f0')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'd7130f5f-7c65-4aad-ae4b-c3caf72179ac', N'3c228a67-1885-411a-8339-939f8fdfba56')
GO
INSERT [dbo].[AspNetUsers] ([Id], [CreatedAt], [IsActive], [UpdatedAt], [RefreshToken], [RefreshTokenExpiry], [RefreshTokenIp], [RefreshTokenUserAgent], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'd7130f5f-7c65-4aad-ae4b-c3caf72179ac', CAST(N'2026-02-14T18:26:09.7310653' AS DateTime2), 1, NULL, NULL, NULL, NULL, NULL, N'wayloadADMIN@gmail.com', N'WAYLOADADMIN@GMAIL.COM', N'wayloadADMIN@gmail.com', N'WAYLOADADMIN@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEGA+UQ+da2yiJkFk0I3tFq/ajLXbmfRHExc/jj2u9UhrhPZZqGR2kpF2T4ZqIgnpDA==', N'GNNMUSOMWUNA7XXA47ONWFEWGR7DLIEM', N'3cf5b4f0-43b4-496f-b7e8-1a93ca4b5ebe', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [CreatedAt], [IsActive], [UpdatedAt], [RefreshToken], [RefreshTokenExpiry], [RefreshTokenIp], [RefreshTokenUserAgent], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'ba8eea65-a726-4216-aeeb-ce16b120bf8f', CAST(N'2026-02-15T18:11:37.6689895' AS DateTime2), 1, NULL, NULL, NULL, NULL, NULL, N'wayloadUSER1@gmail.com', N'WAYLOADUSER1@GMAIL.COM', N'wayloadUSER1@gmail.com', N'WAYLOADUSER1@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEMzcnkmdrtTHnLwyvZwXW4c3u2nRIuU2lrlyYBX1S22FptjY9DQWIdxRFW6FRD7sNA==', N'7QKVX2EA6BCUK6DLORQPD665HJQ2WLDV', N'eae02ec2-64f1-44c5-98e5-98db8f13c082', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [CreatedAt], [IsActive], [UpdatedAt], [RefreshToken], [RefreshTokenExpiry], [RefreshTokenIp], [RefreshTokenUserAgent], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', CAST(N'2026-02-13T17:31:23.2869516' AS DateTime2), 1, NULL, N'LTa9IYu9nct+3rw5CRye7eJVF5GQlfDYfBJNsZg6ySQrC5Y/S6ajEHF7vuqCWbrFddztL0UTM3qy4pMS2DkxBg==', CAST(N'2026-02-23T01:01:23.3604388' AS DateTime2), N'::1', NULL, N'wayloadUSER@gmail.com', N'WAYLOADUSER@GMAIL.COM', N'wayloadUSER@gmail.com', N'WAYLOADUSER@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEKcN5ockiuiMBiIz9XK+FfaGuCAnrnYtyuqpv2YJ/6ZHu7QIsQAAbV+vpEUYVFt0IQ==', N'YNENK6XLXSWBZZSI77ZHRLP6CKNPOD2O', N'd4cc38e0-02f9-4cc6-b0e2-a91682a4c979', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Bookmarks] ([Id], [VideoId], [UserId], [TimestampSeconds], [Title], [CreatedAt]) VALUES (N'522d3021-a1ba-40a4-a2c8-2de35b8402f9', N'919f2311-06c2-4639-aa40-72c0f16c285b', N'ba8eea65-a726-4216-aeeb-ce16b120bf8f', 2, N'Entry', CAST(N'2026-02-15T18:14:33.8803948' AS DateTime2))
INSERT [dbo].[Bookmarks] ([Id], [VideoId], [UserId], [TimestampSeconds], [Title], [CreatedAt]) VALUES (N'a583b726-cb01-4118-be74-36f2372d0451', N'ea50c766-d110-4836-bb7a-771dd500343d', N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', 7, N'Perfundimi', CAST(N'2026-02-15T00:58:01.8951696' AS DateTime2))
INSERT [dbo].[Bookmarks] ([Id], [VideoId], [UserId], [TimestampSeconds], [Title], [CreatedAt]) VALUES (N'354e49a1-6bee-4035-a1e7-43947cdaa1c3', N'ea50c766-d110-4836-bb7a-771dd500343d', N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', 4, N'Permbajtja', CAST(N'2026-02-15T00:57:45.4721689' AS DateTime2))
INSERT [dbo].[Bookmarks] ([Id], [VideoId], [UserId], [TimestampSeconds], [Title], [CreatedAt]) VALUES (N'6dfdaebc-fc0c-4dcf-9dc3-d48ab189e18a', N'ea50c766-d110-4836-bb7a-771dd500343d', N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', 2, N'Hyrja', CAST(N'2026-02-15T00:57:35.6966062' AS DateTime2))
GO
INSERT [dbo].[Videos] ([Id], [Title], [FilePath], [ContentType], [FileSizeBytes], [UserId], [UploadedAt]) VALUES (N'919f2311-06c2-4639-aa40-72c0f16c285b', N'MyPresantation', N'uploads/videos/ba8eea65-a726-4216-aeeb-ce16b120bf8f/919f2311-06c2-4639-aa40-72c0f16c285b.mov', N'video/quicktime', 16155881, N'ba8eea65-a726-4216-aeeb-ce16b120bf8f', CAST(N'2026-02-15T18:12:09.3945667' AS DateTime2))
INSERT [dbo].[Videos] ([Id], [Title], [FilePath], [ContentType], [FileSizeBytes], [UserId], [UploadedAt]) VALUES (N'ea50c766-d110-4836-bb7a-771dd500343d', N'Test', N'uploads/videos/695b235c-f6d2-4ec9-a60b-ec2f19cea460/ea50c766-d110-4836-bb7a-771dd500343d.mov', N'video/quicktime', 16155881, N'695b235c-f6d2-4ec9-a60b-ec2f19cea460', CAST(N'2026-02-14T22:42:15.4483612' AS DateTime2))
GO
/****** Object:  Index [IX_Annotations_UserId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_Annotations_UserId] ON [dbo].[Annotations]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Annotations_VideoId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_Annotations_VideoId] ON [dbo].[Annotations]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Bookmarks_UserId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_Bookmarks_UserId] ON [dbo].[Bookmarks]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Bookmarks_VideoId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_Bookmarks_VideoId] ON [dbo].[Bookmarks]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Videos_UserId]    Script Date: 2/16/2026 6:10:07 PM ******/
CREATE NONCLUSTERED INDEX [IX_Videos_UserId] ON [dbo].[Videos]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Annotations]  WITH CHECK ADD  CONSTRAINT [FK_Annotations_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Annotations] CHECK CONSTRAINT [FK_Annotations_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Annotations]  WITH CHECK ADD  CONSTRAINT [FK_Annotations_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Annotations] CHECK CONSTRAINT [FK_Annotations_Videos_VideoId]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Bookmarks]  WITH CHECK ADD  CONSTRAINT [FK_Bookmarks_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Bookmarks] CHECK CONSTRAINT [FK_Bookmarks_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Bookmarks]  WITH CHECK ADD  CONSTRAINT [FK_Bookmarks_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bookmarks] CHECK CONSTRAINT [FK_Bookmarks_Videos_VideoId]
GO
ALTER TABLE [dbo].[Videos]  WITH CHECK ADD  CONSTRAINT [FK_Videos_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Videos] CHECK CONSTRAINT [FK_Videos_AspNetUsers_UserId]
GO
USE [master]
GO
ALTER DATABASE [AssessmentErionShahiniDB] SET  READ_WRITE
GO
```
