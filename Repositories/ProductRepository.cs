using Yum.Data.Models;
using Yum.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Yum.Repositories.Interfaces;

namespace Yum.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            if (product is not null)
            {
                _db.Products.Remove(product);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product is not null)
            {
                return product;
            }

            return new Product();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.Include(p => p.Category).ToListAsync();
        }

        public IQueryable<Product> GetProductsQueryable(int? categoryId = null, string? searchString = null)
        {
            IQueryable<Product> query = _db.Products;
            if (categoryId is null && searchString is null)
            {
                return query;
            }

            if (categoryId is not null)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            if (searchString is not null)
            {
                query = query.Where(x => x.Name.Contains(searchString));
            }

            return query;
        }


        public async Task<Product> UpdateAsync(Product product)
        {
            var prodFromDb = await _db.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (prodFromDb is not null)
            {
                prodFromDb.Name = product.Name;
                prodFromDb.Description = product.Description;
                prodFromDb.Price = product.Price;
                prodFromDb.SpecialTag = product.SpecialTag;
                prodFromDb.CategoryId = product.CategoryId;
                prodFromDb.ImageUrl = product.ImageUrl;
                _db.Products.Update(prodFromDb);
                await _db.SaveChangesAsync();
                return prodFromDb;
            }
            return product;
        }
    }
}
