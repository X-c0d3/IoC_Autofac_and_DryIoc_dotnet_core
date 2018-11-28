
USE [MiniProfilers]
GO
/****** Object:  Table [dbo].[MiniProfilerClientTimings]    Script Date: 11/28/2018 12:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MiniProfilerClientTimings](
	[RowId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[MiniProfilerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Start] [decimal](9, 3) NOT NULL,
	[Duration] [decimal](9, 3) NOT NULL,
 CONSTRAINT [PK__MiniProf__FFEE7431E9B40E9C] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MiniProfilers]    Script Date: 11/28/2018 12:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MiniProfilers](
	[RowId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[RootTimingId] [uniqueidentifier] NULL,
	[Name] [nvarchar](200) NULL,
	[Started] [datetime] NOT NULL,
	[DurationMilliseconds] [decimal](9, 3) NOT NULL,
	[User] [nvarchar](100) NULL,
	[HasUserViewed] [bit] NOT NULL,
	[MachineName] [nvarchar](100) NULL,
	[CustomLinksJson] [text] NULL,
	[ClientTimingsRedirectCount] [int] NULL,
 CONSTRAINT [PK__MiniProf__FFEE743111EC9CBF] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MiniProfilerTimings]    Script Date: 11/28/2018 12:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MiniProfilerTimings](
	[RowId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[MiniProfilerId] [uniqueidentifier] NOT NULL,
	[ParentTimingId] [uniqueidentifier] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[DurationMilliseconds] [decimal](9, 3) NOT NULL,
	[StartMilliseconds] [decimal](9, 3) NOT NULL,
	[IsRoot] [bit] NOT NULL,
	[Depth] [smallint] NOT NULL,
	[CustomTimingsJson] [text] NULL,
 CONSTRAINT [PK__MiniProf__FFEE7431F383091C] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

