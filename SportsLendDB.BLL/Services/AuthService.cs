using Microsoft.Extensions.Logging;
using SportsLendDB.BLL.Interfaces;
using SportsLendDB.BO.Models;
using SportsLendDB.DAL.Repositories;
using SportsLendDB.DAL.Repositories.Interfaces;
using System.Text;

namespace SportsLendDB.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger _logger;
        private readonly IGenericRepository<User> _userRepository;

        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;
            _userRepository ??= new GenericRepository<User>();
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            try
            {
                _logger.LogInformation($"Attempting login for user {email}.");
                byte[] passwordHash;

                if (password.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    string hex = password.Substring(2);
                    int numberChars = hex.Length;
                    passwordHash = new byte[numberChars / 2];
                    for (int i = 0; i < numberChars; i += 2)
                        passwordHash[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                }
                else
                {
                    passwordHash = Encoding.UTF8.GetBytes(password);
                }

                var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash && (u.IsActive ?? true));
                if (user != null)
                {
                    _logger.LogInformation($"User {email} logged in successfully.");
                    return user;
                }
                else
                {
                    _logger.LogWarning($"Login failed for user {email}. Invalid credentials or inactive account.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during login for user {email}.");
                throw;
            }
        }
    }
}
