using CatDataBank.Service;
using CatDataBank.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatDataBank.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatController : ApiControllerBase
    {
        private readonly ICatService _catService;

        public CatController(ICatService catService)
        {
            _catService = catService;
        }

        [HttpGet]
        public IActionResult GetCats()
        {
            try
            {
                return Success(_catService.GetCats());
            }
            catch
            {
                return InternalError();
            }
        }

        [HttpPost]
        public IActionResult AddCat([FromBody] Cat[] cats)
        {
            try
            {
                _catService.AddCats(cats);
                return Created();
            }
            catch
            {
                return InternalError();
            }

        }
    }
}