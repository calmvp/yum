using Stripe.Checkout;
using Yum.Data;

namespace Yum.Services.Interfaces
{
    public interface IPaymentService
    {
        public Session CreateStripeCheckoutSession(Order order);
        public Task<Order> CheckPaymentStatusAndUpdateOrder(string sessionId);
    }
}
