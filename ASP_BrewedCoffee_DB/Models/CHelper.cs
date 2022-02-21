namespace ASP_BrewedCoffee_DB.Models;
public static class CHelper
{
    public static int GetMonthNum(string url, CMenu menu)
    {
        int month = 1; // filler
        CMenu menu_months = new CMenu();

        foreach (CMenuItem item in menu)
        {
            if (item.Title != "Old")
                menu_months.Add(item);
        }

        for (int i = 0; i < menu_months.Count; i++)

            if (menu_months[i].Url == url)
            {
                month = ++i;
                break;
            }

        return month;
    }
    public static DateTime ParseDate(string date)
    {
        string[] parsed = date.Split('.');
        return new DateTime(int.Parse(parsed[2]), int.Parse(parsed[1]), int.Parse(parsed[0]));
    }
    public static CMenu SortArchive(CMenu menu)
    {
        CMenuItem old = menu[0];
        CMenuItem curr_month = menu[DateTime.Now.Month];
        menu.Remove(old);

        IEnumerable<CMenuItem> items = menu.Skip(DateTime.Now.Month - 1).Concat(menu.Take(DateTime.Now.Month - 1));

        CMenu sorted = new CMenu() { Title = menu.Title, ShowCount = menu.ShowCount};
        foreach (CMenuItem item in items) sorted.Add(item);

        sorted.Remove(curr_month);
        sorted.Reverse();
        sorted.Insert(0, curr_month);
        sorted.Add(old);

        return sorted;
    }
    public static string? GetPathFromConfig(string key)
    {
        string[] lines = File.ReadAllLines(CConfService.ConfigPath);

        foreach (string line in lines)
        {
            string[] parts = line.Split(';');
            if (parts[0] == key) return parts[1];
        }

        return null;
    }
    public static int ValidatePage(int page, int all_posts_num, int num_per_page)
    {
        int max_pages = (int)Math.Ceiling(decimal.Divide(all_posts_num, num_per_page));
        if (page > max_pages) page = max_pages;
        else if (page < 1) page = 1;

        return page;
    }
    public static string GetMonthBySlug(string slug, CMenu arch_menu)
    {
         foreach (CMenuItem item in arch_menu) 
            if (item.Slug == slug) return item.Title;

        return "";
    }
    public static string CropString(string text, int max_len) => text.Length < max_len ? text : text[0..max_len] + "...";
    public static void DeleteAllPosts(CPostsService post_model)
    {
        var posts = new List<CPost>();
        foreach (CPost post in post_model.GetPosts()) posts.Add(post);
        foreach (CPost post in posts) post_model.DeletePost(post.Id);
    }
    public static void CreateRandomPosts(int num , CPostsService post_model)
    {
        string[] lorems = { "lorem", "ipsum", "dolor", "sit", "amet", "consectetur",
        "adipiscing", "elit", "curabitur", "vel", "hendrerit", "libero",
        "eleifend", "blandit", "nunc", "ornare", "odio", "ut",
        "orci", "gravida", "imperdiet", "nullam", "purus", "lacinia",
        "a", "pretium", "quis", "congue", "praesent", "sagittis",
        "laoreet", "auctor", "mauris", "non", "velit", "eros",
        "dictum", "proin", "accumsan", "sapien", "nec", "massa",
        "volutpat", "venenatis", "sed", "eu", "molestie", "lacus",
        "quisque", "porttitor", "ligula", "dui", "mollis", "tempus",
        "at", "magna", "vestibulum", "turpis", "ac", "diam",
        "tincidunt", "id", "condimentum", "enim", "sodales", "in",
        "hac", "habitasse", "platea", "dictumst", "aenean", "neque",
        "fusce", "augue", "leo", "eget", "semper", "mattis",
        "tortor", "scelerisque", "nulla", "interdum", "tellus", "malesuada",
        "rhoncus", "porta", "sem", "aliquet", "et", "nam",
        "suspendisse", "potenti", "vivamus", "luctus", "fringilla", "erat",
        "donec", "justo", "vehicula", "ultricies", "varius", "ante",
        "primis", "faucibus", "ultrices", "posuere", "cubilia", "curae",
        "etiam", "cursus", "aliquam", "quam", "dapibus", "nisl",
        "feugiat", "egestas", "class", "aptent", "taciti", "sociosqu",
        "ad", "litora", "torquent", "per", "conubia", "nostra",
        "inceptos", "himenaeos", "phasellus", "nibh", "pulvinar", "vitae",
        "urna", "iaculis", "lobortis", "nisi", "viverra", "arcu",
        "morbi", "pellentesque", "metus", "commodo", "ut", "facilisis",
        "felis", "tristique", "ullamcorper", "placerat", "aenean", "convallis",
        "sollicitudin", "integer", "rutrum", "duis", "est", "etiam",
        "bibendum", "donec", "pharetra", "vulputate", "maecenas", "mi",
        "fermentum", "consequat", "suscipit", "aliquam", "habitant", "senectus",
        "netus", "fames", "quisque", "euismod", "curabitur", "lectus",
        "elementum", "tempor", "risus", "cras" };

        Random x = new Random();
        int length = lorems.Length;

        for (int i = 0; i < num; i++)
        {
            string title = "";
            for (int k = 0; k < x.Next(8, 25); k++)
            {
                if (k != 0) title += " ";
                title += lorems[x.Next(length)];
            }

            title = char.ToUpper(title[0]) + title.Substring(1);

            string content = "";
            for (int k = 0; k < x.Next(50, 150); k++)
            {
                if (k != 0) content += " ";
                content += lorems[x.Next(length)];
            }
            DateTime date = new DateTime(x.Next(2000, 2021), x.Next(1, 12), x.Next(1, 28), x.Next(0, 23), x.Next(0, 59), x.Next(0, 59));

            content = char.ToUpper(content[0]) + content.Substring(1);

            post_model.Add(new CPost()
            {
                Title = title,
                Author = "admin",
                CategoryId = x.Next(1, 10),
                Content = content,
                CreatedDate = date,
                Likes = x.Next(150)
            });
        }
    }
}