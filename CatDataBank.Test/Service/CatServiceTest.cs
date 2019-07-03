using System;
using AutoFixture;
using CatDataBank.DataAccess;
using CatDataBank.Helper;
using CatDataBank.Model;
using CatDataBank.Service;
using FluentAssertions;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CatDataBank.Test.Service
{
    public class CatServiceTest
    {
         private Fixture _fixture;
        private readonly Mock<ICatDataAccess> _catDataAccess;
        private readonly Mock<CatService> _catService;
        public CatServiceTest()
        {
            _fixture = new Fixture();
            _catDataAccess = new Mock<ICatDataAccess>();
            _catService = new Mock<CatService>(_catDataAccess.Object);
        }

        [Fact]
        public void GetCats_Success()
        {
            //Arrange
            var cats = _fixture.Create<IEnumerable<Cat>>();
            _catDataAccess.Setup(a => a.GetAllCats()).Returns(cats);

            //Act
            var result = _catService.Object.GetCats();

            //Assert
            _catDataAccess.Verify(a => a.GetAllCats(), Times.Once);
            result.Should().BeSameAs(cats);
        }

        [Fact]
        public void AddCats_Success()
        {
            //Arrange
            var cats = _fixture.Create<IEnumerable<Cat>>();
            _catDataAccess.Setup(a => a.AddCats(It.IsAny<IEnumerable<Cat>>()));
            _catDataAccess.Setup(a => a.Commit());

            //Act
            _catService.Object.AddCats(cats.ToArray());

            //Assert
            _catDataAccess.Verify(a => a.AddCats(cats), Times.Once);
            _catDataAccess.Verify(a => a.Commit(), Times.Once);
        }
    }
}