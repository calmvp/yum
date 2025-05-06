using Yum.Repositories.Interfaces;
using Yum.Services.Interfaces;

namespace Yum.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepo;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductService(IProductRepository productRepo, IWebHostEnvironment webHostEnvironment) 
        {
            _productRepo = productRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);

            if (product is null)
                return false;

            if (!string.IsNullOrEmpty(product.ImageUrl)) 
            {
                DeleteProductImage(product.ImageUrl);
            }

            try
            {
                var res = await _productRepo.DeleteAsync(product);
                return res;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        private void DeleteProductImage(string imageUrl)
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(imagePath)) { 
                File.Delete(imagePath);
            }
        }
    }
}
