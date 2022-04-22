using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class ProductRepository
    {
        private readonly Module14Context _context;

        public ProductRepository(Module14Context context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetAsync(int orderId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(b => b.Id == orderId);

            if (product == null)
                throw new Exception("Product not found");

            return product;
        }

        public async Task<bool> CreateAsync(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
                throw new Exception("Product is invalid");

            _context.Products.Add(product);

            var res = await _context.SaveChangesAsync();

            if (res > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(int productId, Product product)
        {
            if (product == null || productId == 0 || productId != product.Id)
                throw new ArgumentNullException("Product is invalid");

            _context.Products.Update(product);

            var res = await _context.SaveChangesAsync();

            if (res > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            var product = _context.Products.Find(productId);

            if (product == null)
                throw new Exception("Product not found");

            _context.Products.Remove(product);

            var res = await _context.SaveChangesAsync();

            if (res > 0)
            {
                return true;
            }
            return false;
        }

    }
}
