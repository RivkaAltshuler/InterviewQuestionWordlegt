namespace src.Models
{
    public class LetterGuessResponse
    {
        public string Type => GetType().Name;
    }



    public class Correct : LetterGuessResponse
    {
    }

    public class Exists : LetterGuessResponse
    {
    }

    public class Wrong : LetterGuessResponse
    {
    }

    public class LetterGuessResponseFactory
    {
        public static LetterGuessResponse Create(char c)
        {
            switch (c)
            {
                case '0':
                    return new Wrong();
                case '1':
                    return new Exists();
                case '2':
                    return new Correct();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
