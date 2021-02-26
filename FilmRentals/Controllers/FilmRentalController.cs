using FilmRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmRentals.Controllers
{
    [Route("api/[controller]")]
    public class FilmRentalController : Controller
    {
        public AppDb Db { get; }

        public FilmRentalController(AppDb db)
        {
            this.Db = db;
        }

        // GET api/filmrental/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var rental = new FilmRental(Db);
                var result = await rental.FindOneAsync(id);
                if (result is null)
                    return new NotFoundResult();
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return new OkObjectResult(e);
            }
        }

        // GET api/filmrental
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var rental = new FilmRental(Db);
            var result = await rental.FindAllAsync();
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/filmrental
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FilmRental body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }
    }
}
