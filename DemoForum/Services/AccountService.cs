using DemoForum.Models;
using DemoForum.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace DemoForum.Services
{
    public class AccountService
    {
        private readonly string connectionString;
        public AccountService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<string> GenerateLoginToken(string userName, string emailAddress)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("6f91f332-c191-4f29-9626-c0e74f271824"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var listClaims = new List<Claim>()
            {
                //new Claim(JwtRegisteredClaimNames.Sub, "Muntasir"),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                //new Claim(ClaimTypes.NameIdentifier,  userName),
                //new Claim(ClaimTypes.Name, userName ),
                //new Claim(ClaimTypes.Email, emailAddress ),
                new Claim("UserName", userName),
                new Claim("EmailAddress", emailAddress)
            };


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "MuntasirMustafa",
                Audience = "MuntasirMustafa",
                SigningCredentials = credentials,
                Subject = new ClaimsIdentity(listClaims),
                //NotBefore = dateCreate,
                Expires = DateTime.Now.AddDays(1),
            };
            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
            var securityToken = handler.CreateToken(tokenDescriptor);

            return await Task.FromResult(securityToken);
        }

        public async Task<TokenDataModel> GetTokenData(string loginToken)
        {
            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidIssuer = "MuntasirMustafa",
                ValidAudience = "MuntasirMustafa",
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("6f91f332-c191-4f29-9626-c0e74f271824"))
            };

            var tokenHandler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();

            var validationResult = await tokenHandler.ValidateTokenAsync(loginToken, tokenValidationParameter);

            TokenDataModel tokenData = new TokenDataModel();

            tokenData.IsTokenValid = validationResult.IsValid;

            if (validationResult.IsValid)
            {
                tokenData.UserName = validationResult.ClaimsIdentity.FindFirst("UserName")?.Value ?? "";
                tokenData.EmailAddress = validationResult.ClaimsIdentity.FindFirst("EmailAddress")?.Value ?? "";
            }

            return await Task.FromResult(tokenData);
        }

        public async Task<bool> CheckUserForRegistration(RegistrationModel registrationData)
        {
            bool userExists = false;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string spName = "spCheckUser";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@UserName", registrationData.UserName));
                        command.Parameters.Add(new SqlParameter("@Email", registrationData.Email));

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows) userExists = true;

                        await reader.CloseAsync();

                        await command.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            return await Task.FromResult(userExists);
        }

        public async Task<User> CheckUserForLogin(LoginModel loginData)
        {
            User user = new();

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string spName = "spGetUser";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@UserName", loginData.UserName));
                        command.Parameters.Add(new SqlParameter("@Password", loginData.Password));

                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            user.Id = reader.GetInt32("Id");
                            user.UserName = reader.GetString("Name");
                            user.Email = reader.GetString("Email");
                            user.Password = reader.GetString("Password");
                            user.RegisteredOn = reader.GetDateTime("RegisteredOn");
                        }
                        await reader.CloseAsync();

                        await command.DisposeAsync();
                    }
                    await connection.CloseAsync();
                }
            }

            return await Task.FromResult(user);
        }

        public async Task CreateUser(RegistrationModel registrationData)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string spName = "spCreateUser";

                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@UserName", registrationData.UserName));
                        command.Parameters.Add(new SqlParameter("@Email", registrationData.Email));
                        command.Parameters.Add(new SqlParameter("@Password", registrationData.Password));

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
