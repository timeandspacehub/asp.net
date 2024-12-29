using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net.Data;
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
    }
}