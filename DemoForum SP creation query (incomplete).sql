IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetQuestion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spGetQuestion
		@QuestionId INT
	AS
	BEGIN
		SELECT * FROM Question WHERE Id = @QuestionId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spCreateQuestion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spCreateQuestion
		@Title VARCHAR(50),
		@Description VARCHAR(MAX),
		@UserName VARCHAR(20),
		@Email VARCHAR(50)
	AS
	BEGIN
		INSERT INTO Question(Title, Description, UpvoteCount, DownvoteCount, UserName, Email, PostedOn)
		VALUES(@Title, @Description, 0, 0, @UserName, @Email, GETDATE())
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spUpdateQuestion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spUpdateQuestion
		@QuestionId INT,
		@Title VARCHAR(50),
		@Description VARCHAR(MAX)
	AS
	BEGIN
		UPDATE Question
		SET Title = @Title, Description = @Description, UpdatedOn = GETDATE()
		WHERE Id = @QuestionId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spUpvoteQuestion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spUpvoteQuestion
		@QuestionId INT
	AS
	BEGIN
		UPDATE Question SET UpvoteCount = UpvoteCount + 1
		WHERE Id = @QuestionId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spDownvoteQuestion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spDownvoteQuestion
		@QuestionId INT
	AS
	BEGIN
		UPDATE Question SET DownvoteCount = DownvoteCount + 1
		WHERE Id = @QuestionId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetAnswer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spGetAnswer
		@AnswerId INT
	AS
	BEGIN
		SELECT * FROM Answer WHERE Id = @AnswerId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spCreateAnswer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spCreateAnswer
		@QuestionId INT,
		@Content VARCHAR(MAX),
		@UserName VARCHAR(20),
		@Email VARCHAR(50)
	AS
	BEGIN
		INSERT INTO Answer(QuestionId, Content, UpvoteCount, DownvoteCount, UserName, Email, PostedOn)
		VALUES(@QuestionId, @Content, 0, 0, @UserName, @Email, GETDATE())
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spUpdateAnswer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spUpdateAnswer
		@AnswerId INT,
		@Content VARCHAR(MAX)
	AS
	BEGIN
		UPDATE Answer
		SET Content = @Content, UpdatedOn = GETDATE()
		WHERE Id = @AnswerId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spUpvoteAnswer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spUpvoteAnswer
		@AnswerId INT
	AS
	BEGIN
		UPDATE Answer SET UpvoteCount = UpvoteCount + 1
		WHERE Id = @AnswerId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spDownvoteAnswer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spDownvoteAnswer
		@AnswerId INT
	AS
	BEGIN
		UPDATE Answer SET DownvoteCount = DownvoteCount + 1
		WHERE Id = @AnswerId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spCreateComment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spCreateComment
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
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spUpdateComment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spUpdateComment
		@CommentId INT,
		@Content VARCHAR(MAX)
	AS
	BEGIN
		UPDATE Comment
		SET Content = @Content, UpdatedOn = GETDATE()
		WHERE Id = @CommentId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spUpvoteComment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spUpvoteComment
		@CommentId INT
	AS
	BEGIN
		UPDATE Comment SET UpvoteCount = UpvoteCount + 1
		WHERE Id = @CommentId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spGetUser
		@UserName VARCHAR(20),
		@Password VARCHAR(50)
	AS
	BEGIN
		SELECT * FROM [User] WHERE UserName = @UserName AND Password = @Password
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spCheckUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spCheckUser
		@UserName VARCHAR(20),
		@Email VARCHAR(50)
	AS
	BEGIN
		SELECT * FROM [User] WHERE UserName = @UserName OR Email = @Email
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spCreateUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spCreateUser
		@UserName VARCHAR(20),
		@Email VARCHAR(50),
		@Password VARCHAR(50)
	AS
	BEGIN
		INSERT INTO [User](UserName, Email, Password, RegisteredOn)
		VALUES(@UserName, @Email, @Password, GETDATE())
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetQuestionAnswers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spGetQuestionAnswers
		@QuestionId INT
	AS
	BEGIN
		SELECT * FROM Answer WHERE QuestionId = @QuestionId
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetQuestionAndAnswerComments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spGetQuestionAndAnswerComments
		@QuestionId INT
	AS
	BEGIN
		SELECT * FROM Comment WHERE (PostType = 'Question' AND PostId = @QuestionId) OR (PostType = 'Answer' AND PostId IN (SELECT Id FROM Answer WHERE QuestionId = @QuestionId))
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spFindQuestion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spFindQuestion
		@SearchTerm VARCHAR(50)
	AS
	BEGIN
		SELECT * FROM Question WHERE LOWER(Title) LIKE CONCAT('%', LOWER(@SearchTerm), '%') OR LOWER(Description) LIKE CONCAT('%', LOWER(@SearchTerm), '%')
	END
END

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetQuestions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	CREATE PROC spGetQuestions
	AS
	BEGIN
		SELECT Question.*, ISNULL(QuestionAnswerCountData.AnswerCount, 0) AnswerCount
		FROM Question 
		LEFT JOIN (SELECT QuestionId, COUNT(Id) AnswerCount FROM Answer GROUP BY QuestionId) QuestionAnswerCountData ON Question.Id = QuestionAnswerCountData.QuestionId
	END
END