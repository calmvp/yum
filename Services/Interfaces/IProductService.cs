namespace Yum.Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> DeleteProductAsync(int productId);
    }
}
