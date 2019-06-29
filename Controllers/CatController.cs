using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CatDataBank.Model;

namespace CatDataBank.Controllers
{


    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatController : Controller
    {
        AppDbContext _appDbContext = new AppDbContext();


        [HttpGet]
        public IActionResult GetCats()
        {
            return Ok(_appDbContext.Cats);
        }

        [HttpPost]
        public IActionResult AddCat([FromBody]Cat cat)
        {
            try
            {
                _appDbContext.Cats.Add(cat);
                _appDbContext.SaveChanges();
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(500);
            }

        }
    }
}