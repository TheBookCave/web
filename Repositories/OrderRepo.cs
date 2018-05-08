using web.Data;
using System.Collections.Generic;
using System.Linq;
using web.Models.ViewModels;
using System;
using web.Models.InputModels;
using web.Data.EntityModels;

namespace web.Repositories
{
    public class OrderRepo
    {
        private DataContext _db;

        public OrderRepo(DataContext context) {
            _db = context;
        }

// Function that returns a list of all the genres in a database
        public List<OrderListViewModel> GetAllOrders()
        {
            var orders = (from o in _db.Orders
                         select new OrderListViewModel
                         {
                             Id = o.Id,
                             CustomerId = o.CustomerId,
                             BillingAddressId = o.BillingAddressId,
                             ShippingAddressId = o.ShippingAddressId,
                             Status = o.Status,
                             OrderDate = o.OrderDate, 
                             ShippingDate = o.ShippingDate,
                             PurchaseAmount = o.PurchaseAmount
                         }).ToList();
            return orders;
        }


        public void AddOrder(OrderInputModel inputOrder)
        {
            var newOrder = new Order()
            {
                Id = inputOrder.Id,
                CustomerId = inputOrder.CustomerId,
                BillingAddressId = inputOrder.BillingAddressId,
                ShippingAddressId = inputOrder.ShippingAddressId,
                Status = inputOrder.Status,
                OrderDate = inputOrder.OrderDate, 
                ShippingDate = inputOrder.ShippingDate,
                PurchaseAmount = inputOrder.PurchaseAmount
            };

            _db.Orders.Add(newOrder);
            _db.SaveChanges();
        }

        public OrderDetailViewModel GetOrderWithId(int Id)
        {
            var order = (from o in _db.Orders
                        select new OrderDetailViewModel
                        {
                            Id = o.Id,
                            CustomerId = o.CustomerId,
                            BillingAddressId = o.BillingAddressId,
                            ShippingAddressId = o.ShippingAddressId,
                            Status = o.Status,
                            OrderDate = o.OrderDate, 
                            ShippingDate = o.ShippingDate,
                            PurchaseAmount = o.PurchaseAmount
                        }).FirstOrDefault();
            return order;
        }

        
    }
}