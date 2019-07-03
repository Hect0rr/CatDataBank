using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CatDataBank.Controllers;
using CatDataBank.Service;
using CatDataBank.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace CatDataBank.Test.Controller
{
    public class CatControllerTest
    {
        private Fixture _fixture = new Fixture();
        private readonly Mock<ICatService> _catService;
        private Mock<CatController> _catController;

        public CatControllerTest()
        {
            _catService = new Mock<ICatService>();
            _catController = new Mock<CatController>(_catService.Object);
        }

        [Fact]
        public void GetCats_Success()
        {
            //Arrange
            var cats = _fixture.Create<IEnumerable<Cat>>();
            _catService.Setup(a => a.GetCats()).Returns(cats);

            //Act
            var result = _catController.Object.GetCats();

            //Assert
            _catService.Verify(a => a.GetCats(), Times.Once);
            result.Should().Be(_catController.Object.Success(cats));
        }

        [Fact]
        public void GetCats_Error()
        {
            //Arrange
            _catService.Setup(a => a.GetCats()).Throws(new Exception());

            //Act
            var result = _catController.Object.GetCats();

            //Assert
            _catService.Verify(a => a.GetCats(), Times.Once);
            result.Should().Be(_catController.Object.Error(It.IsAny<object>()));
        }

        [Fact]
        public void AddCat_Success()
        {
            //Arrange
            var cats = _fixture.Create<Cat[]>();
            _catService.Setup(a => a.AddCats(cats));

            //Act
            var result = _catController.Object.AddCat(cats.ToArray());

            //Assert
            _catService.Verify(a => a.AddCats(cats), Times.Once);
            result.Should().Be(_catController.Object.Created());
        }

        [Fact]
        public void AddCat_Error()
        {
            //Arrange
            var cats = _fixture.Create<Cat[]>();
            _catService.Setup(a => a.AddCats(cats)).Throws(new Exception());

            //Act
            var result = _catController.Object.AddCat(cats.ToArray());

            //Assert
            _catService.Verify(a => a.AddCats(cats), Times.Once);
            result.Should().Be(_catController.Object.InternalError());
        }
    }
}