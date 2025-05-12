using Yum.Data;

namespace Yum.Services
{
    public class StateContainerService
    {
        public Order Value { get; set; } = new Order();

        public event Action OnStateChange;

        public void SetValue(Order value)
        {
            Value = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
