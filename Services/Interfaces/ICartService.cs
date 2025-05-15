namespace Yum.Services.Interfaces
{
    public interface ICartService
    {
        public Task<bool> UpdateCartAsync(string userId, int productId, int updateByAmount);
    }
}
