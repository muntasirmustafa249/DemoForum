using DemoForum.Models;
using DemoForum.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DemoForum.Services
{
    public class QuestionService
    {
        private readonly string connectionString;
        public QuestionService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<Question>> GetQuestions()
        {
            List<Question> questions = [];

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM Question";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Question question = new()
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.GetString("Description"),
                                UpvoteCount = reader.GetInt32("UpvoteCount"),
                                DownvoteCount = reader.GetInt32("DownvoteCount"),
                                UserName = reader.GetString("UserName"),
                                Email = reader.GetString("Email"),
                                PostedOn = reader.GetDateTime("PostedOn")
                            };
                            if (!reader.IsDBNull("UpdatedOn")) question.UpdatedOn = reader.GetDateTime("UpdatedOn");

                            questions.Add(question);
                        }
                        await reader.CloseAsync();

                        await command.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            return await Task.FromResult(questions);
        }

        public async Task<List<QuestionModel>> GetQuestionsForView()
        {
            List<QuestionModel> questions = [];

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string spName = "spGetQuestions";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            QuestionModel question = new()
                            {
                                Id = reader.GetInt32("Id"),
                                Title = reader.GetString("Title"),
                                Description = reader.GetString("Description"),
                                UpvoteCount = reader.GetInt32("UpvoteCount"),
                                DownvoteCount = reader.GetInt32("DownvoteCount"),
                                UserName = reader.GetString("UserName"),
                                PostedOn = reader.GetDateTime("PostedOn"),
                                AnswerCount = reader.GetInt32("AnswerCount")
                            };
                            if (!reader.IsDBNull("UpdatedOn")) question.UpdatedOn = reader.GetDateTime("UpdatedOn");

                            questions.Add(question);
                        }
                        await reader.CloseAsync();

                        await command.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            return await Task.FromResult(questions);
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            Question question = new();

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string spName = "spGetQuestion";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@QuestionId", questionId));

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            question.Id = reader.GetInt32("Id");
                            question.Title = reader.GetString("Title");
                            question.Description = reader.GetString("Description");
                            question.UpvoteCount = reader.GetInt32("UpvoteCount");
                            question.DownvoteCount = reader.GetInt32("DownvoteCount");
                            question.UserName = reader.GetString("UserName");
                            question.Email = reader.GetString("Email");
                            question.PostedOn = reader.GetDateTime("PostedOn");
                            if (!reader.IsDBNull("UpdatedOn")) question.UpdatedOn = reader.GetDateTime("UpdatedOn");
                        }
                        await reader.CloseAsync();

                        await command.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            return await Task.FromResult(question);
        }

        public async Task<QuestionDetailModel> GetQuestionDetails(int questionId)
        {
            QuestionDetailModel questionDetail = new();

            Question questionData = new();
            List<Answer> questionAnswers = [];
            List<Comment> questionAndAnswerComments = [];

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string sp1Name = "spGetQuestion";

                    using (var command1 = new SqlCommand(sp1Name, connection))
                    {
                        command1.CommandType = CommandType.StoredProcedure;

                        command1.Parameters.Clear();
                        command1.Parameters.Add(new SqlParameter("@QuestionId", questionId));

                        SqlDataReader reader = await command1.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            questionData.Id = reader.GetInt32("Id");
                            questionData.Title = reader.GetString("Title");
                            questionData.Description = reader.GetString("Description");
                            questionData.UpvoteCount = reader.GetInt32("UpvoteCount");
                            questionData.DownvoteCount = reader.GetInt32("DownvoteCount");
                            questionData.UserName = reader.GetString("UserName");
                            questionData.Email = reader.GetString("Email");
                            questionData.PostedOn = reader.GetDateTime("PostedOn");
                            if (!reader.IsDBNull("UpdatedOn")) questionData.UpdatedOn = reader.GetDateTime("UpdatedOn");
                        }
                        await reader.CloseAsync();

                        await command1.DisposeAsync();
                    }

                    string sp2Name = "spGetQuestionAnswers";

                    using (var command2 = new SqlCommand(sp2Name, connection))
                    {
                        command2.CommandType = CommandType.StoredProcedure;

                        command2.Parameters.Clear();
                        command2.Parameters.Add(new SqlParameter("@QuestionId", questionId));

                        SqlDataReader reader = await command2.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Answer answer = new()
                            {
                                Id = reader.GetInt32("Id"),
                                Content = reader.GetString("Content"),
                                UpvoteCount = reader.GetInt32("UpvoteCount"),
                                DownvoteCount = reader.GetInt32("DownvoteCount"),
                                UserName = reader.GetString("UserName"),
                                Email = reader.GetString("Email"),
                                PostedOn = reader.GetDateTime("PostedOn")
                            };
                            if (!reader.IsDBNull("UpdatedOn")) answer.UpdatedOn = reader.GetDateTime("UpdatedOn");

                            questionAnswers.Add(answer);
                        }
                        await reader.CloseAsync();

                        await command2.DisposeAsync();
                    }

                    string sp3Name = "spGetQuestionAndAnswerComments";

                    using (var command3 = new SqlCommand(sp3Name, connection))
                    {
                        command3.CommandType = CommandType.StoredProcedure;

                        command3.Parameters.Clear();
                        command3.Parameters.Add(new SqlParameter("@QuestionId", questionId));

                        SqlDataReader reader = await command3.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Comment comment = new()
                            {
                                Id = reader.GetInt32("Id"),
                                PostId = reader.GetInt32("PostId"),
                                PostType = reader.GetString("PostType"),
                                Content = reader.GetString("Content"),
                                UpvoteCount = reader.GetInt32("UpvoteCount"),
                                UserName = reader.GetString("UserName"),
                                Email = reader.GetString("Email"),
                                PostedOn = reader.GetDateTime("PostedOn")
                            };
                            if (!reader.IsDBNull("UpdatedOn")) comment.UpdatedOn = reader.GetDateTime("UpdatedOn");

                            questionAndAnswerComments.Add(comment);
                        }
                        await reader.CloseAsync();

                        await command3.DisposeAsync();
                    }

                    await connection.CloseAsync();

                    questionDetail = await PrepareQuestionDetailData(questionData, questionAnswers, questionAndAnswerComments);
                }
            }

            return await Task.FromResult(questionDetail);
        }

        public async Task InsertQuestion(NewQuestionModel question)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spCreateQuestion";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@Title", question.Title));
                        command.Parameters.Add(new SqlParameter("@Description", question.Description));
                        command.Parameters.Add(new SqlParameter("@UserName", question.UserName));
                        command.Parameters.Add(new SqlParameter("@Email", question.Email));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        public async Task UpdateQuestion(EditQuestionModel question)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spUpdateQuestion";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@QuestionId", question.Id));
                        command.Parameters.Add(new SqlParameter("@Title", question.Title));
                        command.Parameters.Add(new SqlParameter("@Description", question.Description));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        public async Task UpvoteQuestion(int questionId)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spUpvoteQuestion";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@QuestionId", questionId));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        public async Task DownvoteQuestion(int questionId)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spDownvoteQuestion";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@QuestionId", questionId));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        private async Task<QuestionDetailModel> PrepareQuestionDetailData(Question question, List<Answer> questionAnswers, List<Comment> questionAndAnswerComments)
        {
            QuestionDetailModel questionDetail = new()
            {
                Id = question.Id,
                Title = question.Title,
                Description = question.Description,
                UpvoteCount = question.UpvoteCount,
                DownvoteCount = question.DownvoteCount,
                UserName = question.UserName,
                PostedOn = question.PostedOn,
                UpdatedOn = question.UpdatedOn
            };

            var answers = questionAnswers.FindAll(a => a.QuestionId == question.Id);

            if (answers.Count > 0) questionDetail.Answers = answers.Select(ans => new AnswerDetailModel
            {
                Id = ans.Id,
                QuestionId = ans.QuestionId,
                Content = ans.Content,
                UpvoteCount = ans.UpvoteCount,
                DownvoteCount = ans.DownvoteCount,
                UserName = ans.UserName,
                PostedOn = ans.PostedOn,
                UpdatedOn = ans.UpdatedOn,
                AnswerComments = []
            }).ToList();

            var questionComments = questionAndAnswerComments.FindAll(c => c.PostType == "Question" && c.PostId == question.Id);

            if (questionComments.Count > 0) questionDetail.QuestionComments = questionComments.Select(c => new CommentDetailModel
            {
                Id = c.Id,
                PostId = c.PostId,
                PostType = c.PostType,
                Content = c.Content,
                UpvoteCount = c.UpvoteCount,
                UserName = c.UserName,
                PostedOn = c.PostedOn,
                UpdatedOn = c.UpdatedOn
            }).ToList();

            foreach (var answer in questionDetail.Answers)
            {
                var answerComments = questionAndAnswerComments.FindAll(c => c.PostType == "Answer" && c.PostId == answer.Id);

                if (answerComments.Count > 0) answer.AnswerComments = answerComments.Select(c => new CommentDetailModel
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    PostType = c.PostType,
                    Content = c.Content,
                    UpvoteCount = c.UpvoteCount,
                    UserName = c.UserName,
                    PostedOn = c.PostedOn,
                    UpdatedOn = c.UpdatedOn
                }).ToList();
            }

            return await Task.FromResult(questionDetail);
        }
    }
}
