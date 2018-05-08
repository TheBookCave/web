using System.Collections.Generic;
using web.Models.InputModels;
using web.Models.ViewModels;
using web.Repositories;
using System.Linq;
using System;

namespace web.Services
{
    public class OrderService
    {
        // OrderService owns a private instance of OrderRepo
        private OrderRepo _orderRepo;
        private BookRepo _bookRepo;
        private GenreRepo _genreRepo;
        private AuthorRepo _authorRepo;
        private PublisherRepo _publisherRepo;

        // Constructor for OrderService that creates the _orderRepo
        public OrderService()
        {
            _bookRepo = new BookRepo();
        }

        // Function that return a list of all the orders
        public List<OrderListViewModel> GetAllOrders()
        {
            var orders = _orderRepo.GetAllOrders();
            return orders;
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
