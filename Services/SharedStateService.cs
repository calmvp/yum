using Yum.Data;
using Yum.Services.Interfaces;

namespace Yum.Services
{
    public class SharedStateService
    {
        private int _totalCartCount;

        public event Action OnCartCountChange;

        public int TotalCartCount
        {
            get => _totalCartCount;
            set
            {
                _totalCartCount = value;
                NotifyCartCountChanged();
            }
        }
        private void NotifyCartCountChanged() => OnCartCountChange?.Invoke();
    }
}
