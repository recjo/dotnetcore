using System;
using System.Linq;
using System.Collections.Generic;
using ecom.Entities;

namespace ecom.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private StoreDbContext _context;

        public OrderRepository(StoreDbContext context)
        {
            _context = context;
        }

        public void AddOrder(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void AddOrderDetail(OrderDetails orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
            _context.SaveChanges();
        }

        public Orders GetOrder(string orderNumber)
        {
            return _context.Orders.Where(w => w.OrderNumber == orderNumber).Select(p => p).FirstOrDefault();
        }

        public List<OrderDetails> GetOrderItems(string orderNumber)
        {
            return _context.OrderDetails.Where(w => w.OrderNumber == orderNumber).Select(p => p).ToList();
        }
    }
}