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
        public OrderDetailViewModel GetOpenOrder(string userId) {
            var order = (from o in _db.Orders
                        where o.CustomerId == userId
                        where o.Status == "open"

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
            OrderPurchaseSum(order.Id);
            return order;
        }

        public void OrderPurchaseSum(int orderId) {
            var orderNow = _db.Orders.SingleOrDefault(o => o.Id == orderId);
            var orderItemAmount = (from oi in _db.OrderItems
                                    where oi.OrderId == orderNow.Id
                                    select oi.ItemPrice * oi.Quantity * (1 - oi.ItemDiscount)).ToList().Sum();
            orderNow.PurchaseAmount = orderItemAmount;
            _db.SaveChanges();
        }

        public void CloseOrder(OrderConfirmationViewModel confirmed) {
            var orderNow = _db.Orders.SingleOrDefault(o => o.Id == confirmed.Order.Id);
            orderNow.Status = "closed";
            _db.SaveChanges();
        }

        public void ChangeOrderAddress(OrderInputModel orderinput) {
            var orderNow = _db.Orders.SingleOrDefault(o => o.Id == orderinput.Id);
            orderNow.ShippingAddressId = orderinput.ShippingAddressId;
            orderNow.BillingAddressId = orderinput.BillingAddressId;
            _db.SaveChanges();
        }

        public List<AddressListViewModel> GetAddressesOnOrder(int orderId) {
            var del_address = (from da in _db.Addresses
                            join o in _db.Orders on da.Id equals o.ShippingAddressId
                            where o.Id == orderId
                            select da).SingleOrDefault();
            var bil_address = (from ba in _db.Addresses
                            join o in _db.Orders on ba.Id equals o.BillingAddressId
                            where o.Id == orderId
                            select ba).SingleOrDefault();
            
            var del_listviewmodel = new AddressListViewModel() {
                Id = del_address.Id,
                CustomerId = del_address.CustomerId,
                FirstName = del_address.FirstName,
                LastName = del_address.LastName,
                PhoneNumber = del_address.PhoneNumber,
                Country = del_address.Country,
                City = del_address.City,
                StreetAddress = del_address.StreetAddress,
                ZipCode = del_address.ZipCode
            };

            var bil_listviewmodel = new AddressListViewModel() {
                Id = bil_address.Id,
                CustomerId = bil_address.CustomerId,
                FirstName = bil_address.FirstName,
                LastName = bil_address.LastName,
                PhoneNumber = bil_address.PhoneNumber,
                Country = bil_address.Country,
                City = bil_address.City,
                StreetAddress = bil_address.StreetAddress,
                ZipCode = bil_address.ZipCode
            };

            var addresses = new List<AddressListViewModel>();
            addresses.Add(del_listviewmodel);
            addresses.Add(bil_listviewmodel);
            return addresses;
        }


        public List<AddressListViewModel> GetUserAddresses(string userId) {
            var addressesEntity = (from a in _db.Addresses
                            where a.CustomerId == userId
                            select a
                            ).ToList();
            var addresses = new List<AddressListViewModel>();
            foreach (var a in addressesEntity) {
                var addressView = new AddressListViewModel {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    PhoneNumber = a.PhoneNumber,
                    Country = a.Country,
                    City = a.City,
                    StreetAddress = a.StreetAddress,
                    ZipCode = a.ZipCode
                };
                addresses.Add(addressView);
            }

            return addresses;
        }

        public OrderItemInputModel GetOrderItemInputModel(int bookId, string userId) {


            var orderiteminput = new OrderItemInputModel
            {
                BookId = bookId,
                OrderId = GetOpenOrderId(userId),
                ItemDiscount = Convert.ToDecimal((from b in _db.Books where b.Id == bookId select b.Discount).ToList()[0]),
                ItemPrice = Convert.ToDecimal((from b in _db.Books where b.Id == bookId select b.Price).ToList()[0]),
                Quantity = 1
            };
            return orderiteminput;
        }

        public void AddToCart(OrderItemInputModel oi) {
            var orderinput = new OrderItem() {
                OrderId = oi.OrderId,
                BookId = oi.BookId,
                Quantity = oi.Quantity,
                ItemPrice = oi.ItemPrice,
                ItemDiscount = oi.ItemDiscount
            };
            
            _db.Add(orderinput);
            _db.SaveChanges();
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
                var neworder = new Order() {
                CustomerId = userId,
                BillingAddressId = -1,
                ShippingAddressId = -1,
                Status = "open",
                OrderDate = "", 
                ShippingDate = "",
                PurchaseAmount = 0
                };

                _db.Add(neworder);
                _db.SaveChanges();

                return neworder.Id;
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



        public void IncrementOrderItemQuantity (int orderItemId) { 
            var orderItem = _db.OrderItems.SingleOrDefault(oi => oi.Id == orderItemId);
            orderItem.Quantity = orderItem.Quantity + 1;
            _db.SaveChanges();
        }

        public void DecrementOrderItemQuantity (int orderItemId) { 
            var orderItem = _db.OrderItems.SingleOrDefault(oi => oi.Id == orderItemId);
            orderItem.Quantity = orderItem.Quantity - 1;
            _db.SaveChanges();
        }

        public void DeleteOrderItem (int orderItemId) {
            var orderItem = _db.OrderItems.SingleOrDefault(oi => oi.Id == orderItemId);
            _db.Remove(orderItem);
            _db.SaveChanges();
        }
    }
}