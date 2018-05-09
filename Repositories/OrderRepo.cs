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
                        where o.Id == Id

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


        public int GetOpenOrderId(string userId)
        {
            var orderId = (from o in _db.Orders
                        where o.Status == "open"
                        where o.CustomerId == userId
                        select o.Id).ToList();
            if(orderId.Count() != 0) {
                return orderId[0];
            }
            else {
                return 0;

                //var neworderinput = new OrderInputModel() {
                    

                //}
               // _db.Orders.Add()
            }
        }

        public IQueryable<OrderItemListViewModel> GetAllOrderItemsLinqQuery()
        {

            var orderitems = (from oi in _db.OrderItems
                            join b in _db.Books on oi.BookId equals b.Id
                            join o in _db.Orders on oi.OrderId equals o.Id
                         select new OrderItemListViewModel
                         {
                             Id = oi.Id,
                             OrderId = oi.OrderId,
                             BookId = oi.BookId,
                             CustomerId = o.CustomerId,
                             BookName = b.Name,
                             Quantity = oi.Quantity,
                             ItemPrice = oi.ItemPrice,
                             ItemDiscount = Math.Round(oi.ItemDiscount * 100),
                             ItemTotalPrice = Math.Round((oi.ItemPrice * oi.Quantity) * (1 - oi.ItemDiscount))
                         });
            return orderitems;
        }



        public IQueryable<OrderListViewModel> GetAllOrdersLinqQuery()
        {

            var orders = (from o in _db.Orders
                            join oi in _db.OrderItems on o.Id equals oi.OrderId into a
                         select new OrderListViewModel
                         {
                            Id = o.Id,
                            CustomerId = o.CustomerId,
                            BillingAddressId = o.BillingAddressId,
                            ShippingAddressId = o.ShippingAddressId,
                            Status = o.Status,
                            OrderDate = o.OrderDate,
                            ShippingDate = o.ShippingDate,
                            TrackingNumber = o.TrackingNumber,
                            PurchaseAmount = o.PurchaseAmount
                         });
            return orders;
        }
    }
}