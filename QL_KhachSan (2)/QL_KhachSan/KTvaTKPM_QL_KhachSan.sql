USE [master]
GO
/****** Object:  Database [KTvaTKPM_QL_KhachSan]    Script Date: 23/05/2025 10:11:13 AM ******/
CREATE DATABASE [KTvaTKPM_QL_KhachSan]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'KTvaTKPM_QL_KhachSan', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\KTvaTKPM_QL_KhachSan.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'KTvaTKPM_QL_KhachSan_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\KTvaTKPM_QL_KhachSan_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KTvaTKPM_QL_KhachSan].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET ARITHABORT OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET  DISABLE_BROKER 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET  MULTI_USER 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET DB_CHAINING OFF 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET QUERY_STORE = ON
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [KTvaTKPM_QL_KhachSan]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 23/05/2025 10:11:14 AM ******/
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
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
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
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
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
/****** Object:  Table [dbo].[Bookings]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bookings](
	[Id] [uniqueidentifier] NOT NULL,
	[RoomId] [uniqueidentifier] NOT NULL,
	[CheckInDate] [datetime2](7) NULL,
	[CheckOutDate] [datetime2](7) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CustomerName] [nvarchar](200) NOT NULL,
	[UserId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Bookings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterDataKeys]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterDataKeys](
	[Id] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_MasterDataKeys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterDataValues]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterDataValues](
	[Id] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[MasterDataKeyId] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_MasterDataValues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[Id] [uniqueidentifier] NOT NULL,
	[RoomNumber] [nvarchar](max) NOT NULL,
	[RoomTypeId] [uniqueidentifier] NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[PricePerNight] [decimal](18, 2) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CheckInDate] [datetime2](7) NULL,
	[CheckOutDate] [datetime2](7) NULL,
	[TotalPrice] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceRequests]    Script Date: 23/05/2025 10:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceRequests](
	[Id] [uniqueidentifier] NOT NULL,
	[GuestEmail] [nvarchar](max) NOT NULL,
	[ServiceValueName] [nvarchar](max) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[ServiceKeyName] [nvarchar](max) NOT NULL,
	[RequestedDate] [datetime2](7) NOT NULL,
	[CompletedDate] [datetime2](7) NULL,
	[StaffRemarks] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[AssignedStaffEmail] [nvarchar](max) NULL,
	[RequestedEndDate] [datetime2](7) NULL,
	[RequestedServicesDetails] [nvarchar](max) NULL,
 CONSTRAINT [PK_ServiceRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'00000000000000_CreateIdentitySchema', N'8.0.15')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250519133154_InitialCreate', N'8.0.15')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250521134958_UpdateServiceRequest', N'8.0.15')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250522115411_update', N'8.0.15')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250522123440_update', N'8.0.15')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'10e9529d-b670-4f53-b262-3a44738667f0', N'Customer', N'CUSTOMER', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'1195bdb5-744b-417b-ab39-de5f9a1693f4', N'Admin', N'ADMIN', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'23615263-b04f-402d-be6f-4a1fdabbad89', N'Receptionist', N'RECEPTIONIST', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'2fac935e-4c08-4903-920f-0f3cc1b85069', N'Manager', N'MANAGER', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'32853db8-0451-4106-95fa-0d2d52bc1fa8', N'Housekeeping', N'HOUSEKEEPING', NULL)
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 

INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1, N'9a6270cf-3623-4944-81a6-0a19139e8d00', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', N'ngominhtri290820@gmail.com')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2, N'9a6270cf-3623-4944-81a6-0a19139e8d00', N'IsActive', N'True')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (3, N'46edd6b2-0724-4b87-8ffb-8ddb62768d6c', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', N'2224802010314@student.tdmu.edu.vn')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (5, N'139b80d6-c818-48c3-ac15-76f63de08258', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', N'ngominhtrichgpt@gmail.com')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (9, N'139b80d6-c818-48c3-ac15-76f63de08258', N'AvatarUrl', N'/uploads/avatars/139b80d6-c818-48c3-ac15-76f63de08258_62804771.jpg')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (11, N'139b80d6-c818-48c3-ac15-76f63de08258', N'IsActive', N'True')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1007, N'9a6270cf-3623-4944-81a6-0a19139e8d00', N'AvatarUrl', N'/uploads/avatars/9a6270cf-3623-4944-81a6-0a19139e8d00_52c1b43b.jpg')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1008, N'46edd6b2-0724-4b87-8ffb-8ddb62768d6c', N'IsActive', N'False')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
GO
INSERT [dbo].[AspNetUserLogins] ([LoginProvider], [ProviderKey], [ProviderDisplayName], [UserId]) VALUES (N'Google', N'100095745877142466033', N'Google', N'139b80d6-c818-48c3-ac15-76f63de08258')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'139b80d6-c818-48c3-ac15-76f63de08258', N'10e9529d-b670-4f53-b262-3a44738667f0')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'9a6270cf-3623-4944-81a6-0a19139e8d00', N'1195bdb5-744b-417b-ab39-de5f9a1693f4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'46edd6b2-0724-4b87-8ffb-8ddb62768d6c', N'2fac935e-4c08-4903-920f-0f3cc1b85069')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'139b80d6-c818-48c3-ac15-76f63de08258', N'ngominhtrichgpt@gmail.com', N'NGOMINHTRICHGPT@GMAIL.COM', N'ngominhtrichgpt@gmail.com', N'NGOMINHTRICHGPT@GMAIL.COM', 1, NULL, N'QFW44IRN7LDLRESCZMKYECT3QRMM2F2X', N'7fe41c06-6974-4c9a-a167-e5d92d38c05c', N'0123465789', 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'46edd6b2-0724-4b87-8ffb-8ddb62768d6c', N'Manager', N'MANAGER', N'2224802010314@student.tdmu.edu.vn', N'2224802010314@STUDENT.TDMU.EDU.VN', 1, N'AQAAAAIAAYagAAAAEPV5/UV3ikimFzT1pli+aj3XF+K4MqglRuzTKsUPF91GZUUKm/DvTJq8fFdX/4nR+w==', N'VN2UH4BODHEBQJ5ERAZPBVQ77DZQAHW4', N'8225836d-c1bb-4958-9d19-e8ea2d9a6db5', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'9a6270cf-3623-4944-81a6-0a19139e8d00', N'Admin', N'ADMIN', N'ngominhtri290820@gmail.com', N'NGOMINHTRI290820@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEDY+YIwGCeNXXuC5Qoh38DzMZI3Dzc1Rm7MYkhTjnHpNm/Kb526s+TWmxbToUVl4dA==', N'NO2FXD4YQYZ6BZEAES4DGBBYDNAXMDYR', N'a1acba57-3b55-4178-9d37-9085419d4d2e', N'0123465789', 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Bookings] ([Id], [RoomId], [CheckInDate], [CheckOutDate], [Status], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CustomerName], [UserId]) VALUES (N'008fa676-2bd5-4be3-92a3-1942b1be4ba6', N'f0c47bd8-94db-4a5f-838a-7a26b8e4fc26', CAST(N'2025-05-22T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-23T12:00:00.0000000' AS DateTime2), N'Cancelled', CAST(N'2025-05-22T13:21:43.8285898' AS DateTime2), CAST(N'2025-05-22T13:21:43.8285899' AS DateTime2), NULL, NULL, 0, N'Minh Trí', N'9a6270cf-3623-4944-81a6-0a19139e8d00')
INSERT [dbo].[Bookings] ([Id], [RoomId], [CheckInDate], [CheckOutDate], [Status], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CustomerName], [UserId]) VALUES (N'ad525d68-94ac-4067-acd9-1943306a49f7', N'b788d556-2392-4319-984b-e86c14661daa', CAST(N'2025-05-23T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-24T12:00:00.0000000' AS DateTime2), N'Confirmed', CAST(N'2025-05-23T03:04:51.4676157' AS DateTime2), CAST(N'2025-05-23T03:05:20.4558504' AS DateTime2), N'ngominhtrichgpt@gmail.com', N'Admin', 0, N'ádasd', N'139b80d6-c818-48c3-ac15-76f63de08258')
INSERT [dbo].[Bookings] ([Id], [RoomId], [CheckInDate], [CheckOutDate], [Status], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CustomerName], [UserId]) VALUES (N'd66a97c7-d9e0-4b32-a4bf-3d0a02a23a85', N'50b12320-b987-4f55-bd37-381f93c5b131', CAST(N'2025-05-22T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-23T12:00:00.0000000' AS DateTime2), N'Confirmed', CAST(N'2025-05-22T13:50:23.2708881' AS DateTime2), CAST(N'2025-05-22T13:50:34.6693776' AS DateTime2), N'ngominhtri290820@gmail.com', N'Admin', 0, N'Minh Trí', N'9a6270cf-3623-4944-81a6-0a19139e8d00')
INSERT [dbo].[Bookings] ([Id], [RoomId], [CheckInDate], [CheckOutDate], [Status], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CustomerName], [UserId]) VALUES (N'5af73137-ae06-403e-bf41-522ad906e044', N'f0c47bd8-94db-4a5f-838a-7a26b8e4fc26', CAST(N'2025-05-22T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-25T12:00:00.0000000' AS DateTime2), N'Confirmed', CAST(N'2025-05-22T13:22:58.3799294' AS DateTime2), CAST(N'2025-05-22T13:22:58.3799294' AS DateTime2), NULL, NULL, 0, N'M', N'9a6270cf-3623-4944-81a6-0a19139e8d00')
INSERT [dbo].[Bookings] ([Id], [RoomId], [CheckInDate], [CheckOutDate], [Status], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CustomerName], [UserId]) VALUES (N'a7b97998-b283-46b6-be73-71f2fd6ef519', N'f0c47bd8-94db-4a5f-838a-7a26b8e4fc26', CAST(N'2025-05-22T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-24T12:00:00.0000000' AS DateTime2), N'Rejected', CAST(N'2025-05-22T13:20:57.6514443' AS DateTime2), CAST(N'2025-05-22T13:20:57.6514446' AS DateTime2), NULL, NULL, 0, N'Minh Trí', N'9a6270cf-3623-4944-81a6-0a19139e8d00')
INSERT [dbo].[Bookings] ([Id], [RoomId], [CheckInDate], [CheckOutDate], [Status], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CustomerName], [UserId]) VALUES (N'e8830ac0-0937-48f9-8afe-78d7fff57388', N'f0c47bd8-94db-4a5f-838a-7a26b8e4fc26', CAST(N'2025-05-22T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-28T12:00:00.0000000' AS DateTime2), N'Rejected', CAST(N'2025-05-22T13:26:20.9951834' AS DateTime2), CAST(N'2025-05-22T13:50:36.1316250' AS DateTime2), NULL, N'Admin', 0, N'Minh Trí', N'9a6270cf-3623-4944-81a6-0a19139e8d00')
INSERT [dbo].[Bookings] ([Id], [RoomId], [CheckInDate], [CheckOutDate], [Status], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CustomerName], [UserId]) VALUES (N'21d2a5d2-b5e8-45ff-9eb5-d2ba39aef477', N'93a35c18-4c95-4668-9d18-6970c882e2bb', CAST(N'2025-05-22T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-23T12:00:00.0000000' AS DateTime2), N'PendingApproval', CAST(N'2025-05-22T14:22:34.7617313' AS DateTime2), CAST(N'2025-05-22T14:22:34.7617318' AS DateTime2), N'ngominhtri290820@gmail.com', N'ngominhtri290820@gmail.com', 0, N'Minh Trí', N'9a6270cf-3623-4944-81a6-0a19139e8d00')
GO
INSERT [dbo].[MasterDataKeys] ([Id], [IsActive], [Name], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'a6273ac4-3d1e-4efa-1424-08dd9927819e', 1, N'Phòng', CAST(N'2025-05-22T11:55:11.3742353' AS DateTime2), CAST(N'2025-05-22T11:55:11.3742358' AS DateTime2), N'Admin', NULL, 0)
INSERT [dbo].[MasterDataKeys] ([Id], [IsActive], [Name], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'4a931489-4da2-41bb-1425-08dd9927819e', 1, N'Dịch Vụ', CAST(N'2025-05-22T11:55:16.1140996' AS DateTime2), CAST(N'2025-05-22T11:55:16.1141006' AS DateTime2), N'Admin', NULL, 0)
GO
INSERT [dbo].[MasterDataValues] ([Id], [IsActive], [Name], [MasterDataKeyId], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'c2a642fa-974a-47fc-649f-08dd99278be7', 1, N'Phòng VIP', N'a6273ac4-3d1e-4efa-1424-08dd9927819e', CAST(N'2025-05-22T11:55:28.6424807' AS DateTime2), CAST(N'2025-05-22T11:55:28.6424813' AS DateTime2), N'Admin', NULL, 0)
INSERT [dbo].[MasterDataValues] ([Id], [IsActive], [Name], [MasterDataKeyId], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'89f07bc5-8993-4d2d-64a0-08dd99278be7', 1, N'Phòng Thường', N'a6273ac4-3d1e-4efa-1424-08dd9927819e', CAST(N'2025-05-22T11:55:36.0319162' AS DateTime2), CAST(N'2025-05-22T11:55:36.0319169' AS DateTime2), N'Admin', NULL, 0)
INSERT [dbo].[MasterDataValues] ([Id], [IsActive], [Name], [MasterDataKeyId], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'eb874424-af31-41bf-64a1-08dd99278be7', 1, N'Phòng Đôi', N'a6273ac4-3d1e-4efa-1424-08dd9927819e', CAST(N'2025-05-22T11:55:42.9591791' AS DateTime2), CAST(N'2025-05-22T11:55:42.9591798' AS DateTime2), N'Admin', NULL, 0)
INSERT [dbo].[MasterDataValues] ([Id], [IsActive], [Name], [MasterDataKeyId], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'b3aad66f-95ca-4fde-64a2-08dd99278be7', 1, N'Phòng Đơn', N'a6273ac4-3d1e-4efa-1424-08dd9927819e', CAST(N'2025-05-22T11:55:48.7081480' AS DateTime2), CAST(N'2025-05-22T11:55:48.7081485' AS DateTime2), N'Admin', NULL, 0)
INSERT [dbo].[MasterDataValues] ([Id], [IsActive], [Name], [MasterDataKeyId], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'68c9b62d-505d-4d0f-64a3-08dd99278be7', 1, N'Phòng Gia Đình', N'a6273ac4-3d1e-4efa-1424-08dd9927819e', CAST(N'2025-05-22T11:55:54.9373453' AS DateTime2), CAST(N'2025-05-22T11:55:54.9373460' AS DateTime2), N'Admin', NULL, 0)
INSERT [dbo].[MasterDataValues] ([Id], [IsActive], [Name], [MasterDataKeyId], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'42bdec52-9dd8-4337-56dd-08dd993fa341', 1, N'Ăn ', N'4a931489-4da2-41bb-1425-08dd9927819e', CAST(N'2025-05-22T14:47:55.7131228' AS DateTime2), CAST(N'2025-05-22T14:57:32.7948000' AS DateTime2), N'Admin', NULL, 1)
INSERT [dbo].[MasterDataValues] ([Id], [IsActive], [Name], [MasterDataKeyId], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'85b0cc08-84b5-43c2-56de-08dd993fa341', 1, N'Thuê Xe', N'4a931489-4da2-41bb-1425-08dd9927819e', CAST(N'2025-05-22T14:48:03.2643354' AS DateTime2), CAST(N'2025-05-22T14:48:03.2643361' AS DateTime2), N'Admin', NULL, 0)
INSERT [dbo].[MasterDataValues] ([Id], [IsActive], [Name], [MasterDataKeyId], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted]) VALUES (N'7e18e73e-8919-4286-f3bd-08dd9940fef5', 1, N'Ăn Sáng', N'4a931489-4da2-41bb-1425-08dd9927819e', CAST(N'2025-05-22T14:57:39.0798302' AS DateTime2), CAST(N'2025-05-22T14:57:39.0798311' AS DateTime2), N'Admin', NULL, 0)
GO
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'df676dca-6c2a-4b1c-ad46-07a2888f18ba', N'GD503', N'68c9b62d-505d-4d0f-64a3-08dd99278be7', N'Trống', CAST(1850000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'354d0497-d09f-47ac-8c1e-1077ec520e60', N'309', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Available', CAST(1020000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'64817555-3e5b-4066-bc17-174561f90eba', N'THUONG203', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Trống', CAST(800000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'36e9e1ef-ebff-43e9-86e2-19831d8ecbec', N'VIP103', N'c2a642fa-974a-47fc-649f-08dd99278be7', N'Trống', CAST(2550000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'48abc9a4-fae5-424c-980b-20e3e4a664cb', N'203', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Available', CAST(850000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'bed8a4fd-dcab-4a35-9086-276bd5aa4c30', N'DON401', N'b3aad66f-95ca-4fde-64a2-08dd99278be7', N'Trống', CAST(700000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'77e226fd-44c7-4114-b49a-278649d4118b', N'102', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Available', CAST(520000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'50b12320-b987-4f55-bd37-381f93c5b131', N'202', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Booked', CAST(820000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T13:50:34.6699717' AS DateTime2), NULL, N'Admin', 0, CAST(N'2025-05-22T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-23T12:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'f23ca09f-8232-4e2f-8ebc-448ce883fd6f', N'304', N'c2a642fa-974a-47fc-649f-08dd99278be7', N'Available', CAST(2200000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'9f15a8a2-dd10-4171-88b4-4b42160241c9', N'DOI303', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Đang sử dụng', CAST(1300000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'9cd9a804-327f-4bf0-82c4-5152b01dc75e', N'VIP101', N'c2a642fa-974a-47fc-649f-08dd99278be7', N'Trống', CAST(2500000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'01b1290f-496a-4069-8142-5c37fa6cbb34', N'301', N'c2a642fa-974a-47fc-649f-08dd99278be7', N'Available', CAST(2000000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'0a903870-081e-4f77-8631-5ec5a3b8c1be', N'107', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Available', CAST(510000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'41b4ccc4-c543-438c-bd63-600a0a649951', N'THUONG201', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Trống', CAST(800000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'fb9c3c49-951e-45ea-8f31-637fd9f91928', N'VIP102', N'c2a642fa-974a-47fc-649f-08dd99278be7', N'Đang sử dụng', CAST(2600000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'93a35c18-4c95-4668-9d18-6970c882e2bb', N'306', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Available', CAST(950000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'dc029964-dbb8-4982-beb1-6c0ea9430a19', N'110', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Available', CAST(550000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'c0ff846b-ba4c-4a6e-a73c-746134c046d7', N'310', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Available', CAST(1050000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'f0c47bd8-94db-4a5f-838a-7a26b8e4fc26', N'201', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Booked', CAST(800000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, CAST(N'2025-05-22T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-25T12:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'681750bf-0a01-49fb-845a-800b890563ad', N'108', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Booked', CAST(540000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'edc3846c-fc97-46a7-ae32-8e9933712ce4', N'308', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Available', CAST(1000000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'b08a31ed-3a69-4842-8cfc-99230a8b1bd4', N'302', N'c2a642fa-974a-47fc-649f-08dd99278be7', N'Available', CAST(2100000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'2ab0891a-bf9a-4ecc-b6db-99511944f089', N'DOI301', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Trống', CAST(1200000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'd39b0b74-7900-441b-b2a9-9fb6078bc99e', N'THUONG204', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Đang sử dụng', CAST(850000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'ad297fa4-c802-47db-bc18-a627d001391c', N'103', N'b3aad66f-95ca-4fde-64a2-08dd99278be7', N'Available', CAST(450000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'9e468d48-a107-47f2-970a-a9a62bdea13f', N'DON403', N'b3aad66f-95ca-4fde-64a2-08dd99278be7', N'Trống', CAST(720000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'34f3d595-7670-4ddf-9051-b0c59170c39b', N'208', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Available', CAST(830000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'0ff63970-60ff-4ecb-8c7d-b9efba08fe54', N'106', N'b3aad66f-95ca-4fde-64a2-08dd99278be7', N'Available', CAST(480000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'c1365388-37b4-4569-a648-bcfe54179a5f', N'305', N'c2a642fa-974a-47fc-649f-08dd99278be7', N'Available', CAST(2300000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'9323e283-1e26-40d6-b2f3-c15713116246', N'206', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Available', CAST(900000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'491c0798-f9d6-4380-bff5-cabdbdeeeb5f', N'104', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Maintenance', CAST(500000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'139ef59a-cf2b-443c-8776-d2e6f2e111be', N'307', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Available', CAST(980000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'aa30ab2e-b031-4df8-98ab-d5d4e46aa0ba', N'101', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Available', CAST(500000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'3514c3d1-e33a-43e6-9767-d6f35bb09cf5', N'GD501', N'68c9b62d-505d-4d0f-64a3-08dd99278be7', N'Trống', CAST(1800000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'd806a282-f987-4956-94ec-dce766d8a3bf', N'303', N'c2a642fa-974a-47fc-649f-08dd99278be7', N'Maintenance', CAST(2000000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'708ddbe7-591c-491a-abd0-df0a4183009e', N'207', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Booked', CAST(920000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'5919c413-2f8a-468c-9098-e7b70a2ed382', N'THUONG202', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Đang dọn dẹp', CAST(820000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'b788d556-2392-4319-984b-e86c14661daa', N'205', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Booked', CAST(880000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-23T03:05:20.4565405' AS DateTime2), NULL, N'Admin', 0, CAST(N'2025-05-23T14:00:00.0000000' AS DateTime2), CAST(N'2025-05-24T12:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'8ad5d184-4a25-4c2e-bdd6-f0e6b0b229ab', N'204', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Maintenance', CAST(800000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'44488c89-571b-4d50-9c80-f207a1a24d55', N'GD502', N'68c9b62d-505d-4d0f-64a3-08dd99278be7', N'Đang sử dụng', CAST(1900000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'3130fa12-4650-424b-b188-f25af61c8c5e', N'DON402', N'b3aad66f-95ca-4fde-64a2-08dd99278be7', N'Đang dọn dẹp', CAST(700000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'b6c9a52d-82a4-4cd4-8fea-f3ce02ee8d98', N'105', N'89f07bc5-8993-4d2d-64a0-08dd99278be7', N'Available', CAST(530000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'4cee3b3d-64c0-4c91-843d-fb4205ed4887', N'210', N'68c9b62d-505d-4d0f-64a3-08dd99278be7', N'Available', CAST(1550000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'ccf2f1bd-688f-46c8-ae00-fc64e01144cd', N'209', N'68c9b62d-505d-4d0f-64a3-08dd99278be7', N'Available', CAST(1500000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'e53241c4-e99b-443b-b4d7-fc6761a36431', N'DOI302', N'eb874424-af31-41bf-64a1-08dd99278be7', N'Trống', CAST(1250000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), CAST(N'2025-05-22T18:57:56.5433333' AS DateTime2), N'Admin', N'Admin', 0, NULL, NULL, NULL)
INSERT [dbo].[Rooms] ([Id], [RoomNumber], [RoomTypeId], [Status], [PricePerNight], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [CheckInDate], [CheckOutDate], [TotalPrice]) VALUES (N'375f00c7-721c-421d-ae73-ffddd9c7ec9f', N'109', N'b3aad66f-95ca-4fde-64a2-08dd99278be7', N'Available', CAST(460000.00 AS Decimal(18, 2)), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), CAST(N'2025-05-22T11:57:56.5466667' AS DateTime2), NULL, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[ServiceRequests] ([Id], [GuestEmail], [ServiceValueName], [Status], [ServiceKeyName], [RequestedDate], [CompletedDate], [StaffRemarks], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [AssignedStaffEmail], [RequestedEndDate], [RequestedServicesDetails]) VALUES (N'00860282-5d29-48f3-b092-03a966c36253', N'ngominhtri290820@gmail.com', N'Ăn Sáng', N'Chờ xác nhận', N'Dịch Vụ', CAST(N'2025-05-22T00:00:00.0000000' AS DateTime2), NULL, NULL, CAST(N'2025-05-22T15:04:54.7233307' AS DateTime2), CAST(N'2025-05-22T15:04:54.7233313' AS DateTime2), N'ngominhtri290820@gmail.com', NULL, 0, NULL, CAST(N'2025-05-24T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[ServiceRequests] ([Id], [GuestEmail], [ServiceValueName], [Status], [ServiceKeyName], [RequestedDate], [CompletedDate], [StaffRemarks], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [AssignedStaffEmail], [RequestedEndDate], [RequestedServicesDetails]) VALUES (N'2d8aee53-f08b-4af8-b711-7641b5b7e8ae', N'ngominhtrichgpt@gmail.com', N'Thuê Xe', N'Đã từ chối', N'Dịch Vụ', CAST(N'2025-05-23T00:00:00.0000000' AS DateTime2), NULL, N'hết xe', CAST(N'2025-05-22T17:00:47.8695463' AS DateTime2), CAST(N'2025-05-23T03:07:24.5713665' AS DateTime2), N'ngominhtrichgpt@gmail.com', N'ngominhtri290820@gmail.com', 0, NULL, CAST(N'2025-05-24T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[ServiceRequests] ([Id], [GuestEmail], [ServiceValueName], [Status], [ServiceKeyName], [RequestedDate], [CompletedDate], [StaffRemarks], [CreatedDate], [UpdatedDate], [CreatedBy], [UpdatedBy], [IsDeleted], [AssignedStaffEmail], [RequestedEndDate], [RequestedServicesDetails]) VALUES (N'408bf836-aa56-42ce-8529-e5c97186b942', N'ngominhtri290820@gmail.com', N'Thuê Xe', N'Chờ xác nhận', N'Dịch Vụ', CAST(N'2025-05-22T00:00:00.0000000' AS DateTime2), NULL, NULL, CAST(N'2025-05-22T14:48:23.8625374' AS DateTime2), CAST(N'2025-05-22T14:48:23.8625376' AS DateTime2), N'ngominhtri290820@gmail.com', NULL, 0, NULL, CAST(N'2025-05-23T00:00:00.0000000' AS DateTime2), NULL)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Bookings_RoomId]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_Bookings_RoomId] ON [dbo].[Bookings]
(
	[RoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Bookings_UserId]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_Bookings_UserId] ON [dbo].[Bookings]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MasterDataValues_MasterDataKeyId]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_MasterDataValues_MasterDataKeyId] ON [dbo].[MasterDataValues]
(
	[MasterDataKeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Rooms_RoomTypeId]    Script Date: 23/05/2025 10:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_Rooms_RoomTypeId] ON [dbo].[Rooms]
(
	[RoomTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bookings] ADD  DEFAULT (N'') FOR [CustomerName]
GO
ALTER TABLE [dbo].[ServiceRequests] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [RequestedDate]
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
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_Bookings_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_Rooms_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_Bookings_Rooms_RoomId]
GO
ALTER TABLE [dbo].[MasterDataValues]  WITH CHECK ADD  CONSTRAINT [FK_MasterDataValues_MasterDataKeys_MasterDataKeyId] FOREIGN KEY([MasterDataKeyId])
REFERENCES [dbo].[MasterDataKeys] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MasterDataValues] CHECK CONSTRAINT [FK_MasterDataValues_MasterDataKeys_MasterDataKeyId]
GO
ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD  CONSTRAINT [FK_Rooms_MasterDataValues_RoomTypeId] FOREIGN KEY([RoomTypeId])
REFERENCES [dbo].[MasterDataValues] ([Id])
GO
ALTER TABLE [dbo].[Rooms] CHECK CONSTRAINT [FK_Rooms_MasterDataValues_RoomTypeId]
GO
USE [master]
GO
ALTER DATABASE [KTvaTKPM_QL_KhachSan] SET  READ_WRITE 
GO
