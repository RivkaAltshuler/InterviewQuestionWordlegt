namespace src.Models
{
    public class WordGuessResponse
    {
        public int GuessesLeft { get; set; }
        public int TimeLeftinMinutes { get; set; }
        public LetterGuessResponse[] LettersResponses { get; set; }
    }
}
