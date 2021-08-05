//using System.Collections.Generic;
//using AutoMapper;
//using CustomerLibCore.Api.Controllers;
//using CustomerLibCore.Api.Dtos;
//using CustomerLibCore.Api.Exceptions;
//using CustomerLibCore.Domain.Models;
//using CustomerLibCore.Domain.Enums;
//using CustomerLibCore.ServiceLayer.Services;
//using CustomerLibCore.TestHelpers;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Xunit;

//namespace CustomerLibCore.Api.Tests.Controllers
//{
//    public class AddressesControllerTest
//    {
//        #region Constructors

//        [Fact]
//        public void ShouldCreateAddressesController()
//        {
//            var mockAddressService = new StrictMock<IAddressService>();
//            var mockMapper = new StrictMock<IMapper>();

//            var controller = new AddressesController(mockAddressService.Object, mockMapper.Object);

//            Assert.NotNull(controller);
//        }

//        #endregion

//        #region Find all by customer ID

//        [Fact]
//        public void ShouldThrowOnFindAllForCustomerWhenProvidedBadId()
//        {
//            // Given
//            var customerId = 0;

//            var controller = new AddressesControllerFixture().CreateController();

//            // When
//            var exception = Assert.Throws<RouteArgumentException>(() =>
//                controller.FindAllForCustomer(customerId));

//            // Then
//            Assert.Equal("customerId", exception.ParamName);
//            Assert.Equal("ID cannot be less than 1", exception.Message);
//        }

//        [Fact]
//        public void ShouldFindAllForCustomerEmtpy()
//        {
//            // Given
//            var mapper = CreateMapper();
//            var customerId = 5;

//            var addresses = new List<Address>();
//            var addressesDto = mapper.Map<IEnumerable<AddressDto>>(addresses);

//            var fixture = new AddressesControllerFixture();
//            fixture.MockAddressService.Setup(s => s.FindAllForCustomer(customerId))
//                .Returns(addresses);
//            fixture.MockMapper.Setup(m => m.Map<IEnumerable<AddressDto>>(addresses))
//                .Returns(addressesDto);

//            var controller = fixture.CreateController();

//            // When
//            var result = controller.FindAllForCustomer(customerId).Result;

//            //Then
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(200, okResult.StatusCode);

//            var value = okResult.Value;
//            var items = Assert.IsAssignableFrom<IEnumerable<AddressDto>>(value);
//            Assert.Empty(items);

//            fixture.MockAddressService.Verify(s => s.FindAllForCustomer(customerId), Times.Once);
//            fixture.MockMapper.Verify(m => m.Map<IEnumerable<AddressDto>>(addresses), Times.Once);
//        }

//        [Fact]
//        public void ShouldFindAllForCustomer()
//        {
//            // Given
//            var mapper = CreateMapper();
//            var customerId = 5;

//            var addresses = new List<Address>() { MockAddress(7, customerId),
//                MockAddress(9, customerId) };
//            var addressesDto = mapper.Map<IEnumerable<AddressDto>>(addresses);

//            var fixture = new AddressesControllerFixture();
//            fixture.MockAddressService.Setup(s => s.FindAllForCustomer(customerId))
//                .Returns(addresses);
//            fixture.MockMapper.Setup(m => m.Map<IEnumerable<AddressDto>>(addresses))
//                .Returns(addressesDto);

//            var controller = fixture.CreateController();

//            // When
//            var result = controller.FindAllForCustomer(customerId).Result;

//            //Then
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(200, okResult.StatusCode);

//            var value = okResult.Value;
//            var items = Assert.IsAssignableFrom<IEnumerable<AddressDto>>(value);
//            Assert.Equal(addressesDto, items);

//            fixture.MockAddressService.Verify(s => s.FindAllForCustomer(customerId), Times.Once);
//            fixture.MockMapper.Verify(m => m.Map<IEnumerable<AddressDto>>(addresses), Times.Once);
//        }

//        #endregion

//        #region Get single for customer

//        [Theory]
//        [InlineData(0, 1, "customerId")]
//        [InlineData(1, 0, "addressId")]
//        public void ShouldThrowOnGetSingleForCustomerWhenProvidedBadIds(
//            int customerId, int addressId, string paramName)
//        {
//            // Given
//            var controller = new AddressesControllerFixture().CreateController();

//            // When
//            var exception = Assert.Throws<RouteArgumentException>(() =>
//                controller.GetForCustomer(customerId, addressId));

//            // Then
//            Assert.Equal(paramName, exception.ParamName);
//            Assert.Equal("ID cannot be less than 1", exception.Message);
//        }

//        [Fact]
//        public void ShouldGetSingleForCustomer()
//        {
//            // Given
//            var mapper = CreateMapper();
//            var addressId = 3;
//            var customerId = 5;

//            var address = MockAddress(addressId, customerId);
//            var addressDto = mapper.Map<AddressDto>(address);

//            var fixture = new AddressesControllerFixture();
//            fixture.MockAddressService.Setup(s => s.GetForCustomer(addressId, customerId))
//                .Returns(address);
//            fixture.MockMapper.Setup(m => m.Map<AddressDto>(address)).Returns(addressDto);

//            var controller = fixture.CreateController();

//            // When
//            var result = controller.GetForCustomer(customerId, addressId).Result;

//            //Then
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(200, okResult.StatusCode);

//            var value = okResult.Value;
//            var item = Assert.IsAssignableFrom<AddressDto>(value);
//            Assert.Equal(addressDto, item);

//            fixture.MockAddressService.Verify(s => s.GetForCustomer(addressId, customerId),
//                Times.Once);
//            fixture.MockMapper.Verify(m => m.Map<AddressDto>(address), Times.Once);
//        }

//        #endregion

//        #region Save

//        [Fact]
//        public void ShouldThrowOnSaveWhenProvidedBadId()
//        {
//            // Given
//            var customerId = 0;
//            var addressDto = new AddressDto();

//            var controller = new AddressesControllerFixture().CreateController();

//            // When
//            var exception = Assert.Throws<RouteArgumentException>(() =>
//                controller.Save(customerId, addressDto));

//            // Then
//            Assert.Equal("customerId", exception.ParamName);
//            Assert.Equal("ID cannot be less than 1", exception.Message);
//        }

//        [Fact]
//        public void ShouldThrowOnSaveWhenProvidedInvalidBody()
//        {
//            // Given
//            var mapper = CreateMapper();
//            var customerId = 5;

//            var badAddressDto = MockAddressDto();
//            badAddressDto.City = " ";

//            var controller = new AddressesControllerFixture().CreateController();

//            // When
//            var errors = Assert.Throws<InvalidBodyException>(() =>
//                controller.Save(customerId, badAddressDto)).Errors;

//            //Then
//            var error = Assert.Single(errors);

//            Assert.Equal(nameof(AddressDto.City), error.PropertyName);
//        }

//        [Fact]
//        public void ShouldSave()
//        {
//            // Given
//            var mapper = CreateMapper();
//            var customerId = 5;

//            var addressDto = MockAddressDto();
//            var address = mapper.Map<Address>(addressDto);

//            var fixture = new AddressesControllerFixture();
//            fixture.MockMapper.Setup(m => m.Map<Address>(addressDto)).Returns(address);
//            fixture.MockAddressService.Setup(s => s.Save(address));

//            var controller = fixture.CreateController();

//            // When
//            var result = controller.Save(customerId, addressDto);

//            //Then
//            var okResult = Assert.IsType<OkResult>(result);
//            Assert.Equal(200, okResult.StatusCode);

//            fixture.MockAddressService.Verify(s => s.Save(address), Times.Once);
//            fixture.MockAddressService.Verify(s =>
//                s.Save(It.Is<Address>(n => n.CustomerId == customerId)), Times.Once);
//            fixture.MockMapper.Verify(m => m.Map<Address>(addressDto), Times.Once);
//        }

//        #endregion

//        #region Update

//        [Theory]
//        [InlineData(0, 1, "customerId")]
//        [InlineData(1, 0, "addressId")]
//        public void ShouldThrowOnUpdateWhenProvidedBadIds(
//            int customerId, int addressId, string paramName)
//        {
//            // Given
//            var addressDto = new AddressDto();

//            var controller = new AddressesControllerFixture().CreateController();

//            // When
//            var exception = Assert.Throws<RouteArgumentException>(() =>
//                controller.Update(customerId, addressId, addressDto));

//            // Then
//            Assert.Equal(paramName, exception.ParamName);
//            Assert.Equal("ID cannot be less than 1", exception.Message);
//        }

//        [Fact]
//        public void ShouldThrowOnUpdateWhenProvidedInvalidBody()
//        {
//            // Given
//            var mapper = CreateMapper();
//            var addressId = 3;
//            var customerId = 5;

//            var badAddressDto = MockAddressDto();
//            badAddressDto.City = " ";

//            var controller = new AddressesControllerFixture().CreateController();

//            // When
//            var errors = Assert.Throws<InvalidBodyException>(() =>
//                controller.Update(customerId, addressId, badAddressDto)).Errors;

//            //Then
//            var error = Assert.Single(errors);

//            Assert.Equal(nameof(AddressDto.City), error.PropertyName);
//        }

//        [Fact]
//        public void ShouldUpdate()
//        {
//            // Given
//            var mapper = CreateMapper();
//            var addressId = 3;
//            var customerId = 5;

//            var addressDto = MockAddressDto();
//            var address = mapper.Map<Address>(addressDto);

//            var fixture = new AddressesControllerFixture();
//            fixture.MockMapper.Setup(m => m.Map<Address>(addressDto)).Returns(address);
//            fixture.MockAddressService.Setup(s => s.UpdateForCustomer(address));

//            var controller = fixture.CreateController();

//            // When
//            var result = controller.Update(customerId, addressId, addressDto);

//            //Then
//            var okResult = Assert.IsType<OkResult>(result);
//            Assert.Equal(200, okResult.StatusCode);

//            fixture.MockAddressService.Verify(s => s.UpdateForCustomer(address), Times.Once);
//            fixture.MockAddressService.Verify(s => s.UpdateForCustomer(It.Is<Address>(n =>
//                n.AddressId == addressId && n.CustomerId == customerId)), Times.Once);
//            fixture.MockMapper.Verify(m => m.Map<Address>(addressDto), Times.Once);
//        }

//        #endregion

//        #region Delete

//        [Theory]
//        [InlineData(0, 1, "customerId")]
//        [InlineData(1, 0, "addressId")]
//        public void ShouldThrowOnDeleteWhenProvidedBadIds(
//            int customerId, int addressId, string paramName)
//        {
//            // Given
//            var controller = new AddressesControllerFixture().CreateController();

//            // When
//            var exception = Assert.Throws<RouteArgumentException>(() =>
//                controller.Delete(customerId, addressId));

//            // Then
//            Assert.Equal(paramName, exception.ParamName);
//            Assert.Equal("ID cannot be less than 1", exception.Message);
//        }

//        [Fact]
//        public void ShouldDelete()
//        {
//            // Given
//            var addressId = 3;
//            var customerId = 5;

//            var fixture = new AddressesControllerFixture();
//            fixture.MockAddressService.Setup(s => s.DeleteForCustomer(addressId, customerId));

//            var controller = fixture.CreateController();

//            // When
//            var result = controller.Delete(customerId, addressId);

//            //Then
//            var okResult = Assert.IsType<OkResult>(result);
//            Assert.Equal(200, okResult.StatusCode);

//            fixture.MockAddressService.Verify(s => s.DeleteForCustomer(addressId, customerId),
//                Times.Once);
//        }

//        #endregion

//        #region Fixture, object mock helpers

//        public class AddressesControllerFixture
//        {
//            public StrictMock<IAddressService> MockAddressService { get; set; }
//            public StrictMock<IMapper> MockMapper { get; set; }

//            public AddressesControllerFixture()
//            {
//                MockAddressService = new();
//                MockMapper = new();
//            }

//            public AddressesController CreateController() =>
//                new(MockAddressService.Object, MockMapper.Object);
//        }

//        public static IMapper CreateMapper()
//        {
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile(new AutoMapperApiProfile());
//            });

//            return config.CreateMapper();
//        }

//        public static Address MockAddress(int addressId, int customerId) => new()
//        {
//            AddressId = addressId,
//            CustomerId = customerId,
//            Line = "line one",
//            Line2 = "line two",
//            Type = AddressType.Shipping,
//            City = "city x",
//            PostalCode = "3241",
//            State = "state x",
//            Country = "Canada"
//        };

//        public static AddressDto MockAddressDto() => new()
//        {
//            Line = "line one",
//            Line2 = "line two",
//            Type = "Shipping",
//            City = "city x",
//            PostalCode = "3241",
//            State = "state x",
//            Country = "Canada"
//        };

//        #endregion
//    }
//}
