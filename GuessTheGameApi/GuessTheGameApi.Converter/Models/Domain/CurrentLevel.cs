namespace GuessTheGameApi.Converter.Models.Domain
{
    public class CurrentLevel
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int Combo { get; set; }
        public int IdUser { get; set; }
        public List<Game> ListGames { get; set; }
    }
}
