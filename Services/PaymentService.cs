using Microsoft.AspNetCore.Components;
using Stripe;
using Stripe.Checkout;
using Yum.Data;
using Yum.Repositories;
using Yum.Repositories.Interfaces;
using Yum.Services.Interfaces;

namespace Yum.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly NavigationManager _navManager;
        private readonly IOrderRepository _orderRepo;
        private readonly IConfiguration _config;

        public PaymentService(NavigationManager navManager, IOrderRepository orderRepo, IConfiguration config)
        {
            _navManager = navManager;
            _orderRepo = orderRepo;
            _config = config;
        }

        public Session CreateStripeCheckoutSession(Order order)
        {
            StripeConfiguration.ApiKey = _config["StripeApiKey"];



            IEnumerable<SessionLineItemOptions> lineItemOptions = order.Items.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    Currency = "usd",
                    UnitAmountDecimal = (decimal?)item.Price * 100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = item.ProductName
                    }
                },
                Quantity = item.ItemCount
            });

            var options = new SessionCreateOptions()
            {
                SuccessUrl = $"{_navManager.BaseUri}order/confirmation/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_navManager.BaseUri}cart",
                LineItems = lineItemOptions.ToList(),
                Mode = "payment"
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<Order> CheckPaymentStatusAndUpdateOrder(string sessionId)
        {
            Order order = await _orderRepo.GetBySessionIdAsync(sessionId);
            var service = new SessionService();
            var session = service.Get(sessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                await _orderRepo.UpdateOrderStatusAsync(order.OrderKey, Utility.OrderStatusEnum.Approved, session.PaymentIntentId);
            }
            return order;
        }
    }
}
