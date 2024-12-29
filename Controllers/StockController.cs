using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net.Data;
using asp.net.Dtos.Stock;
using asp.net.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace asp.net.Controllers
{

    [Route("api/stock")]
    [ApiController]
    public class StockController: ControllerBase
    {
        
        private readonly ApplicationDBContext _context;

        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(){

            //Get all stock objects from DB, then use Select method similar to map method JS
            //to iterate over each element & transform each Stock object to StockDto object
            //using the arrow function.
            var stocks = _context.Stock.ToList().Select(s => s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id){
            var stock = _context.Stock.Find(id);

            if(stock == null){
                return NotFound();
            }

            //For singel object transformation call the static method directly,
            //no need to use Select method
            return Ok(stock.ToStockDto());
        }


        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto){

            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stock.Add(stockModel);
            _context.SaveChanges();

            //Below createAtAction first parameter is the name of the method declared in this controller file.
            //Second parameter is the id that is assigned by the SQL Server to the newly created object.
            //Third parameter is saying return the result in the form of StockDto not the Stock model
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

        }


        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto){

            //1. First check and see if the provided id exists in the DB.
            //If it does, get that object and store in stockModel variable
            var stockModel = _context.Stock.FirstOrDefault(x => x.Id == id);

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

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }
    }
}