using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globeweigh.Model;
using Globeweigh.UI.Shared.Helpers;

namespace Globeweigh.UI.Shared.Services
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<Product> MarkProductAsDeletedAsync(int productId);
        Task<Product> GetProduct(int productId);
        Task<Product> AddUpdateProductAsync(Product product);
    }

    public class ProductRepository : IProductRepository
    {
        public async Task<List<Product>> GetProducts()
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.Products.Where(a => a.DateDeleted == null).ToListAsync();
            }
        }

        public async Task<Product> GetProduct(int productId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.Products.Where(a => a.id == productId).FirstOrDefaultAsync();
            }
        }

        public async Task<Product> AddUpdateProductAsync(Product product)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                if (product.id == 0)
                {
                    product.DateCreated = DateTime.Now;
                    context.Products.Add(product);
                }
                else
                {
                    context.Products.Attach(product);
                    context.Entry(product).State = EntityState.Modified;
                }
                await context.SaveChangesAsync();
                return product;
            }
        }

        public async Task<Product> MarkProductAsDeletedAsync(int productId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var productToUpdate = await context.Products.FirstOrDefaultAsync(a => a.id == productId);
                if (productToUpdate != null)
                {
                    productToUpdate.DateDeleted = DateTime.Now;
                    context.Products.Attach(productToUpdate);
                    context.Entry(productToUpdate).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                return productToUpdate;
            }
        }
    }
}
