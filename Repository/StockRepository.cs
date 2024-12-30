using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net.Data;
using asp.net.Dtos.Stock;
using asp.net.Interfaces;
using asp.net.Models;
using Microsoft.EntityFrameworkCore;

namespace asp.net.Repository
{
    public class StockRepository : IStockRepository
    {

        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {   
           
            _context = context;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
           return  await _context.Stock.ToListAsync();
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
           //FirstOrDefault can return null. Hence, we have the '?' symbol
           //in the return type in the method signature
           var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

           if(stockModel == null){
                return null;
           }

           //Note: remove isn't an asynchronous operation, so don't put await in remove
           _context.Stock.Remove(stockModel);
           await _context.SaveChangesAsync();
           return stockModel;
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.FindAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockRequestDto)
        {
            //FirstOrDefault can return null. Hence, we have the '?' symbol
           //in the return type in the method signature
           var existingStock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

           if(existingStock == null){
                return null;
           }

            existingStock.Symbol = stockRequestDto.Symbol;
            existingStock.CompanyName = stockRequestDto.CompanyName;
            existingStock.Purchase = stockRequestDto.Purchase;
            existingStock.LastDiv = stockRequestDto.LastDiv;
            existingStock.Industry = stockRequestDto.Industry;
            existingStock.MarketCap = stockRequestDto.MarketCap;

            await _context.SaveChangesAsync();
            return existingStock;
        }


    }
}