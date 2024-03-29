﻿using ProductApi.Domain.DomainModels;
using ProductApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Domain.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        public async Task<bool> DeleteStockAsync(InventoryStock inventoryStock)
        {
            return await _inventoryRepository.DeleteStockAsync(inventoryStock);        
        }

        public async Task<InventoryStock> SaveInventoryStockAsync(InventoryStock inventoryStock)
        {
            return await _inventoryRepository.SaveInventoryStockAsync(inventoryStock);
           
        }
    }
}
