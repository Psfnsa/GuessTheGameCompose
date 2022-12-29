namespace GuessTheGameApi.Converter.Models.Domain
{
    public class GameApiResult
    {
        public string error { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int number_of_page_results { get; set; }
        public int number_of_total_results { get; set; }
        public int status_code { get; set; }
        public Result[] results { get; set; }
        public string version { get; set; }

        public class Result
        {
            public string release_date { get; set; }
            public string description { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string deck { get; set; }
            public Image image { get; set; }
            public Genre[] genres { get; set; }
            public Theme[] themes { get; set; }
            public Franchise[] franchises { get; set; }
            public string images_api_url { get; set; }
            public string reviews_api_url { get; set; }
            public string articles_api_url { get; set; }
            public string videos_api_url { get; set; }
            public string releases_api_url { get; set; }
            public string site_detail_url { get; set; }
        }

        public class Image
        {
            public string square_tiny { get; set; }
            public string screen_tiny { get; set; }
            public string square_small { get; set; }
            public string original { get; set; }
        }

        public class Genre
        {
            public string name { get; set; }
        }

        public class Theme
        {
            public string name { get; set; }
        }

        public class Franchise
        {
            public string name { get; set; }
        }
    }
}