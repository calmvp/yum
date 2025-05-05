using Yum.Data.Models;

namespace Yum.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<Category> CreateAsync(Category category);
        public Task<Category> UpdateAsync(Category category);
        public Task<bool> DeleteAsync(int categoryId);
        public Task<Category> GetByIdAsync(int categoryId);
        public Task<IEnumerable<Category>> GetAllAsync();
    }
}
