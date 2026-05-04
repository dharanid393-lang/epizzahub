
    // PSEUDOCODE / PLAN:
    // - Create a test class `CartControllerTests` using xUnit.
    // - Use Moq to mock `ICartService` and `ILogger<CartController>`.
    // - Instantiate `CartController` with mocked dependencies.
    // - Write unit tests covering:
    //   1) GetItemCount:
    //      - returns BadRequest when empty Guid passed.
    //      - returns NotFound when service returns negative count.
    //      - returns Ok with value when service returns non-negative count.
    //   2) AddItems:
    //      - returns Ok with boolean result and verifies service called with expected DTO.
    //   3) GetCartDetail:
    //      - returns BadRequest when empty Guid passed.
    //      - returns Ok with `CartResponseDto` when service returns a DTO.
    //   4) UpdateCartUser:
    //      - returns Ok with updated cart id (or integer result) and verifies call.
    //   5) DeleteItem:
    //      - returns Ok with boolean and verifies service called correctly.
    //   6) UpdateItem:
    //      - returns Ok with boolean and verifies service called correctly.
    // - Use `Assert.IsType<T>` to validate response types and assert response values.
    // - Use Moq `Verify` to ensure mocked service methods are invoked with expected arguments.

    using System;
    using System.Threading.Tasks;
    using Xunit;
    using Moq;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Mvc;
    using ePizza.API.Controllers;
    using ePizza.Application.Contracts;
    using ePizza.Application.DTOs.Request;
    using ePizza.Application.DTOs.Response;
    using global::ePizza.Application.Contracts;

    namespace ePizza.API.Tests
    {
        public class CartControllerTests
        {
            private readonly Mock<ICartService> _cartServiceMock;
            private readonly Mock<ILogger<CartController>> _loggerMock;
            private readonly CartController _controller;

            public CartControllerTests()
            {
                _cartServiceMock = new Mock<ICartService>();
                _loggerMock = new Mock<ILogger<CartController>>();
                _controller = new CartController(_cartServiceMock.Object, _loggerMock.Object);
            }

            [Fact]
            public async Task GetItemCount_ReturnsBadRequest_WhenCartIdEmpty()
            {
                // Act
                var result = await _controller.GetItemCount(Guid.Empty);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.Equal("cartId is required.", badRequest.Value);
            }

            [Fact]
            public async Task GetItemCount_ReturnsNotFound_WhenServiceReturnsNegative()
            {
            // Arrange  -- in this arrange part we are setting up the things to be tested and dependencies will be mocked
            var cartId = Guid.NewGuid();
                _cartServiceMock.Setup(s => s.GetItemCountAsync(cartId)).ReturnsAsync(-1);

            // Act-- in this part we are calling the method to be tested
            var result = await _controller.GetItemCount(cartId);

            // Assert -- matching the result with expected outcome
            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
                Assert.Equal("Cart not found.", notFound.Value);
            }

            [Fact]
            public async Task GetItemCount_ReturnsOkWithCount_WhenServiceReturnsNonNegative()
            {
                // Arrange
                var cartId = Guid.NewGuid();
                int expectedCount = 3;
                _cartServiceMock.Setup(s => s.GetItemCountAsync(cartId)).ReturnsAsync(expectedCount);

                // Act
                var result = await _controller.GetItemCount(cartId);

                // Assert
                var ok = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(expectedCount, ok.Value);
            }

            [Fact]
            public async Task AddItems_ReturnsOkAndCallsService()
            {
                // Arrange
                var request = new AddItemsDto
                {
                    UserId = 1,
                    CartId = Guid.NewGuid(),
                    ItemId = 10,
                    UnitPrice = 9.99m,
                    Quantity = 2
                };

                _cartServiceMock.Setup(s => s.AddItemsAsync(It.IsAny<AddItemsDto>())).ReturnsAsync(true);

                // Act
                var result = await _controller.AddItems(request);

                // Assert
                var ok = Assert.IsType<OkObjectResult>(result.Result);
                Assert.True((bool)ok.Value);

                _cartServiceMock.Verify(s =>
                    s.AddItemsAsync(It.Is<AddItemsDto>(r =>
                        r.CartId == request.CartId &&
                        r.ItemId == request.ItemId &&
                        r.Quantity == request.Quantity &&
                        r.UnitPrice == request.UnitPrice &&
                        r.UserId == request.UserId)), Times.Once);
            }

            [Fact]
            public async Task GetCartDetail_ReturnsBadRequest_WhenCartIdEmpty()
            {
                // Act
                var result = await _controller.GetCartDetail(Guid.Empty);

                // Assert
                var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.Equal("cartId is required.", badRequest.Value);
            }

            [Fact]
            public async Task GetCartDetail_ReturnsOkWithCartDto_WhenServiceReturnsDto()
            {
                // Arrange
                var cartId = Guid.NewGuid();
                var responseDto = new CartResponseDto
                {
                    CartId = cartId,
                    UserId = 1,
                    CreatedDate = DateTime.UtcNow,
                    Total = 20m,
                    Tax = 2m,
                    GrantTotal = 22m,
                    CartItems = new System.Collections.Generic.List<CartItemsResponseDto>()
                };

                _cartServiceMock.Setup(s => s.GetCartDetailsAsync(cartId)).ReturnsAsync(responseDto);

                // Act
                var result = await _controller.GetCartDetail(cartId);

                // Assert
                var ok = Assert.IsType<OkObjectResult>(result.Result);
                Assert.Equal(responseDto, ok.Value);
            }

            [Fact]
            public async Task UpdateCartUser_ReturnsOkAndCallsService()
            {
                // Arrange
                var request = new UpdateCartUserDto
                {
                    CartId = Guid.NewGuid(),
                    UserId = 42
                };

                _cartServiceMock.Setup(s => s.UpdateCartUserAsync(request)).ReturnsAsync(1);

                // Act
                var result = await _controller.UpdateCartUser(request);

                // Assert
                var ok = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(1, ok.Value);

                _cartServiceMock.Verify(s => s.UpdateCartUserAsync(request), Times.Once);
            }

            [Fact]
            public async Task DeleteItem_ReturnsOkAndCallsService()
            {
                // Arrange
                var request = new DeleteItemFromCartRequestDto
                {
                    CartId = Guid.NewGuid(),
                    ItemId = 5
                };

                _cartServiceMock.Setup(s => s.DeleteItemFromCartAsync(request.CartId, request.ItemId)).ReturnsAsync(true);

                // Act
                var result = await _controller.DeleteItem(request);

                // Assert
                var ok = Assert.IsType<OkObjectResult>(result);
                Assert.True((bool)ok.Value);

                _cartServiceMock.Verify(s => s.DeleteItemFromCartAsync(request.CartId, request.ItemId), Times.Once);
            }

            [Fact]
            public async Task UpdateItem_ReturnsOkAndCallsService()
            {
                // Arrange
                var request = new UpdateCartItemRequestDto
                {
                    CartId = Guid.NewGuid(),
                    ItemId = 7,
                    Quantity = 4
                };

                _cartServiceMock.Setup(s => s.UpdateItemInCartAsync(request.CartId, request.ItemId, request.Quantity)).ReturnsAsync(true);

                // Act
                var result = await _controller.UpdateItem(request);

                // Assert
                var ok = Assert.IsType<OkObjectResult>(result);
                Assert.True((bool)ok.Value);

                _cartServiceMock.Verify(s => s.UpdateItemInCartAsync(request.CartId, request.ItemId, request.Quantity), Times.Once);
            }
        }
    }

