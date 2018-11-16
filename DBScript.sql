USE [master]
GO
/****** Object:  Database [EF_DEMO]    Script Date: 11/16/2018 11:36:09 ******/
CREATE DATABASE [EF_DEMO]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EF_DEMO', FILENAME = N'/var/opt/mssql/data/EF_DEMO.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'EF_DEMO_log', FILENAME = N'/var/opt/mssql/data/EF_DEMO_log.ldf' , SIZE = 2304KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [EF_DEMO] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EF_DEMO].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EF_DEMO] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EF_DEMO] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EF_DEMO] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EF_DEMO] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EF_DEMO] SET ARITHABORT OFF 
GO
ALTER DATABASE [EF_DEMO] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EF_DEMO] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EF_DEMO] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EF_DEMO] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EF_DEMO] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EF_DEMO] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EF_DEMO] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EF_DEMO] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EF_DEMO] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EF_DEMO] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EF_DEMO] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EF_DEMO] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EF_DEMO] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EF_DEMO] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EF_DEMO] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EF_DEMO] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EF_DEMO] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EF_DEMO] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EF_DEMO] SET  MULTI_USER 
GO
ALTER DATABASE [EF_DEMO] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EF_DEMO] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EF_DEMO] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EF_DEMO] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [EF_DEMO] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'EF_DEMO', N'ON'
GO
ALTER DATABASE [EF_DEMO] SET QUERY_STORE = OFF
GO
USE [EF_DEMO]
GO
/****** Object:  UserDefinedFunction [dbo].[fnBuildAggregateFunctionColumns]    Script Date: 11/16/2018 11:36:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnBuildAggregateFunctionColumns] 
(
	@columns	varchar(1024)   
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @returnColumns		varchar(max)	= ''
		  , @tmpColumns			varchar(1024)
		  , @colSeparator		char(1)			= ','
		  , @aggSeparator		char(1)			= '|'
		  , @keywordSeparator	char(1)			= ':'
		  , @aggregate			varchar(20)
		  , @preComma			bit = 0
	
	WHILE LEN(@columns) > 0
	BEGIN
		IF LEN(@returnColumns) > 0
			SET @preComma = 1
		ELSE
			SET @preComma = 0
		-- if more than one aggregate functions
		IF CHARINDEX(@aggSeparator, @columns) > 0
		BEGIN			
			-- get aggregate function & columns
			SET @tmpColumns = SUBSTRING(@columns, 0, CHARINDEX(@aggSeparator, @columns))
			-- get aggreate function
			SET @aggregate = SUBSTRING(@tmpColumns, 0, CHARINDEX(@keywordSeparator, @columns))
			
			SET @tmpColumns = RIGHT(@tmpColumns, LEN(@tmpColumns) - CHARINDEX(@keywordSeparator, @columns))
			WHILE LEN(@tmpColumns) > 0
			BEGIN		
				IF LEN(@returnColumns) > 0
					SET @preComma = 1
				ELSE
					SET @preComma = 0					
				IF CHARINDEX(@colSeparator, @tmpColumns) > 0
				BEGIN
					--SELECT @returnColumns += CASE WHEN @preComma = 1 THEN ', ' ELSE '' END + @aggregate + '(' + SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns)) + ')' + ' AS ' + QUOTENAME( (LOWER(@aggregate) + CASE LTRIM(SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns))) WHEN '*' THEN 'All' ELSE LTRIM(SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns))) END), ']')
					SELECT @returnColumns += CASE WHEN @preComma = 1 THEN ', ' ELSE '' END + @aggregate + '(' + SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns)) + ')' + ' AS ' + QUOTENAME( (CASE LTRIM(SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns))) WHEN '*' THEN 'All' ELSE LTRIM(SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns))) END), ']')
					--SET @preComma = 1
					SET @tmpColumns = SUBSTRING(@tmpColumns, CHARINDEX(@colSeparator, @tmpColumns) + 1, LEN(@tmpColumns))
				END
				ELSE
				BEGIN
					--SELECT @returnColumns += CASE WHEN @preComma = 1 THEN ', ' ELSE '' END + @aggregate + '(' + @tmpColumns + ')' + ' AS ' + QUOTENAME( (LOWER(@aggregate) + CASE LTRIM(@tmpColumns) WHEN '*' THEN 'All' ELSE LTRIM(@tmpColumns) END), ']')
					SELECT @returnColumns += CASE WHEN @preComma = 1 THEN ', ' ELSE '' END + @aggregate + '(' + @tmpColumns + ')' + ' AS ' + QUOTENAME( (CASE LTRIM(@tmpColumns) WHEN '*' THEN 'All' ELSE LTRIM(@tmpColumns) END), ']')

					SET @tmpColumns = ''
				END				
			END								
			SET @columns = RIGHT(@columns, LEN(@columns) - CHARINDEX(@aggSeparator, @columns))					
		END
		ELSE -- only one aggregate function
		BEGIN
			-- get aggregate function & columns			
			SET @tmpColumns = @columns			
			-- get aggreate function
			SET @aggregate = SUBSTRING(@tmpColumns, 0, CHARINDEX(@keywordSeparator, @tmpColumns))			
			WHILE LEN(@tmpColumns) > 0
			BEGIN
				IF LEN(@returnColumns) > 0
					SET @preComma = 1
				ELSE
					SET @preComma = 0							
				SET @tmpColumns = RIGHT(@tmpColumns, LEN(@tmpColumns) - CHARINDEX(@keywordSeparator, @tmpColumns))	
											
				IF CHARINDEX(@colSeparator, @tmpColumns) > 0
				BEGIN										
					--SELECT @returnColumns += CASE WHEN @preComma = 1 THEN ', ' ELSE '' END + @aggregate + '(' + SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns)) + ')' + ' AS ' + QUOTENAME( (LOWER(@aggregate) + CASE LTRIM(SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns))) WHEN '*' THEN 'All' ELSE LTRIM(SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns))) END), ']')
					SELECT @returnColumns += CASE WHEN @preComma = 1 THEN ', ' ELSE '' END + @aggregate + '(' + SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns)) + ')' + ' AS ' + QUOTENAME( (CASE LTRIM(SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns))) WHEN '*' THEN 'All' ELSE LTRIM(SUBSTRING(@tmpColumns, 0, CHARINDEX(@colSeparator, @tmpColumns))) END), ']')
					SET @tmpColumns = SUBSTRING(@tmpColumns, CHARINDEX(@colSeparator, @tmpColumns) + 1, LEN(@tmpColumns))
				END
				ELSE
				BEGIN
					--SELECT @returnColumns += CASE WHEN @preComma = 1 THEN ', ' ELSE '' END + @aggregate + '(' + @tmpColumns + ')' + ' AS ' + QUOTENAME( (LOWER(@aggregate) + CASE LTRIM(@tmpColumns) WHEN '*' THEN 'All' ELSE LTRIM(@tmpColumns) END), ']')
					SELECT @returnColumns += CASE WHEN @preComma = 1 THEN ', ' ELSE '' END + @aggregate + '(' + @tmpColumns + ')' + ' AS ' + QUOTENAME( (CASE LTRIM(@tmpColumns) WHEN '*' THEN 'All' ELSE LTRIM(@tmpColumns) END), ']')
					SET @tmpColumns = ''
				END				
			END								
			SET @columns = ''			
		END
	END

	RETURN @returnColumns
END

GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/16/2018 11:36:09 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[City]    Script Date: 11/16/2018 11:36:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[CityId] [int] IDENTITY(1,1) NOT NULL,
	[CountryCode] [nvarchar](2) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 11/16/2018 11:36:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[CountryCode] [nvarchar](2) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[CountryCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hotel]    Script Date: 11/16/2018 11:36:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hotel](
	[HotelId] [int] IDENTITY(1,1) NOT NULL,
	[CountryCode] [nvarchar](2) NULL,
	[CityId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Hotel] PRIMARY KEY CLUSTERED 
(
	[HotelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[Hotel] ADD  CONSTRAINT [DF_Hotel_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Hotel] ADD  CONSTRAINT [DF_Hotel_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[City]  WITH CHECK ADD  CONSTRAINT [FK_City_Country] FOREIGN KEY([CountryCode])
REFERENCES [dbo].[Country] ([CountryCode])
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [FK_City_Country]
GO
ALTER TABLE [dbo].[Hotel]  WITH CHECK ADD  CONSTRAINT [FK_Hotel_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([CityId])
GO
ALTER TABLE [dbo].[Hotel] CHECK CONSTRAINT [FK_Hotel_City]
GO
ALTER TABLE [dbo].[Hotel]  WITH CHECK ADD  CONSTRAINT [FK_Hotel_Country] FOREIGN KEY([CountryCode])
REFERENCES [dbo].[Country] ([CountryCode])
GO
ALTER TABLE [dbo].[Hotel] CHECK CONSTRAINT [FK_Hotel_Country]
GO
/****** Object:  StoredProcedure [dbo].[pprHelperPaging]    Script Date: 11/16/2018 11:36:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pprHelperPaging]
(	  
	   @sql					varchar(max)			-- full sql queries for retrieval data (include column list, WHERE clause, ORDER BY clause,...	  
	  ,@sortField			varchar(50)				-- Sort Field (only single column support)
	  ,@pageNumber			int						-- Page Number
	  ,@pageSize			int						-- Page Size	  
	  ,@isPaging			bit						-- Is Paging (0 = no paging , 1 = has paging)	  
	  ,@reversed			bit						-- ASC or DESC order
	  ,@outputColumns		varchar(max)	= NULL		-- output column (e.g column1, column2, column3)	  
	  ,@aggregateBy			varchar(max)	= NULL
	  ,@groupBy				varchar(1024)	= NULL
	  ,@summaryOnlyFlag		bit				= 0
	  ,@outTotalResult		int output				-- total record to return.
)
AS
BEGIN

	SET NOCOUNT ON;


	-- Functional declaration
	DECLARE 
		 @nvchrSQL		nvarchar(MAX)
		,@nvchrReversed	nvarchar(5)
		,@intFirstRow	int
		,@intLastRow	int
		,@orderMethod	char(6) = ' DESC '
		,@groupbyColumns	varchar(1024)
	
	-- Error handing declare.
	DECLARE 
		 @intErr			int		
		,@intErrSeverity	int		= 16	
			 
		IF @reversed = 0 
			SET @orderMethod = ' ASC '
		ELSE IF @reversed = 1 
			SET @orderMethod = ' DESC '
		ELSE -- BIT type in MS SQL treat 0 as FALSE and the others as TRUE, thus the else block will never be entered except case NULL value.
			BEGIN				
				-- SET @vchrErrDetail = 'Reversed bit can be either 0 (ASC) or 1 (DESC)'				
				RAISERROR('Reversed bit can be either 0 (ASC) or 1 (DESC)', @intErrSeverity, 1)
				RETURN -100
			END

		-- If IsPaging bit is not 0 or 1, raise error.
		-- BIT type in MS SQL treat 0 as FALSE and the others as TRUE.
		IF @isPaging <> 1 AND @isPaging <> 0 OR @isPaging IS NULL
		BEGIN				
			-- SET @vchrErrDetail = 'IsPaging bit can be either 0 (No paging) or 1 (Paging is provided).'			
			RAISERROR('IsPaging bit can be either 0 (No paging) or 1 (Paging is provided).'	, @intErrSeverity, 1)
			RETURN -100
		END
		
		IF @isPaging = 1 -- Paging provided.
		BEGIN

			-- Invalid page size, raise error.
			IF @pageSize = 0 OR @pageSize < 0 OR @pageSize IS NULL
			BEGIN						
				-- SET @vchrErrDetail = 'Page Size can not be 0, less than 0 or NULL.'				
				RAISERROR('Page Size can not be 0, less than 0 or NULL.', @intErrSeverity, 1)
				RETURN -100
			END

			-- Invalid page number, raise error.
			IF @pageNumber = 0 OR @pageNumber < 0 OR @pageNumber IS NULL
			BEGIN						
				-- SET @vchrErrDetail = 'Page Number can not be 0, less than 0 or NULL.'				
				RAISERROR('Page Number can not be 0, less than 0 or NULL.', @intErrSeverity, 1)
				RETURN -100
			END

		END				

		-- process query with paging support and force sorting
		SET @nvchrSQL = N'SELECT @p_out_intRowCount = COUNT(1) 
							FROM (' + @sql + N') [result]'

		EXEC sp_executesql  @statement = @nvchrSQL
							,@params = N'@p_out_intRowCount int OUTPUT'							   
							,@p_out_intRowCount = @outTotalResult OUTPUT

		IF @isPaging = 0 -- No paging.
		BEGIN
			-- process query with no paging support but still force sorting
			IF @summaryOnlyFlag = 0
			BEGIN
				SET @nvchrSQL = N'SELECT '+ ISNULL(@outputColumns, '*') + N', ' + CAST(@outTotalResult AS varchar(10)) + N' AS TotalResult
								  FROM
								  (  
									SELECT   [result].*
										   , ROW_NUMBER() OVER(ORDER BY '+ @sortField + @orderMethod + N') AS RowNumber
									FROM (' + @sql + N') [result]
								  ) [finalResult]
								  ORDER BY [finalResult].RowNumber'

				EXEC sp_executesql @statement = @nvchrSQL
								   --,@params = N'@p_out_intRowCount int OUTPUT'							   
								   --,@p_out_intRowCount = @outTotalResult OUTPUT
			END
			--SET @outTotalResult = @@ROWCOUNT
		END
		ELSE -- IF @isPaging = 1 (Paging provided)
		BEGIN
			---- process query with paging support and force sorting
			--SET @nvchrSQL = N'SELECT @p_out_intRowCount = COUNT(1) 
			--				  FROM (' + @sql + N') [result]'

			--EXEC sp_executesql  @statement = @nvchrSQL
			--				   ,@params = N'@p_out_intRowCount int OUTPUT'							   
			--				   ,@p_out_intRowCount = @outTotalResult OUTPUT

			-- Prevent error when input page number greater than 1 and page size is greater than total result
			IF @pageSize >= @outTotalResult AND @pageNumber > 1
			BEGIN
				SET @pageNumber = 1
			END

			-- Compute first row and end row to return from the input required page number and page size. (1 based index)
			SET	@intFirstRow = ( @pageNumber - 1) * @pageSize + 1
			SET @intLastRow = (@pageNumber - 1) * @pageSize + @pageSize

			IF @intFirstRow > @outTotalResult
			BEGIN
				SET @intFirstRow = (@outTotalResult - @pageSize) + 1
				SET @intLastRow = @outTotalResult
			END

			-- Invalid page number, raise error.
			--IF @outTotalResult <> 0 AND @intFirstRow > @outTotalResult
			--BEGIN					
			--	-- SET @vchrErrDetail = 'The first row of the Page Number is greater than the total result.'				
			--	RAISERROR(3063542, @intErrSeverity, 1)
			--	RETURN -100
			--END

			-- Build the result set
			 IF @summaryOnlyFlag = 0
			 BEGIN
				 SET @nvchrSQL = N'SELECT '+ ISNULL(@outputColumns, '*') + N', ' + CAST(@outTotalResult AS varchar(10)) + N' AS TotalResult 
									  FROM
									  (
										SELECT [result].*, 
											 ROW_NUMBER() OVER(ORDER BY '+ @sortField + @orderMethod + N') AS RowNumber
										FROM (' + @sql + N') [result] 
									  ) [result_page]
									  WHERE RowNumber BETWEEN @firstRow AND @lastRow
									  ORDER BY RowNumber'
			
					EXEC sp_executesql  @statement = @nvchrSQL
									   ,@params = N'@firstRow int, @lastRow int'
									   ,@firstRow = @intFirstRow
									   ,@lastRow = @intLastRow
			 END	
		END

		-- Summary
		IF /*@aggreate IS NOT NULL AND @aggreate <> '' AND*/ @aggregateBy IS NOT NULL AND @aggregateBy <> '' AND @groupBy IS NOT NULL AND @groupBy <> ''
		BEGIN
				
			SET @nvchrSQL = N'	SELECT ' + @groupBy + ' , ' + (SELECT [dbo].[fnBuildAggregateFunctionColumns](@aggregateBy)) + '
									  ,COUNT(1) AS TotalResult
								FROM 
								(
									' + @sql + N'
								) [result]
								GROUP BY 
									' + @groupBy + '
								ORDER BY
									' + @sortField + '
									' + @orderMethod + '
								'
			
			EXEC sp_executesql  @statement = @nvchrSQL		
				
			--print @nvchrSQL						   
		END		
END

GO
/****** Object:  StoredProcedure [dbo].[prGetAllHotel]    Script Date: 11/16/2018 11:36:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[prGetAllHotel]
(
	@hotelId		int			  = 0
   ,@cityId			int			  = NULL
   ,@allRelevantFlag		bit	  = 0
   ,@sortField		varchar(50)	  = NULL
   ,@pageNumber		int			  = 1
   ,@pageSize		int			  = 50
   ,@isPaging		bit			  = 0
   ,@reversed		bit			  = 0
   ,@outTotalResult	int = 0 output
)
AS
BEGIN	
	SET NOCOUNT ON;	
	SET XACT_ABORT ON;
	
	-- Error declare
	DECLARE	  @ErrNumber	int
			, @ErrorMsg		nvarchar(1024)
			, @RetryCount	int	= 1			
		    , @ErrDetail	varchar(1024)
	-- Function declare		
	DECLARE
			  @sql				varchar(max)
			, @sqlScope			varchar(max) = ''
			, @entityTypeId		int
			, @entityTypeName	varchar(100) = 'Station'
	-- Initial values
			   
StoredProcedureRetry:	
	BEGIN TRY	
	
		SET @sortField = CASE WHEN @sortField IS NULL OR @sortField = '' THEN 'HotelId' ELSE @sortField END
		SET @hotelId = NULLIF(@hotelId, 0)


		SET @sql = '
			SELECT				
				   [HotelId]
				  ,[CountryCode]
				  ,[CityId]
				  ,[Name]
				  ,[Description]
				  ,[IsActive]
				  ,[CreateDate]
				  ,[UpdateDate]
			FROM Hotel WHERE 1=1'
			+
			CASE WHEN @hotelId IS NOT NULL THEN ' AND HotelId = ' + CAST(@hotelId AS varchar(10)) ELSE '' END
						
		EXEC [dbo].[pprHelperPaging]
				 @sql = @sql
				,@sortField = @sortField
				,@pageNumber = @pageNumber
				,@pageSize = @pageSize
				,@isPaging = @isPaging
				,@reversed = @reversed
				,@outputColumns = '*'
				,@outTotalResult = @outTotalResult output

	
							   						  
	END TRY
	BEGIN CATCH
		SELECT @ErrNumber = ERROR_NUMBER(), @ErrorMsg = ERROR_MESSAGE()
		
		IF XACT_STATE() = -1
			ROLLBACK TRAN
								
		IF (@ErrNumber = 1205 OR @ErrNumber = 2627) AND @RetryCount < 5
		BEGIN													
			SET @RetryCount += 1
			WAITFOR DELAY '00:00:00:250'
			GOTO StoredProcedureRetry
		END
		
		RAISERROR(@ErrorMsg, 16, 1);
		RETURN -112;
	END CATCH						
END


GO
USE [master]
GO
ALTER DATABASE [EF_DEMO] SET  READ_WRITE 
GO
