using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WongaApi.Data;
using WongaApi.DTOs;
using WongaApi.Services;
using Xunit;

namespace WongaApi.Tests
{
    public class AuthServiceTest
    {
        private static AppDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private static IConfiguration CreateConfig()
        {
            var dict = new Dictionary<string, string?>
            {
                ["JWT_SECRET"] = "ThisIsATestSecretKeyThatIsLongEnoughToSignJwtTokens1234567890"
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(dict)
                .Build();
        }

        private static AuthService CreateService(AppDbContext db)
        {
            return new AuthService(db, CreateConfig());
        }

        [Fact]
        public async Task RegisterAsync_NewUser_ReturnsToken()
        {
            using var db = CreateDb();
            var service = CreateService(db);

            var token = await service.RegisterAsync(new RegisterRequest
            {
                FirstName = "Miki",
                LastName = "Pathan",
                Email = "miki@test.com",
                Password = "Test123!"
            });

            Assert.False(string.IsNullOrWhiteSpace(token));
            Assert.Equal(1, await db.Users.CountAsync());
        }

        [Fact]
        public async Task RegisterAsync_DuplicateEmail_ReturnsNull()
        {
            using var db = CreateDb();
            var service = CreateService(db);

            var req = new RegisterRequest
            {
                FirstName = "Miki",
                LastName = "Pathan",
                Email = "miki@test.com",
                Password = "Test123!"
            };

            var token1 = await service.RegisterAsync(req);
            var token2 = await service.RegisterAsync(req);

            Assert.NotNull(token1);
            Assert.Null(token2);
            Assert.Equal(1, await db.Users.CountAsync());
        }

        [Fact]
        public async Task LoginAsync_CorrectPassword_ReturnsToken()
        {
            using var db = CreateDb();
            var service = CreateService(db);

            await service.RegisterAsync(new RegisterRequest
            {
                FirstName = "Miki",
                LastName = "Pathan",
                Email = "miki@test.com",
                Password = "Test123!"
            });

            var token = await service.LoginAsync(new LoginRequest
            {
                Email = "miki@test.com",
                Password = "Test123!"
            });

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            using var db = CreateDb();
            var service = CreateService(db);

            await service.RegisterAsync(new RegisterRequest
            {
                FirstName = "Miki",
                LastName = "Pathan",
                Email = "miki@test.com",
                Password = "Test123!"
            });

            var token = await service.LoginAsync(new LoginRequest
            {
                Email = "miki@test.com",
                Password = "WrongPass!"
            });

            Assert.Null(token);
        }
    }
}