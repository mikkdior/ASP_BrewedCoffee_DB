namespace ASP_BrewedCoffee_DB.Models
{
    public class CFooterMenuM : List<Dictionary<string, Dictionary<string, string>>>
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public CFooterMenuM ParseMenu(string dict_key, string title)
        {
            string[] parsed_title = title.Split('|');
            Year = int.Parse(parsed_title[0]);
            Title = parsed_title[1];

            string[] parsed = dict_key.Split('&');
            foreach (string menu_item in parsed)
            {
                string[] right = menu_item.Split('|');
                var dict = new Dictionary<string, Dictionary<string, string>>();
                var links = new Dictionary<string, string>();
                string[] link = right[1].Split('~');
                links.Add(link[0], link[1]);
                dict.Add(right[0], links);
                this.Add(dict);
            }

            return this;
        }
    }
}
