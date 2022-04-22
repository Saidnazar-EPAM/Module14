using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EFCore.Test
{
    public class ProductRepositoryTests
    {
        private readonly ProductRepository _repository;
        private readonly Module14Context _context;

        public ProductRepositoryTests()
        {
            _context = new Module14Context();
            _repository = new ProductRepository(_context);
            SeedData();
        }

        private void SeedData()
        {
            _context.Products.RemoveRange(_context.Products);
            _context.SaveChanges();

            var products = new List<Product>
            {
                new Product { Name = "Product1", Description = "Product1 description", Length = 1, Weight = 5 },
                new Product { Name = "Product2", Description = "Product2 description", Length = 2, Weight = 10 },
                new Product { Name = "Product3", Description = "Product3 description", Length = 3, Weight = 15 },
                new Product { Name = "Product4", Description = "Product4 description", Length = 5, Weight = 20 }
            };

            _context.Products.AddRange(products);
            _context.SaveChanges();
        }

        [Fact]
        public async void GetAll_ReturnsAllProducts()
        {
            var orders = await _repository.GetAllAsync();
            Assert.Equal(4, orders.Count);
        }

        [Fact]
        public async void Get_ProductId_ReturnsProduct()
        {
            var testProduct = await _context.Products.FirstAsync();
            var product = await _repository.GetAsync(testProduct.Id);

            Assert.Equal(testProduct, product);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async void Get_OrderId_ThrowsException(int productId)
        {
            var action = async () => await _repository.GetAsync(productId);

            await Assert.ThrowsAnyAsync<Exception>(action);
        }

        [Fact]
        public async void Create_Order_ReturnsBool()
        {
            var now = DateTime.Now;
            var newProduct = new Product
            {
                Name = "Product5",
                Description = "Product5 description",
                Width = 5,
                Weight = 20
            };
            var res = await _repository.CreateAsync(newProduct);

            Assert.Equal(5, _context.Products.Count());
            Assert.True(res);
        }

        [Fact]
        public async void Update_Order_ReturnsBool()
        {
            var product = await _context.Products.FirstAsync(b => b.Length == 3);
            product.Length = 5;

            var res = await _repository.UpdateAsync(product.Id, product);

            Assert.Equal(5, product.Length);
            Assert.True(res);
        }

        [Fact]
        public async void Delete_Order_ReturnsBool()
        {
            var product = await _context.Products.FirstAsync();

            var res = await _repository.DeleteAsync(product.Id);

            Assert.Equal(3, _context.Products.Count());
            Assert.True(res);
        }

    }
}