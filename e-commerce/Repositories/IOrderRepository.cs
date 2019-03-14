using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public interface IOrderRepository
    {
        void AddOrder(Orders order);
        void AddOrderDetail(OrderDetails orderDetailsEntity);
        Orders GetOrder(string orderNumber);
        List<OrderDetails> GetOrderItems(string orderNumber);
    }
}