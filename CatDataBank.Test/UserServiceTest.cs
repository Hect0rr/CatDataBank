using Xunit;
using CatDataBank.Model;
using Microsoft.AspNetCore.Http;
using CatDataBank.Helper;
using CatDataBank.Service;
using Moq;
using Microsoft.Extensions.Options;
using AutoFixture;
using System;
using FluentAssertions;

namespace CatDataBankTest.Test
{
    public class UserServiceTest
    {
        private Fixture _fixture;
        private readonly Mock<AppDbContext> _appDbContext;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly IOptions<AppSettings> _appSettingsOptions;
        private readonly Mock<UserService> _userService;
        public UserServiceTest()
        {
            _fixture = new Fixture();
            _appDbContext = new Mock<AppDbContext>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _userService = new Mock<UserService>(_appDbContext.Object, _httpContextAccessor.Object, _appSettingsOptions);
        }

        [Fact]
        public void Authenticate_Success()
        {
            //Arrange
            var email = _fixture.Create<string>();
            _appDbContext.Object.Users.Add(_fixture.Build<User>().With(a => a.Email, email).Create());
            _appDbContext.Object.SaveChanges();
            _userService.Setup(a => a.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);

            //Act
            Action action = () => _userService.Object.Authenticate(email, _fixture.Create<string>());

            //Assert
            action.Should().NotBeNull();
        }
    }
}