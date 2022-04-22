using Xunit;
using EFCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFCore.Test
{
    public class OrderRepositoryTests
    {
        private readonly OrderRepository _repository;
        private readonly Module14Context _context;

        public OrderRepositoryTests()
        {
            _context = new Module14Context();
            _repository = new OrderRepository(_context);
            SeedData();
        }

        private void SeedData()
        {
            _context.Orders.RemoveRange(_context.Orders);
            _context.Products.RemoveRange(_context.Products);
            _context.SaveChanges();

            var products = new List<Product>
            {
                new Product { Name = "Product1", Description = "Product1 description" },
                new Product { Name = "Product2", Description = "Product2 description"} 
            };

            _context.Products.AddRange(products);
            _context.SaveChanges();

            products = _context.Products.ToList();
            var orders = new List<Order>
            {
                new Order { ProductId = products[0].Id, Status = OrderStatus.Loading, CreatedDate = new System.DateTime(2021, 12, 22)},
                new Order { ProductId = products[0].Id, Status = OrderStatus.Arrived, CreatedDate = new System.DateTime(2020, 12, 25)},
                new Order { ProductId = products[1].Id, Status = OrderStatus.Loading, CreatedDate = new System.DateTime(2015, 08, 11)},
                new Order { ProductId = products[1].Id, Status = OrderStatus.Unloading, CreatedDate = new System.DateTime(2015, 02, 11)},
            };

            _context.Orders.AddRange(orders);
            _context.SaveChanges();
        }

        [Theory]
        [InlineData(2, 12)]
        [InlineData(2, 0, 2015)]
        [InlineData(2, 0, 0, OrderStatus.Loading)]
        public async void GetAll_Filter_ReturnsFilteredOrders(int count, int month = 0, int year = 0, OrderStatus orderStatus = 0, int productId = 0)
        {
            var orders = await _repository.GetAllAsync(month, year, orderStatus, productId);
            Assert.Equal(count, orders.Count);
        }

        [Fact]
        public async void Get_OrderId_ReturnsOrder()
        {
            var testOrder = await _context.Orders.FirstAsync();
            var order = await _repository.GetAsync(testOrder.Id);

            Assert.Equal(testOrder, order);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async void Get_OrderId_ThrowsException(int orderId)
        {
            var action = async () => await _repository.GetAsync(orderId);

            await Assert.ThrowsAnyAsync<Exception>(action);
        }

        [Fact]
        public async void Create_Order_ReturnsBool()
        {
            var now = DateTime.Now;
            var product = await _context.Products.FirstAsync();
            var newOrder = new Order
            {
                ProductId = product.Id,
                CreatedDate = now
            };
            var res = await _repository.CreateAsync(newOrder);    
            var order = await _repository.GetAsync(newOrder.Id);   

            Assert.Equal(5, _context.Orders.Count());
            Assert.True(res);
        }

        [Fact]
        public async void Update_Order_ReturnsBool()
        {
            var order = await _context.Orders.FirstAsync(b => b.Status == OrderStatus.Loading);
            order.Status = OrderStatus.Done;

            var res = await _repository.UpdateAsync(order.Id, order);

            Assert.Equal(OrderStatus.Done, order.Status);
            Assert.True(res);
        }

        [Fact]
        public async void Delete_Order_ReturnsBool()
        {
            var order = await _context.Orders.FirstAsync();

            var res = await _repository.DeleteAsync(order.Id);

            Assert.Equal(3, _context.Orders.Count());
            Assert.True(res);
        }


        [Theory]
        [InlineData(2, 12)]
        [InlineData(2, 0, 2015)]
        [InlineData(2, 0, 0, OrderStatus.Loading)]
        public async void BulkDelete_Filter_ReturnsDeletedRowsCount(int expectedCount, int month = 0, int year = 0, OrderStatus orderStatus = 0, int productId = 0)
        {
            await _repository.BulkDeleteAsync(month, year, orderStatus, productId);
            Assert.Equal(expectedCount, _context.Orders.Count());
        }

    }
}