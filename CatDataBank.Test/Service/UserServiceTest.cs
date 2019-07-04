using System;
using AutoFixture;
using CatDataBank.DataAccess;
using CatDataBank.Helper;
using CatDataBank.Model;
using CatDataBank.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace CatDataBank.Test.Service
{
    public class UserServiceTest
    {
        private Fixture _fixture;
        private readonly Mock<IUserDataAccess> _userDataAccess;
        private readonly Mock<ITokenHandler> _tokenHandler;
        private readonly Mock<UserService> _userService;
        public UserServiceTest()
        {
            _fixture = new Fixture();
            _userDataAccess = new Mock<IUserDataAccess>();
            _tokenHandler = new Mock<ITokenHandler>();
            _userService = new Mock<UserService>(_userDataAccess.Object, _tokenHandler.Object);
        }

        [Fact]
        public void Authenticate_Success()
        {
            //Arrange
            var email = _fixture.Create<string>();
            var user = _fixture.Create<User>();
            var token = _fixture.Create<string>();
            _userDataAccess.Setup(a => a.GetUserByEmail(email)).Returns(user);
            _userService.Setup(a => a.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            _tokenHandler.Setup(a => a.GenerateToken(user.Id)).Returns(token);

            //Act
            var result = _userService.Object.Authenticate(email, _fixture.Create<string>());

            //Assert
            _userDataAccess.Verify(a => a.GetUserByEmail(email), Times.Once);
            _userService.Verify(a => a.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
            _tokenHandler.Verify(a => a.GenerateToken(user.Id), Times.Once);
            result.Should().Be(token);

        }

        [Fact]
        public void Authenticate_EmailEmpty()
        {
            //Arrange
            var email = String.Empty;

            //Act
            var result = _userService.Object.Authenticate(email, _fixture.Create<string>());

            //Assert
            _userDataAccess.Verify(a => a.GetUserByEmail(email), Times.Never);
            _userService.Verify(a => a.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);
            _tokenHandler.Verify(a => a.GenerateToken(It.IsAny<int>()), Times.Never);
            result.Should().BeNull();
        }

        [Fact]
        public void Authenticate_UserNull()
        {
            //Arrange
            var email = _fixture.Create<string>();

            //Act
            var result = _userService.Object.Authenticate(email, _fixture.Create<string>());

            //Assert
            _userDataAccess.Verify(a => a.GetUserByEmail(email), Times.Once);
            _userService.Verify(a => a.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);
            _tokenHandler.Verify(a => a.GenerateToken(It.IsAny<int>()), Times.Never);
            result.Should().BeNull();
        }
    }
}