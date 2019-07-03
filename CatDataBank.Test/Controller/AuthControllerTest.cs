using System;
using AutoFixture;
using CatDataBank.Controllers;
using CatDataBank.DataAccess;
using CatDataBank.Helper;
using CatDataBank.Model;
using CatDataBank.Model.Dto;
using CatDataBank.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CatDataBank.Test.Controller
{
    public class AuthControllerTest
    {
        private Fixture _fixture = new Fixture();
        private Mock<IUserService> _userService;
        private Mock<IAutoMapperProfile> _mapper;
        private Mock<AuthController> _authController;

        public AuthControllerTest()
        {
            _userService = new Mock<IUserService>();
            _mapper = new Mock<IAutoMapperProfile>();

            _authController = new Mock<AuthController>(_userService.Object, _mapper.Object);
        }

        [Fact]
        public void Authenticate_Success()
        {
            //Arrange
            var userDto = _fixture.Create<UserDto>();
            var token = _fixture.Create<string>();
            var obj = new
            {
                Username = userDto.Email,
                Token = token
            };
            var response = _authController.Object.Success(obj);
            _userService.Setup(a => a.Authenticate(userDto.Email, userDto.Password)).Returns(token);
            _authController.Setup(a => a.Success(obj));

            //Act
            var result = _authController.Object.Authenticate(userDto);

            //Assert
            _userService.Verify(a => a.Authenticate(userDto.Email, userDto.Password), Times.Once);
            result.Should().Be(response);
        }

        [Fact]
        public void Authenticate_TokenNull()
        {
            //Arrange
            var userDto = _fixture.Create<UserDto>();
            _userService.Setup(a => a.Authenticate(userDto.Email, userDto.Password)).Returns((string) null);

            //Act
            var result = _authController.Object.Authenticate(userDto);

            //Assert
            _userService.Verify(a => a.Authenticate(userDto.Email, userDto.Password), Times.Once);
            result.Should().Be(_authController.Object.Error(It.IsAny<object>()));
        }

        [Fact]
        public void Authenticate_Null()
        {
            //Arrange
            var userDto = _fixture.Create<UserDto>();
            var response = _authController.Object.InternalError();
            _userService.Setup(a => a.Authenticate(userDto.Email, userDto.Password)).Throws(new Exception());
            _authController.Setup(a => a.InternalError()).Returns(response);

            //Act
            var result = _authController.Object.Authenticate(userDto);

            //Assert
            _userService.Verify(a => a.Authenticate(userDto.Email, userDto.Password), Times.Once);
            result.Should().Be(response);
        }
    }
}