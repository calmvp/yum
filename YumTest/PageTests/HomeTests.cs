using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yum.Components;
using Yum.Components.Pages;
using Yum.Data;
using Yum.Data.Models;
using Yum.Repositories.Interfaces;

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

            _context.Services.AddScoped<IProductRepository>(_ => _mockProductRepo.Object);
            _context.Services.AddScoped<ICategoryRepository>(_ => _mockCategoryRepo.Object);
            _context.ComponentFactories.AddStub<ProductTile>();
        }

        [Fact]
        private void DisplaysCategories()
        {
            GivenCategories();
            WhenComponentRendered();
            ThenDisplaysCategoryNavItems();
        }

        [Fact]
        private void DisplaysProducts()
        {
            GivenCategories();
            GivenProducts();
            WhenComponentRendered();
            ThenDisplaysProductTiles(3);
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

        private void WhenComponentRendered()
        {
            _cut = _context.RenderComponent<Home>();
        }

        private void ThenDisplaysCategoryNavItems()
        {
            var navItems = _cut.FindAll("li.nav-item");
            Assert.Equal(8, navItems.Count);
        }

        private void ThenDisplaysProductTiles(int expectedTileCount)
        {
            var productTiles = _cut.FindComponents<Stub<ProductTile>>();
            Assert.Equal(expectedTileCount, productTiles.Count);
        }

    }
}
