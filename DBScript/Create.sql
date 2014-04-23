USE [FCStore]
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_SelectRangeByWhere]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_SelectRangeByWhere]
	@RowIndex INT,
	@Count INT,
	@Where VARCHAR(3500),
	@DescStr VARCHAR(50) = 'BID',
	@DescTag BIT,
	@RowCount INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @FOrder NVARCHAR(4),@BOrder NVARCHAR(4)
	IF @DescTag = 1
	BEGIN
		SET @FOrder = N'ASC'
		SET @BOrder = N'DESC'
	END
	ELSE
	BEGIN
		SET @FOrder = N'DESC'
		SET @BOrder = N'ASC'
	END
	DECLARE @WhereSQL NVARCHAR(4000)
	SET @WhereSQL = N''
	IF Len(@Where) > 0
	BEGIN
		SET @WhereSQL = N'WHERE ' + @Where
	END
	DECLARE @SQL NVARCHAR(2000)
	SET @SQL = N'
	SELECT @RowCount = Count(1) FROM tb_brand ' + @WhereSQL + N'
	IF @RowIndex < @RowCount
	BEGIN
		IF (@RowCount - @RowIndex) < @Count
		BEGIN
			SET @Count = @RowCount - @RowIndex
		END
		DECLARE @FC INT
		SET @FC = @RowCount - @RowIndex
		SELECT TOP(@Count) * 
		FROM (SELECT TOP(@FC) * FROM tb_brand ' + @WhereSQL + '
		ORDER BY ' + @DescStr + ' ' + @FOrder
	IF @DescStr <> N'BID'
	BEGIN
		SET @SQL = @SQL + ',BID ' + @FOrder
	END
	SET @SQL = @SQL + ') AS tb ORDER BY ' + @DescStr + ' ' + @BOrder
	IF @DescStr <> N'BID'
	BEGIN
		SET @SQL = @SQL + ',BID ' + @BOrder
	END
	SET @SQL = @SQL  + '
	END'
	EXEC sp_executesql @SQL, N'@RowCount INT OUTPUT, @RowIndex INT, @Count INT, @DescTag Bit',@RowCount OUTPUT,@RowIndex,@Count,@DescTag
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_SelectByWhere]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_SelectByWhere]
	@Where VARCHAR(3500)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SQL NVARCHAR(4000)
	SET @SQL = 'SELECT * FROM tb_brand WHERE ' + @Where
	EXEC sp_executesql @SQL
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_DeleteByWhere]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_DeleteByWhere]
	@where VARCHAR(3500)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SQL NVARCHAR(4000)
	SET @SQL = N'DELETE tb_brand WHERE ' + @where
	EXEC sp_executesql @SQL
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Common_GetTableKey]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_Common_GetTableKey]
	-- Add the parameters for the stored procedure here
	@TableName VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @objectid int  
	Set @objectid=object_id(@TableName)
	Select col_name(@objectid,colid) as [key], COLUMNPROPERTY( @objectid,col_name(@objectid,colid),'IsIdentity') as [isIdentity]
	From  sysobjects         as o  
	Inner Join sysindexes    as i On i.name=o.name   
	Inner Join sysindexkeys  as k On k.indid=i.indid  
	Where   
	o.xtype = 'PK' and parent_obj=@objectid and k.id=@objectid
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_DeleteByWhere]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_DeleteByWhere]
	@where VARCHAR(3500)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SQL NVARCHAR(4000)
	SET @SQL = N'DELETE tb_category WHERE ' + @where
	EXEC sp_executesql @SQL
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_Update]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_Update]
	@BID int = 0
,	@NameStr nvarchar = NULL,
	@Tag int = NULL
AS
BEGIN
	DECLARE @SQLStr NVARCHAR(4000)
	SET @SQLStr = N'UPDATE tb_brand SET '
	IF @NameStr IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[NameStr] = [NameStr],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[NameStr] = @NameStr,'
	END
	IF @Tag IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Tag] = [Tag],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Tag] = @Tag,'
	END
	SET @SQLStr = Left(@SQLStr,Len(@SQLStr) - 1) + ' WHERE [BID] = @BID'
EXEC sp_executesql @SQLStr, N'@BID int,@NameStr nvarchar,@Tag int', @BID,@NameStr,@Tag
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_SelectRangeByWhere]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_SelectRangeByWhere]
	@RowIndex INT,
	@Count INT,
	@Where VARCHAR(3500),
	@DescStr VARCHAR(50) = 'CID',
	@DescTag BIT,
	@RowCount INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @FOrder NVARCHAR(4),@BOrder NVARCHAR(4)
	IF @DescTag = 1
	BEGIN
		SET @FOrder = N'ASC'
		SET @BOrder = N'DESC'
	END
	ELSE
	BEGIN
		SET @FOrder = N'DESC'
		SET @BOrder = N'ASC'
	END
	DECLARE @WhereSQL NVARCHAR(4000)
	SET @WhereSQL = N''
	IF Len(@Where) > 0
	BEGIN
		SET @WhereSQL = N'WHERE ' + @Where
	END
	DECLARE @SQL NVARCHAR(2000)
	SET @SQL = N'
	SELECT @RowCount = Count(1) FROM tb_category ' + @WhereSQL + N'
	IF @RowIndex < @RowCount
	BEGIN
		IF (@RowCount - @RowIndex) < @Count
		BEGIN
			SET @Count = @RowCount - @RowIndex
		END
		DECLARE @FC INT
		SET @FC = @RowCount - @RowIndex
		SELECT TOP(@Count) * 
		FROM (SELECT TOP(@FC) * FROM tb_category ' + @WhereSQL + '
		ORDER BY ' + @DescStr + ' ' + @FOrder
	IF @DescStr <> N'CID'
	BEGIN
		SET @SQL = @SQL + ',CID ' + @FOrder
	END
	SET @SQL = @SQL + ') AS tb ORDER BY ' + @DescStr + ' ' + @BOrder
	IF @DescStr <> N'CID'
	BEGIN
		SET @SQL = @SQL + ',CID ' + @BOrder
	END
	SET @SQL = @SQL  + '
	END'
	EXEC sp_executesql @SQL, N'@RowCount INT OUTPUT, @RowIndex INT, @Count INT, @DescTag Bit',@RowCount OUTPUT,@RowIndex,@Count,@DescTag
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_SelectByWhere]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_SelectByWhere]
	@Where VARCHAR(3500)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SQL NVARCHAR(4000)
	SET @SQL = 'SELECT * FROM tb_category WHERE ' + @Where
	EXEC sp_executesql @SQL
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_SelectRangeByWhere]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_SelectRangeByWhere]
	@RowIndex INT,
	@Count INT,
	@Where VARCHAR(3500),
	@DescStr VARCHAR(50) = 'PID',
	@DescTag BIT,
	@RowCount INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @FOrder NVARCHAR(4),@BOrder NVARCHAR(4)
	IF @DescTag = 1
	BEGIN
		SET @FOrder = N'ASC'
		SET @BOrder = N'DESC'
	END
	ELSE
	BEGIN
		SET @FOrder = N'DESC'
		SET @BOrder = N'ASC'
	END
	DECLARE @WhereSQL NVARCHAR(4000)
	SET @WhereSQL = N''
	IF Len(@Where) > 0
	BEGIN
		SET @WhereSQL = N'WHERE ' + @Where
	END
	DECLARE @SQL NVARCHAR(2000)
	SET @SQL = N'
	SELECT @RowCount = Count(1) FROM tb_product ' + @WhereSQL + N'
	IF @RowIndex < @RowCount
	BEGIN
		IF (@RowCount - @RowIndex) < @Count
		BEGIN
			SET @Count = @RowCount - @RowIndex
		END
		DECLARE @FC INT
		SET @FC = @RowCount - @RowIndex
		SELECT TOP(@Count) * 
		FROM (SELECT TOP(@FC) * FROM tb_product ' + @WhereSQL + '
		ORDER BY ' + @DescStr + ' ' + @FOrder
	IF @DescStr <> N'PID'
	BEGIN
		SET @SQL = @SQL + ',PID ' + @FOrder
	END
	SET @SQL = @SQL + ') AS tb ORDER BY ' + @DescStr + ' ' + @BOrder
	IF @DescStr <> N'PID'
	BEGIN
		SET @SQL = @SQL + ',PID ' + @BOrder
	END
	SET @SQL = @SQL  + '
	END'
	EXEC sp_executesql @SQL, N'@RowCount INT OUTPUT, @RowIndex INT, @Count INT, @DescTag Bit',@RowCount OUTPUT,@RowIndex,@Count,@DescTag
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_SelectByWhere]    Script Date: 04/23/2014 18:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_SelectByWhere]
	@Where VARCHAR(3500)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SQL NVARCHAR(4000)
	SET @SQL = 'SELECT * FROM tb_product WHERE ' + @Where
	EXEC sp_executesql @SQL
END
GO
/****** Object:  Table [dbo].[tb_product]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_product](
	[PID] [int] IDENTITY(1,1) NOT NULL,
	[CID] [int] NOT NULL,
	[Title] [nvarchar](1024) NOT NULL,
	[BrandID] [int] NOT NULL,
	[ChoseTag] [nvarchar](200) NULL,
	[Chose] [nvarchar](500) NULL,
	[Price] [float] NOT NULL,
	[MarketPrice] [nchar](10) NULL,
	[Discount] [int] NOT NULL,
	[Stock] [int] NOT NULL,
	[Sale] [int] NOT NULL,
	[ImgPath] [nvarchar](200) NULL,
	[Descript] [nvarchar](max) NOT NULL,
	[Tag] [int] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[PID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_category]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_category](
	[CID] [int] IDENTITY(1,1) NOT NULL,
	[ParCID] [int] NOT NULL,
	[NameStr] [nvarchar](50) NOT NULL,
	[Tag] [int] NOT NULL,
 CONSTRAINT [PK_tb_category] PRIMARY KEY CLUSTERED 
(
	[CID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_brand]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_brand](
	[BID] [int] IDENTITY(1,1) NOT NULL,
	[NameStr] [nvarchar](100) NOT NULL,
	[Tag] [int] NOT NULL,
 CONSTRAINT [PK_tb_brand] PRIMARY KEY CLUSTERED 
(
	[BID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_Update]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_Update]
	@PID int = 0
,	@CID int = NULL,
	@Title nvarchar = NULL,
	@BrandID int = NULL,
	@Chose nvarchar = NULL,
	@Price float = NULL,
	@Discount int = NULL,
	@Stock int = NULL,
	@Sale int = NULL,
	@Descript nvarchar = NULL,
	@Tag int = NULL
AS
BEGIN
	DECLARE @SQLStr NVARCHAR(4000)
	SET @SQLStr = N'UPDATE tb_product SET '
	IF @CID IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[CID] = [CID],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[CID] = @CID,'
	END
	IF @Title IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Title] = [Title],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Title] = @Title,'
	END
	IF @BrandID IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[BrandID] = [BrandID],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[BrandID] = @BrandID,'
	END
	IF @Chose IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Chose] = [Chose],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Chose] = @Chose,'
	END
	IF @Price IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Price] = [Price],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Price] = @Price,'
	END
	IF @Discount IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Discount] = [Discount],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Discount] = @Discount,'
	END
	IF @Stock IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Stock] = [Stock],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Stock] = @Stock,'
	END
	IF @Sale IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Sale] = [Sale],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Sale] = @Sale,'
	END
	IF @Descript IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Descript] = [Descript],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Descript] = @Descript,'
	END
	IF @Tag IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Tag] = [Tag],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Tag] = @Tag,'
	END
	SET @SQLStr = Left(@SQLStr,Len(@SQLStr) - 1) + ' WHERE [PID] = @PID'
EXEC sp_executesql @SQLStr, N'@PID int,@CID int,@Title nvarchar,@BrandID int,@Chose nvarchar,@Price float,@Discount int,@Stock int,@Sale int,@Descript nvarchar,@Tag int', @PID,@CID,@Title,@BrandID,@Chose,@Price,@Discount,@Stock,@Sale,@Descript,@Tag
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_DeleteByWhere]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_DeleteByWhere]
	@where VARCHAR(3500)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SQL NVARCHAR(4000)
	SET @SQL = N'DELETE tb_product WHERE ' + @where
	EXEC sp_executesql @SQL
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_Update]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_Update]
	@CID int = 0
,	@ParCID int = NULL,
	@NameStr nvarchar = NULL,
	@Tag int = NULL
AS
BEGIN
	DECLARE @SQLStr NVARCHAR(4000)
	SET @SQLStr = N'UPDATE tb_category SET '
	IF @ParCID IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[ParCID] = [ParCID],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[ParCID] = @ParCID,'
	END
	IF @NameStr IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[NameStr] = [NameStr],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[NameStr] = @NameStr,'
	END
	IF @Tag IS NULL
	BEGIN
		SET @SQLStr = @SQLStr + '[Tag] = [Tag],'
	END
	ELSE
	BEGIN
		SET @SQLStr = @SQLStr + '[Tag] = @Tag,'
	END
	SET @SQLStr = Left(@SQLStr,Len(@SQLStr) - 1) + ' WHERE [CID] = @CID'
EXEC sp_executesql @SQLStr, N'@CID int,@ParCID int,@NameStr nvarchar,@Tag int', @CID,@ParCID,@NameStr,@Tag
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_SelectRow]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_SelectRow]
	@CID int = 0
AS
BEGIN
	SELECT * FROM tb_category
	WHERE [CID] = @CID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_SelectRow]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_SelectRow]
	@PID int = 0
AS
BEGIN
	SELECT * FROM tb_product
	WHERE [PID] = @PID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_SelectAll]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_SelectAll]
AS
BEGIN
	SELECT * FROM tb_product
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_InsertUpdate]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_InsertUpdate]
	@PID int = 0,
	@CID int = 0,
	@Title nvarchar(1024) = '',
	@BrandID int = 0,
	@Chose nvarchar(500) = '',
	@Price float = 0,
	@Discount int = 0,
	@Stock int = 0,
	@Sale int = 0,
	@Descript nvarchar(MAX) = '',
	@Tag int = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Count INT
	SELECT @Count = Count(PID) FROM tb_product WHERE PID = @PID
	IF @Count > 0
	BEGIN
		UPDATE tb_product SET 
			[CID] = @CID,
			[Title] = @Title,
			[BrandID] = @BrandID,
			[Chose] = @Chose,
			[Price] = @Price,
			[Discount] = @Discount,
			[Stock] = @Stock,
			[Sale] = @Sale,
			[Descript] = @Descript,
			[Tag] = @Tag
		WHERE PID = @PID
	END
	ELSE
	BEGIN
		INSERT tb_product VALUES(
			@CID,
			@Title,
			@BrandID,
			@Chose,
			@Price,
			@Discount,
			@Stock,
			@Sale,
			@Descript,
			@Tag
)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_Insert]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_Insert]
	@CID int = 0,
	@Title nvarchar(1024) = '',
	@BrandID int = 0,
	@Chose nvarchar(500) = '',
	@Price float = 0,
	@Discount int = 0,
	@Stock int = 0,
	@Sale int = 0,
	@Descript nvarchar(Max) = '',
	@Tag int = 0
AS
BEGIN
	INSERT INTO tb_product
		([CID],[Title],[BrandID],[Chose],[Price],[Discount],[Stock],[Sale],[Descript],[Tag])
	VALUES
		(@CID,@Title,@BrandID,@Chose,@Price,@Discount,@Stock,@Sale,@Descript,@Tag)
	DECLARE @ReferenceID int
	SELECT @ReferenceID = @@IDENTITY
	Return @ReferenceID 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_product_DeleteRow]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_product_DeleteRow]
	@PID int = 0
AS
BEGIN
	DELETE tb_product WHERE [PID] = @PID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_SelectAll]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_SelectAll]
AS
BEGIN
	SELECT * FROM tb_category
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_InsertUpdate]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_InsertUpdate]
	@CID int = 0,
	@ParCID int = 0,
	@NameStr nvarchar(50) = '',
	@Tag int = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Count INT
	SELECT @Count = Count(CID) FROM tb_category WHERE CID = @CID
	IF @Count > 0
	BEGIN
		UPDATE tb_category SET 
			[ParCID] = @ParCID,
			[NameStr] = @NameStr,
			[Tag] = @Tag
		WHERE CID = @CID
	END
	ELSE
	BEGIN
		INSERT tb_category VALUES(
			@ParCID,
			@NameStr,
			@Tag
)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_Insert]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_Insert]
	@ParCID int = 0,
	@NameStr nvarchar(50) = '',
	@Tag int = 0
AS
BEGIN
	INSERT INTO tb_category
		([ParCID],[NameStr],[Tag])
	VALUES
		(@ParCID,@NameStr,@Tag)
	DECLARE @ReferenceID int
	SELECT @ReferenceID = @@IDENTITY
	Return @ReferenceID 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_category_DeleteRow]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_category_DeleteRow]
	@CID int = 0
AS
BEGIN
	DELETE tb_category WHERE [CID] = @CID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_SelectRow]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_SelectRow]
	@BID int = 0
AS
BEGIN
	SELECT * FROM tb_brand
	WHERE [BID] = @BID
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_SelectAll]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_SelectAll]
AS
BEGIN
	SELECT * FROM tb_brand
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_InsertUpdate]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_InsertUpdate]
	@BID int = 0,
	@NameStr nvarchar(100) = '',
	@Tag int = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Count INT
	SELECT @Count = Count(BID) FROM tb_brand WHERE BID = @BID
	IF @Count > 0
	BEGIN
		UPDATE tb_brand SET 
			[NameStr] = @NameStr,
			[Tag] = @Tag
		WHERE BID = @BID
	END
	ELSE
	BEGIN
		INSERT tb_brand VALUES(
			@NameStr,
			@Tag
)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_Insert]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_Insert]
	@NameStr nvarchar(100) = '',
	@Tag int = 0
AS
BEGIN
	INSERT INTO tb_brand
		([NameStr],[Tag])
	VALUES
		(@NameStr,@Tag)
	DECLARE @ReferenceID int
	SELECT @ReferenceID = @@IDENTITY
	Return @ReferenceID 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_tb_brand_DeleteRow]    Script Date: 04/23/2014 18:03:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_tb_brand_DeleteRow]
	@BID int = 0
AS
BEGIN
	DELETE tb_brand WHERE [BID] = @BID
END
GO
