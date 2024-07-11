using DemoForum.Models;
using DemoForum.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DemoForum.Services
{
    public class AnswerService
    {
        private readonly string connectionString;
        public AnswerService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<Answer>> GetAnswers()
        {
            List<Answer> answers = [];

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM Answer";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        SqlDataReader reader = await command.ExecuteReaderAsync();
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

                            answers.Add(answer);
                        }
                        await reader.CloseAsync();

                        await command.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            return await Task.FromResult(answers);
        }

        public async Task<Answer> GetAnswer(int answerId)
        {
            Answer answer = new();

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string spName = "spGetAnswer";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@AnswerId", answerId));

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            answer.Id = reader.GetInt32("Id");
                            answer.QuestionId = reader.GetInt32("QuestionId");
                            answer.Content = reader.GetString("Content");
                            answer.UpvoteCount = reader.GetInt32("UpvoteCount");
                            answer.DownvoteCount = reader.GetInt32("DownvoteCount");
                            answer.UserName = reader.GetString("UserName");
                            answer.Email = reader.GetString("Email");
                            answer.PostedOn = reader.GetDateTime("PostedOn");
                            if (!reader.IsDBNull("UpdatedOn")) answer.UpdatedOn = reader.GetDateTime("UpdatedOn");
                        }
                        await reader.CloseAsync();

                        await command.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            return await Task.FromResult(answer);
        }

        public async Task InsertAnswer(NewAnswerModel answer)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spCreateAnswer";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@QuestionId", answer.QuestionId));
                        command.Parameters.Add(new SqlParameter("@Content", answer.Content));
                        command.Parameters.Add(new SqlParameter("@UserName", answer.UserName));
                        command.Parameters.Add(new SqlParameter("@Email", answer.Email));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        public async Task UpdateAnswer(EditAnswerModel answer)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spUpdateAnswer";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@AnswerId", answer.Id));
                        command.Parameters.Add(new SqlParameter("@Content", answer.Content));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        public async Task UpvoteAnswer(int answerId)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spUpvoteAnswer";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@AnswerId", answerId));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        public async Task DownvoteAnswer(int answerId)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spDownvoteAnswer";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@AnswerId", answerId));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }
    }
}
