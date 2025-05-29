using AutoFixture;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Yum.Components;
using Yum.Components.Pages.CartPages;
using Yum.Data;
using Yum.Repositories.Interfaces;
using Yum.Services.Interfaces;

namespace YumTest.ComponentTests
{
    public class CartTests : TestContext
    {
        private TestContext _context;
        private IRenderedComponent<Cart> _cut;
        private Fixture _fixture;
        private readonly string SomeSuccessMessage;
        private readonly string SomeErrorMessage = "Error updating item in cart";
        private IEnumerable<CartLineItem> SomeLineItems = new List<CartLineItem>();
        private bool Authenticated;
        private Mock<ICartLineItemRepository> _mockCartLineRepo;
        private Mock<ICartService> _mockCartService;
        
        public CartTests()
        {
            _context = new TestContext();
            _fixture = new Fixture();
            _mockCartLineRepo = new Mock<ICartLineItemRepository>();
            _mockCartService = new Mock<ICartService>();
            var mockOrderService = new Mock<IOrderService>();

            _context.Services.AddScoped<ICartService>(_ => _mockCartService.Object);
            _context.Services.AddScoped<ICartLineItemRepository>(_ => _mockCartLineRepo.Object);
            _context.Services.AddScoped<IOrderService>(_ => mockOrderService.Object);
        }

        [Fact]
        public void DisplaysEmptyCartMessage()
        {
            GivenUserAuthenticated();
            WhenRenderComponent();
            ThenDisplaysEmptyCart();
        }

        [Fact]
        public void NavigatesUnauthenticatedUserToLogin()
        {
            GivenUserNotAuthenticated();
            WhenRenderComponent();
            ThenNavigateToLogin();
        }

        [Fact]
        public void RendersCartLineItems()
        {
            GivenUserAuthenticated();
            GivenCartLineItems();
            WhenRenderComponent();
            ThenDisplaysCartLineItems();
        }

        [Fact]
        public void ShowsErrorToastr()
        {
            GivenUserAuthenticated();
            GivenCartLineItems();
            GivenRenderedComponent();
            WhenIncrementItem();
            ThenShowErrorToastrInvokedOnce();
        }

        [Fact]
        public void ShowProductAddedSuccessToastr()
        {
            GivenUserAuthenticated();
            GivenCartLineItems();
            GivenUpdateCartWillSucceed();
            GivenRenderedComponent();
            WhenIncrementItem();
            ThenShowItemAddedToastrInvokedOnce();
        }

        [Fact]
        public void ShowProductRemovedSuccessToastr()
        {
            GivenUserAuthenticated();
            GivenCartLineItems();
            GivenUpdateCartWillSucceed();
            GivenRenderedComponent();
            WhenDecrementItem();
            ThenShowItemRemovedToastrInvokedOnce();
        }

        private void GivenUserAuthenticated() 
        {
            _context.AddTestAuthorization().SetAuthorized("auth");
            Authenticated = true;
        }

        private void GivenUserNotAuthenticated()
        {
            _context.AddTestAuthorization();
            Authenticated = false;
        }

        private void GivenCartLineItems()
        {
            SomeLineItems = _fixture.Build<CartLineItem>()
                .With(i => i.Product, _fixture.Build<Product>()
                    .With(p => p.Price, 1).Create())
                .CreateMany(5);
            _mockCartLineRepo.Setup(x => x.GetAllUserItemsAsync(It.IsAny<string>())).ReturnsAsync(SomeLineItems);
            _context.Services.AddScoped<ICartLineItemRepository>(_ => _mockCartLineRepo.Object);
        }

        private void GivenRenderedComponent()
        {
            _context.JSInterop.SetupVoid("showToastr", _ => true);
            _cut = _context.RenderComponent<Cart>(parameters => parameters
                .Add(p => p.isAuthenticated, Authenticated)
            );
        }

        private void GivenUpdateCartWillSucceed()
        {
            _mockCartService.Setup(x => x.UpdateCartAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _context.Services.AddScoped<ICartService>(_ => _mockCartService.Object);
        }

        private void WhenRenderComponent()
        {
            _cut = _context.RenderComponent<Cart>(parameters => parameters
                .Add(p => p.isAuthenticated, Authenticated)
            );
        }

        private void WhenIncrementItem()
        {
            var button = _cut.Find("i.bi-plus-circle-fill");
            button.Click();
        }

        private void WhenDecrementItem()
        {
            var button = _cut.Find("i.bi-dash-circle-fill");
            button.Click();
        }

        private void ThenDisplaysEmptyCart()
        {
            var alertDiv = _cut.Find("div.alert");
            Assert.True(alertDiv.TextContent.Contains("Your cart is empty!"));
        }

        private void ThenNavigateToLogin()
        {
            var navMan = _context.Services.GetRequiredService<FakeNavigationManager>();
            Assert.Equal("http://localhost/account/login", navMan.Uri);
        }

        private void ThenDisplaysCartLineItems()
        {
            var components = _cut.FindComponents<CartItem>();
            Assert.Equal(SomeLineItems.Count(), components.Count);
        }

        private void ThenShowErrorToastrInvokedOnce()
        {
            var invocations = _context.JSInterop.Invocations["showToastr"];
            Assert.Single(invocations);
            Assert.Equal(SomeErrorMessage, invocations[0].Arguments[1]);
        }

        private void ThenShowItemAddedToastrInvokedOnce()
        {
            var expectedErrorMessage = $"{SomeLineItems.First().Product.Name} added to cart";
            var invocations = _context.JSInterop.Invocations["showToastr"];
            Assert.Single(invocations);
            Assert.Equal(expectedErrorMessage, invocations[0].Arguments[1]);
        }

        private void ThenShowItemRemovedToastrInvokedOnce()
        {
            var expectedErrorMessage = $"{SomeLineItems.First().Product.Name} removed from cart";
            var invocations = _context.JSInterop.Invocations["showToastr"];
            Assert.Single(invocations);
            Assert.Equal("success", invocations[0].Arguments[0]);
            Assert.Equal(expectedErrorMessage, invocations[0].Arguments[1]);
        }
    }
}
