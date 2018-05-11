using System.Collections.Generic;
using web.Models.InputModels;
using web.Models.ViewModels;
using web.Repositories;
using System.Linq;
using System;
using web.Data;
using web.Data.EntityModels;
using System.Security.Claims;

namespace web.Services
{
    public class OrderService
    {
        // OrderService owns a private instance of OrderRepo
        private OrderRepo _orderRepo;

        // Constructor for OrderService that creates the _orderRepo
        public OrderService(DataContext context)
        {
            _orderRepo = new OrderRepo(context);
        }

        // Function that return a list of all the orders
        public List<OrderListViewModel> GetAllOrders()
        {
            var orders = _orderRepo.GetAllOrdersLinqQuery().ToList();
            return orders;
        }

        public List<OrderItemListViewModel> GetAllOrderItems(int orderId)
        {
            var orderitems = _orderRepo.GetAllOrderItemsLinqQuery().ToList();
            return orderitems;
        }

        public List <OrderItemListViewModel> GetAllItemsInCart (string userId) {
            var openOrderId = _orderRepo.GetOpenOrderId(userId);
            //var openOrderId = 1;
            var orderitemlist = _orderRepo.GetAllOrderItemsLinqQuery().Where( a => a.OrderId.Equals(openOrderId) && a.CustomerId == userId).ToList();
            return orderitemlist;
        }

        public OrderItemInputModel GetOrderItemInputModel(int bookId, string userId) {

            var oim = _orderRepo.GetOrderItemInputModel(bookId, userId);
            return oim;

        }

        public List<AddressListViewModel> GetUserAddresses(string userId) {

            return _orderRepo.GetUserAddresses(userId);
        }

        public OrderDetailViewModel GetOpenOrder(string userId) {
            return _orderRepo.GetOpenOrder(userId);
        }

        public void OrderPurchaseSum(int orderId) {
            _orderRepo.OrderPurchaseSum(orderId);
        }


        public void CloseOrder(string userId) {
             _orderRepo.CloseOrder(userId);
             return;
        }

        public void ChangeOrderAddress(OrderInputModel orderinput) {
             _orderRepo.ChangeOrderAddress(orderinput);
             return;
        }

        public List<AddressListViewModel> GetAddressesOnOrder(int orderId) {
            return _orderRepo.GetAddressesOnOrder(orderId);
        }

        // Function that return a list of all the orders in alphabetic order
        public List<OrderListViewModel> OrderByDate(List<OrderListViewModel> methodOrders)
        {
            var orders = methodOrders.OrderBy(x => x.OrderDate).ToList();
            return orders;
        }

        // Function that return a list of all the orders in alphabetic order desc.
        public List<OrderListViewModel> OrderByDateDesc(List<OrderListViewModel> methodOrders)
        {
            var orders = methodOrders.OrderByDescending(x => x.OrderDate).ToList();
            return orders;
        }

        // Function that return a list of all the orders ordered by price
        public List<OrderListViewModel> OrderByAmount(List<OrderListViewModel> methodOrders)
        {
            var orders = methodOrders.OrderBy(x => x.PurchaseAmount).ToList();
            return orders;
        }

        // Function that return a list of all the orders ordered by price desc
        public List<OrderListViewModel> OrderByAmountDesc(List<OrderListViewModel> methodOrders)
        {
            var orders = methodOrders.OrderByDescending(x => x.PurchaseAmount).ToList();
            return orders;
        }


        // Function that return a order with specified ID
        public OrderDetailViewModel GetOrderWithId(int Id)
        {
            var order = _orderRepo.GetOrderWithId(Id);
            return order;
        }

        public void AddOrder(OrderInputModel inputOrder)
        {
            _orderRepo.AddOrder(inputOrder);
        }

        public void AddToCart(OrderItemInputModel orderinput) {

            _orderRepo.AddToCart(orderinput);
        }


        public void IncrementOrderItemQuantity (int orderItemId) {
            _orderRepo.IncrementOrderItemQuantity (orderItemId);
        }

        public void DecrementOrderItemQuantity (int orderItemId) {
            _orderRepo.DecrementOrderItemQuantity (orderItemId);
        }

        public void DeleteOrderItem (int orderItemId) {
            _orderRepo.DeleteOrderItem (orderItemId);
        }

    }
}
        // Function that searches orders
        /* 
        public List<OrderListViewModel> SearchResults(string searchString)
        {
            if(searchString == null) 
            {
                searchString = "";
            }
            searchString = searchString.ToLower();
            
            var orders = _orderRepo.GetAllOrdersLinqQuery().Where( a => a.Author.ToLower().Contains(searchString) || a.Name.ToLower().Contains(searchString)).ToList();

            return orders;
        } */
