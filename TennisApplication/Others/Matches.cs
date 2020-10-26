using TennisApplication.Models;

namespace TennisApplication.Others
{
    public class Matches
    {
        public Winner Winner { get; set; }
        
        public string Result { get; set; }

        public Matches()
        {
        }

        public Matches(Winner winner, string result)
        {
            this.Winner = winner;
            this.Result= result;
        }
    }
}