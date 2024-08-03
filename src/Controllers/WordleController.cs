using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using src.Models;

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


    
       
        [HttpPost("api/Wordle/Start")]
        public async Task<ActionResult> Start(string customerID)
        {
            DateTime wordle_Started_Time;
            if (!_memoryCache.TryGetValue("Wordle_Started_Time", out wordle_Started_Time))
            {
                wordle_Started_Time = DateTime.Now;
                _memoryCache.Set<DateTime>("Wordle_Started_Time", wordle_Started_Time, new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromHours(3)));
                _WordleRepository.CreateWordle();
            }
           
            // connect the customer to this game by saving the customer with the game start time
            var configurationToExpireInEndofGame = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(3 - ((DateTime.Now - wordle_Started_Time)).TotalHours));
            _memoryCache.Set<DateTime>($"{customerID}_StartedTime", wordle_Started_Time, configurationToExpireInEndofGame);
            _memoryCache.Set<int>($"{customerID}_GuessesLeft", 6);


            return Ok(customerID);
        }
     
       
        [HttpPost("api/Wordle/CheckWord")]
        public async Task<ActionResult> CheckWord(string word)
        {
            var response = _WordleRepository.IsAWord(word);

            return Ok(response);
        }


        /// <summary>
        /// Guess
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>

        [HttpPost("api/Wordle/Guess")]
        public IActionResult Guess(string customerID, string word)
        {
            DateTime wordle_Started_Time;
            if (!_memoryCache.TryGetValue($"{customerID}_StartedTime", out wordle_Started_Time))
            {
                return Forbid();
            }
            if(!_WordleRepository.IsAWord(new string(word)))
            {
                return Forbid();
            }
            int guessesLeft;
            if (!_memoryCache.TryGetValue($"{customerID}_GuessesLeft", out guessesLeft))
            {
                guessesLeft = 6;

            }
            _memoryCache.Set<int>($"{customerID}_GuessesLeft", guessesLeft - 1);
            if (guessesLeft == 0)
                return Forbid();

            var response = _WordleRepository.CheckWord(word)
                .ToCharArray()
                .Select(LetterGuessResponseFactory.Create)
                .ToArray();

            return Ok(new WordGuessResponse
            {
                GuessesLeft = guessesLeft - 1,
                TimeLeftinMinutes = Convert.ToInt32((wordle_Started_Time.AddHours(3)- DateTime.Now).TotalMinutes),
                LettersResponses = response
            });
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
