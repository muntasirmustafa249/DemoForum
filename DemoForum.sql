USE [master]
GO
/****** Object:  Database [DemoForum]    Script Date: 11/07/24 11:06:06 PM ******/
CREATE DATABASE [DemoForum]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DemoForum', FILENAME = N'H:\Local DB\SQL17\MSSQL14.SQL17\MSSQL\DATA\DemoForum.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DemoForum_log', FILENAME = N'H:\Local DB\SQL17\MSSQL14.SQL17\MSSQL\DATA\DemoForum_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [DemoForum] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DemoForum].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DemoForum] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DemoForum] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DemoForum] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DemoForum] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DemoForum] SET ARITHABORT OFF 
GO
ALTER DATABASE [DemoForum] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DemoForum] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DemoForum] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DemoForum] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DemoForum] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DemoForum] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DemoForum] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DemoForum] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DemoForum] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DemoForum] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DemoForum] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DemoForum] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DemoForum] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DemoForum] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DemoForum] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DemoForum] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DemoForum] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DemoForum] SET RECOVERY FULL 
GO
ALTER DATABASE [DemoForum] SET  MULTI_USER 
GO
ALTER DATABASE [DemoForum] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DemoForum] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DemoForum] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DemoForum] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DemoForum] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DemoForum', N'ON'
GO
ALTER DATABASE [DemoForum] SET QUERY_STORE = OFF
GO
USE [DemoForum]
GO
/****** Object:  Table [dbo].[Answer]    Script Date: 11/07/24 11:06:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[Content] [varchar](max) NOT NULL,
	[UpvoteCount] [int] NOT NULL,
	[DownvoteCount] [int] NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[PostedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 11/07/24 11:06:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NOT NULL,
	[PostType] [varchar](10) NOT NULL,
	[Content] [varchar](max) NOT NULL,
	[UpvoteCount] [int] NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[PostedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Question]    Script Date: 11/07/24 11:06:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[UpvoteCount] [int] NOT NULL,
	[DownvoteCount] [int] NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[PostedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/07/24 11:06:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[RegisteredOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Answer] ADD  DEFAULT ((0)) FOR [UpvoteCount]
GO
ALTER TABLE [dbo].[Answer] ADD  DEFAULT ((0)) FOR [DownvoteCount]
GO
ALTER TABLE [dbo].[Answer] ADD  DEFAULT (getdate()) FOR [PostedOn]
GO
ALTER TABLE [dbo].[Comment] ADD  DEFAULT ((0)) FOR [UpvoteCount]
GO
ALTER TABLE [dbo].[Comment] ADD  DEFAULT (getdate()) FOR [PostedOn]
GO
ALTER TABLE [dbo].[Question] ADD  DEFAULT ((0)) FOR [UpvoteCount]
GO
ALTER TABLE [dbo].[Question] ADD  DEFAULT ((0)) FOR [DownvoteCount]
GO
ALTER TABLE [dbo].[Question] ADD  DEFAULT (getdate()) FOR [PostedOn]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [RegisteredOn]
GO
/****** Object:  StoredProcedure [dbo].[spCheckUser]    Script Date: 11/07/24 11:06:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spCheckUser]
	@UserName VARCHAR(20),
	@Email VARCHAR(50)
AS
BEGIN
	SELECT * FROM [User] WHERE UserName = @UserName OR Email = @Email
END
GO
/****** Object:  StoredProcedure [dbo].[spCreateAnswer]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spCreateAnswer]
	@QuestionId INT,
	@Content VARCHAR(MAX),
	@UserName VARCHAR(20),
	@Email VARCHAR(50)
AS
BEGIN
	INSERT INTO Answer(QuestionId, Content, UpvoteCount, DownvoteCount, UserName, Email, PostedOn)
	VALUES(@QuestionId, @Content, 0, 0, @UserName, @Email, GETDATE())
END
GO
/****** Object:  StoredProcedure [dbo].[spCreateComment]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spCreateComment]
	@PostId INT,
	@PostType VARCHAR(10),
	@Content VARCHAR(MAX),
	@UserName VARCHAR(20),
	@Email VARCHAR(50)
AS
BEGIN
	INSERT INTO Comment(PostId, PostType, Content, UpvoteCount, UserName, Email, PostedOn)
	VALUES(@PostId, @PostType, @Content, 0, @UserName, @Email, GETDATE())
END
GO
/****** Object:  StoredProcedure [dbo].[spCreateQuestion]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spCreateQuestion]
	@Title VARCHAR(50),
	@Description VARCHAR(MAX),
	@UserName VARCHAR(20),
	@Email VARCHAR(50)
AS
BEGIN
	INSERT INTO Question(Title, Description, UpvoteCount, DownvoteCount, UserName, Email, PostedOn)
	VALUES(@Title, @Description, 0, 0, @UserName, @Email, GETDATE())
END
GO
/****** Object:  StoredProcedure [dbo].[spCreateUser]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spCreateUser]
	@UserName VARCHAR(20),
	@Email VARCHAR(50),
	@Password VARCHAR(50)
AS
BEGIN
	INSERT INTO [User](UserName, Email, Password, RegisteredOn)
	VALUES(@UserName, @Email, @Password, GETDATE())
END
GO
/****** Object:  StoredProcedure [dbo].[spDownvoteAnswer]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spDownvoteAnswer]
	@AnswerId INT
AS
BEGIN
	UPDATE Answer SET DownvoteCount = DownvoteCount + 1
	WHERE Id = @AnswerId
END
GO
/****** Object:  StoredProcedure [dbo].[spDownvoteQuestion]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spDownvoteQuestion]
	@QuestionId INT
AS
BEGIN
	UPDATE Question SET DownvoteCount = DownvoteCount + 1
	WHERE Id = @QuestionId
END
GO
/****** Object:  StoredProcedure [dbo].[spFindQuestion]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spFindQuestion]
	@SearchTerm VARCHAR(50)
AS
BEGIN
	SELECT * FROM Question WHERE LOWER(Title) LIKE CONCAT('%', LOWER(@SearchTerm), '%') OR LOWER(Description) LIKE CONCAT('%', LOWER(@SearchTerm), '%')
END
GO
/****** Object:  StoredProcedure [dbo].[spGetAnswer]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spGetAnswer]
	@AnswerId INT
AS
BEGIN
	SELECT * FROM Answer WHERE Id = @AnswerId
END
GO
/****** Object:  StoredProcedure [dbo].[spGetQuestion]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spGetQuestion]
	@QuestionId INT
AS
BEGIN
	SELECT * FROM Question WHERE Id = @QuestionId
END
GO
/****** Object:  StoredProcedure [dbo].[spGetQuestionAndAnswerComments]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spGetQuestionAndAnswerComments]
	@QuestionId INT
AS
BEGIN
	SELECT * FROM Comment WHERE (PostType = 'Question' AND PostId = @QuestionId) OR (PostType = 'Answer' AND PostId IN (SELECT Id FROM Answer WHERE QuestionId = @QuestionId))
END
GO
/****** Object:  StoredProcedure [dbo].[spGetQuestionAnswers]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spGetQuestionAnswers]
	@QuestionId INT
AS
BEGIN
	SELECT * FROM Answer WHERE QuestionId = @QuestionId
END
GO
/****** Object:  StoredProcedure [dbo].[spGetQuestions]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spGetQuestions]
AS
BEGIN
	SELECT Question.*, ISNULL(QuestionAnswerCountData.AnswerCount, 0) AnswerCount
	FROM Question 
	LEFT JOIN (SELECT QuestionId, COUNT(Id) AnswerCount FROM Answer GROUP BY QuestionId) QuestionAnswerCountData ON Question.Id = QuestionAnswerCountData.QuestionId
END
GO
/****** Object:  StoredProcedure [dbo].[spGetUser]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spGetUser]
	@UserName VARCHAR(20),
	@Password VARCHAR(50)
AS
BEGIN
	SELECT * FROM [User] WHERE UserName = @UserName AND Password = @Password
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateAnswer]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spUpdateAnswer]
	@AnswerId INT,
	@Content VARCHAR(MAX)
AS
BEGIN
	UPDATE Answer
	SET Content = @Content, UpdatedOn = GETDATE()
	WHERE Id = @AnswerId
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateComment]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spUpdateComment]
	@CommentId INT,
	@Content VARCHAR(MAX)
AS
BEGIN
	UPDATE Comment
	SET Content = @Content, UpdatedOn = GETDATE()
	WHERE Id = @CommentId
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateQuestion]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spUpdateQuestion]
	@QuestionId INT,
	@Title VARCHAR(50),
	@Description VARCHAR(MAX)
AS
BEGIN
	UPDATE Question
	SET Title = @Title, Description = @Description, UpdatedOn = GETDATE()
	WHERE Id = @QuestionId
END
GO
/****** Object:  StoredProcedure [dbo].[spUpvoteAnswer]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spUpvoteAnswer]
	@AnswerId INT
AS
BEGIN
	UPDATE Answer SET UpvoteCount = UpvoteCount + 1
	WHERE Id = @AnswerId
END
GO
/****** Object:  StoredProcedure [dbo].[spUpvoteComment]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spUpvoteComment]
	@CommentId INT
AS
BEGIN
	UPDATE Comment SET UpvoteCount = UpvoteCount + 1
	WHERE Id = @CommentId
END
GO
/****** Object:  StoredProcedure [dbo].[spUpvoteQuestion]    Script Date: 11/07/24 11:06:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spUpvoteQuestion]
	@QuestionId INT
AS
BEGIN
	UPDATE Question SET UpvoteCount = UpvoteCount + 1
	WHERE Id = @QuestionId
END
GO
USE [master]
GO
ALTER DATABASE [DemoForum] SET  READ_WRITE 
GO
