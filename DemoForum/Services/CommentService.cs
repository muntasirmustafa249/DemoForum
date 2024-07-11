using DemoForum.Models;
using DemoForum.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DemoForum.Services
{
    public class CommentService
    {
        private readonly string connectionString;
        public CommentService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<Comment> GetComment(int commentId)
        {
            Comment comment = new();

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sql = $"SELECT * FROM Comment WHERE Id = {commentId}";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            comment.Id = reader.GetInt32("Id");
                            comment.PostId = reader.GetInt32("PostId");
                            comment.PostType = reader.GetString("PostType");
                            comment.Content = reader.GetString("Content");
                            comment.UpvoteCount = reader.GetInt32("UpvoteCount");
                            comment.UserName = reader.GetString("UserName");
                            comment.Email = reader.GetString("Email");
                            comment.PostedOn = reader.GetDateTime("PostedOn");
                            if (!reader.IsDBNull("UpdatedOn")) comment.UpdatedOn = reader.GetDateTime("UpdatedOn");
                        }
                        await reader.CloseAsync();

                        await command.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            return await Task.FromResult(comment);
        }

        public async Task InsertComment(NewCommentModel comment)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spCreateComment";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@PostId", comment.PostId));
                        command.Parameters.Add(new SqlParameter("@PostType", comment.PostType));
                        command.Parameters.Add(new SqlParameter("@Content", comment.Content));
                        command.Parameters.Add(new SqlParameter("@UserName", comment.UserName));
                        command.Parameters.Add(new SqlParameter("@Email", comment.Email));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        public async Task UpdateComment(EditCommentModel comment)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spUpdateComment";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@CommentId", comment.Id));
                        command.Parameters.Add(new SqlParameter("@Content", comment.Content));

                        await command.ExecuteNonQueryAsync();

                        await command.DisposeAsync();
                    }

                    await connection.CloseAsync();
                }
            }

            await Task.CompletedTask;
        }

        public async Task UpvoteComment(int commentId)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string spName = "spUpvoteComment";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@CommentId", commentId));

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
