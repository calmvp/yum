namespace Yum.Services.Interfaces
{
    public interface ISharedStateService
    {
        public event Action OnCartCountChange;
        public int TotalCartCount { get; set; }
    }
}
