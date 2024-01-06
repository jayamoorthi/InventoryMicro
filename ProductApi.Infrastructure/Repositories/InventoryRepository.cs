using ProductApi.Domain.DomainModels;
using ProductApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    public class InventoryRepository: Repository<InventoryStock>,IInventoryRepository
    {
        private readonly InventoryDbContext _inventoryDbContext;

        public InventoryRepository(InventoryDbContext inventoryDbContext):base(inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public Task<bool> DeleteStockAsync(InventoryStock inventoryStock)
        {
            throw new NotImplementedException();
        }

        public async Task<InventoryStock> SaveInventoryStockAsync(InventoryStock inventoryStock)
        {
           await _inventoryDbContext.InventoryStock.AddAsync(inventoryStock);
           await _inventoryDbContext.SaveChangesAsync();
           return inventoryStock;
        }
    }
}
