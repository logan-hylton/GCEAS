using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;

namespace EMT_Class_Server
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public CustomAuthenticationStateProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly string _connectionString;
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        public void AuthenticateUser(string email, string password)
        {
            SqlConnection cnn;
            SqlCommand sqlCommand;
            SqlDataReader reader;
            string sql = $"select Password from Users u where u.Email = '{email}';";
            string hashedPassword;
            cnn = new SqlConnection(_connectionString);
            try
            {
                cnn.Open();
                sqlCommand = new SqlCommand(sql, cnn);
                reader = sqlCommand.ExecuteReader();
                reader.Read();
                hashedPassword = reader.GetString(0);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured retrieving password from the database.", ex);
            }
            finally { cnn.Close(); }


            var hasher = new PasswordHasher<string>();
            var result = hasher.VerifyHashedPassword(email, hashedPassword, password);

            if (result != PasswordVerificationResult.Success)
            {
                throw new Exception("Login failed.");
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, email),
            }, "Custom Authentication");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
