using AngleSharp.Common;
using AutoFixture;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Yum.Components;
using Yum.Components.Pages;
using Yum.Data;
using Yum.Data.Models;
using Yum.Repositories.Interfaces;
using Yum.Services.Interfaces;

namespace YumTest.PageTests
{
    public class HomeTests : TestContext
    {
        private TestContext _context;
        private IRenderedComponent<Home> _cut;
        private Fixture _fixture;
        private Mock<IProductRepository> _mockProductRepo;
        private Mock<ICategoryRepository> _mockCategoryRepo;
        private List<Product> SomeProducts;
        private IEnumerable<Category> SomeCategories;

        public HomeTests()
        {
            _context = new TestContext();
            _fixture = new Fixture();
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            var mockCartService = new Mock<ICartService>();

            _context.Services.AddScoped<IProductRepository>(_ => _mockProductRepo.Object);
            _context.Services.AddScoped<ICategoryRepository>(_ => _mockCategoryRepo.Object);
            _context.Services.AddScoped<ICartService>(_ => mockCartService.Object);
            //_context.ComponentFactories.AddStub<ProductTile>();
        }

        [Fact]
        private void DisplaysCategories()
        {
            GivenCategories();
            WhenComponentRendered();
            ThenDisplaysCategoryNavItems();
        }

        [Fact]
        private void DisplaysInitialProducts()
        {
            GivenCategories();
            GivenProducts();
            WhenComponentRendered();
            ThenDisplaysProductTiles(SomeProducts.GetRange(0,3));
        }

        [Fact]
        private void DisplaysPageNavigationLinks()
        {
            GivenCategories();
            GivenProducts();
            WhenComponentRendered();
            ThenDisplaysPageNavigationLinks(3);
        }

        [Fact]
        private void DisplaysANextNavLinkOnFirstRender()
        {
            GivenCategories();
            GivenProducts();
            WhenComponentRendered();
            ThenDisplaysNextNavLink();
        }

        [Fact]
        private void DoesNotDisplayAPreviousNavLinkOnFirstRender()
        {
            GivenCategories();
            GivenProducts();
            WhenComponentRendered();
            ThenDoesNotDisplayPreviousNavLink();
        }

        [Fact]
        private void NavigatesToLastPage()
        {
            GivenCategories();
            GivenProducts();
            GivenComponentRendered();
            WhenTargetPageLinkClicked("3");
            ThenDisplaysLastProduct();
        }

        [Fact]
        private void NavigatesToNextPage()
        {
            GivenCategories();
            GivenProducts();
            GivenComponentRendered();
            WhenTargetPageLinkClicked("Next");
            ThenDisplaysProductTiles(SomeProducts.GetRange(3, 3));
        }

        [Fact]
        private void NavigatesToPreviousPage()
        {
            GivenCategories();
            GivenProducts();
            GivenComponentRendered();
            WhenTargetPageLinkClicked("Next");
            WhenTargetPageLinkClicked("Previous");
            ThenDisplaysProductTiles(SomeProducts.GetRange(0, 3));
        }

        [Fact]
        private void DoesNotDisplayNextLinkOnLastPage()
        {
            GivenCategories();
            GivenProducts();
            GivenComponentRendered();
            WhenTargetPageLinkClicked("3");
            ThenDoesNotDisplayNextNavLink();
        }

        private void GivenCategories()
        {
            SomeCategories = _fixture.CreateMany<Category>(7);
            _mockCategoryRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(SomeCategories);
            _context.Services.AddScoped<ICategoryRepository>(_ => _mockCategoryRepo.Object);
        }

        private void GivenProducts()
        {
            SomeProducts = new List<Product>();
            foreach (var cat in SomeCategories)
            {
                var product = _fixture.Build<Product>()
                    .With(p => p.Category, cat)
                    .With(p => p.CategoryId, cat.Id)
                    .With(p => p.Price, 2)
                    .Create();
                SomeProducts.Add(product);
            }
            _mockProductRepo.Setup(x => x.GetProductsQueryable(null, null)).Returns(SomeProducts.AsQueryable());
            _context.Services.AddScoped<IProductRepository>(_ => _mockProductRepo.Object);
        }

        private void GivenComponentRendered() => WhenComponentRendered();

        private void WhenComponentRendered()
        {
            _cut = _context.RenderComponent<Home>();
        }

        private void WhenTargetPageLinkClicked(string linkText)
        {

            var targetButton = _cut.FindAll("button.page-link").Single(btn => btn.TextContent.Trim() == linkText);
            targetButton.Click();
        }

        private void ThenDisplaysCategoryNavItems()
        {
            var navItems = _cut.FindAll("li.nav-item");
            Assert.Equal(8, navItems.Count);
        }

        private void ThenDisplaysProductTiles(List<Product> expectedProducts)
        {
            var productTiles = _cut.FindComponents<ProductTile>();
            Assert.Equal(expectedProducts.Count, productTiles.Count);

            for (int i = 0; i < expectedProducts.Count; i++) 
            {
                Assert.Equal(expectedProducts.GetItemByIndex(i), productTiles.GetItemByIndex(i).Instance.Product);
            }
        }

        private void ThenDisplaysPageNavigationLinks(int expectedPageLinkCount)
        {
            var pageLinks = _cut.FindAll("button.page-link");
            for (int i = 1; i <= expectedPageLinkCount; i++)
            {
                Assert.Single(pageLinks.Where(x => x.TextContent.Trim() == i.ToString()));
            }
        }

        private void ThenDisplaysNextNavLink()
        {
            var targetEl = _cut.FindAll("button.page-link").Single(btn => btn.TextContent.Trim() == "Next");
            Assert.NotNull(targetEl);
        }

        private void ThenDoesNotDisplayPreviousNavLink()
        {
            var targetEl = _cut.FindAll("button.page-link").SingleOrDefault(btn => btn.TextContent.Trim() == "Previous");
            Assert.Null(targetEl);
        }

        private void ThenDoesNotDisplayNextNavLink()
        {
            var targetEl = _cut.FindAll("button.page-link").SingleOrDefault(btn => btn.TextContent.Trim() == "Next");
            Assert.Null(targetEl);
        }

        private void ThenDisplaysLastProduct()
        {
            var productTiles = _cut.FindComponents<ProductTile>();
            Assert.Single(productTiles);
            Assert.Equal(SomeProducts.Last(), productTiles.Single().Instance.Product);
        }
    }
}
