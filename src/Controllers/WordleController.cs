using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordleController : ControllerBase
    {
        WordleRepository _WordleRepository;
        private readonly IMemoryCache _memoryCache;

        public WordleController(WordleRepository wordleRepository, IMemoryCache memoryCache)
        {
            _WordleRepository = wordleRepository;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Example of calls
        /// </summary>
        /// <returns>string</returns>
        [HttpGet]
        public async Task<ActionResult> Test()
        {
            _WordleRepository.CreateWordle();

            var isOkWord = _WordleRepository.IsAWord("Aalst");

            var response = _WordleRepository.CheckWord("abcde");

            return Ok(response);
        }

        //What to add 
        //the function should get customer identifier (could be anything)
        //this will be used in allow only 6 guess
        //Cache example in WordleRepository


        //function (not get) to start game

        //function (not get) to check word exists in dictionary

        //function (not get) to check word against current word
        //please make sure that you return a better object then "01020"
        //Add support for swagger


        //function here to allow only 6 guess - using cache by customer
        //so you need to store the number of guess


        //all function should have validation + correct http status to return
        // 200, 403 if there is an error

    }
}
