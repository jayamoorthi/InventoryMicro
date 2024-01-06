using ProductApi.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        Task<InventoryStock> SaveInventoryStockAsync(InventoryStock inventoryStock);
        Task<bool> DeleteStockAsync(InventoryStock inventoryStock);
    }
}
