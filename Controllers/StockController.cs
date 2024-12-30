using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net.Data;
using asp.net.Dtos.Stock;
using asp.net.Interfaces;
using asp.net.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp.net.Controllers
{

    [Route("api/stock")]
    [ApiController]
    public class StockController: ControllerBase
    {
        
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){

        // The await keyword ensures that the line waits for the asynchronous operation (ToListAsync) 
        // to complete before proceeding to the next line. While the code is "waiting," the thread is not
        // blocked—it is free to do other work in the application (e.g., handling other requests in a web server context).

        // Once the ToListAsync operation completes, the stocks variable is assigned the result, and the 
        // program execution resumes with the next line: 

            var stocks = await _stockRepo.GetAllAsync();

        // The next line (stocks.Select) will not execute until ToListAsync has finished fetching the data 
        // from the database and returned the result. 
            var stockDto = stocks.Select(s => s.ToStockDto());
    
            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var stock = await _context.Stock.FindAsync(id);

            if(stock == null){
                return NotFound();
            }

            //For singel object transformation call the static method directly,
            //no need to use Select method
            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){

            var stockModel = stockDto.ToStockFromCreateDto();
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            //Below createAtAction first parameter is the name of the method declared in this controller file.
            //Second parameter is the id that is assigned by the SQL Server to the newly created object.
            //Third parameter is saying return the result in the form of StockDto not the Stock model
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto){

            //1. First check and see if the provided id exists in the DB.
            //If it does, get that object and store in stockModel variable
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null){
                return NotFound();
            }

            //2. Update db value with the new values user has provided.
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id){

            //1. First check and see if the provided id exists in the DB.
            //If it does, get that object and store in stockModel variable
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null){
                return NotFound();
            }

            //2. Delete the record - don't add async to Delete, it's not an async function
            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}