using Microsoft.EntityFrameworkCore;
using Yum.Data;
using Yum.Data.Models;
using Yum.Repositories.Interfaces;

namespace Yum.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category> CreateAsync(Category category) 
        { 
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
           var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category is not null) { 
                _db.Categories.Remove(category);
                return await _db.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Category> GetByIdAsync(int categoryId)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(category => category.Id == categoryId);
            if (category is not null) { 
                return category;
            }

            return new Category();
        }

        public async Task<IEnumerable<Category>> GetAllAsync() {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var catFromDb = await _db.Categories.FirstOrDefaultAsync(cat => cat.Id == category.Id);
            if (catFromDb is not null)
            {
                catFromDb.Name = category.Name;
                _db.Categories.Update(catFromDb);
                await _db.SaveChangesAsync();
                return catFromDb;
            }
            return category;
        }
    }
}
