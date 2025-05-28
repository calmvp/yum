using Yum.Data;

namespace Yum.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> CreateAsync(Product Product);
        public Task<Product> UpdateAsync(Product Product);
        public Task<bool> DeleteAsync(Product product);
        public Task<Product> GetByIdAsync(int productId);
        public Task<IEnumerable<Product>> GetAllAsync();
        IQueryable<Product> GetProductsQueryable(int? categoryId = null, string? searchString = null);
    }
}
