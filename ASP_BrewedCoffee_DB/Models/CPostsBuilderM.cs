using System.Linq;

namespace ASP_BrewedCoffee_v2.Models;
public class CPostsBuilderM
{
    private CFilterOptionsM FilterOptions = new CFilterOptionsM();
    public CPostsModel Build()
    {
        var posts = new CPostsModel();
        CTableM? table = CFilesM.Instance.GetData(CConf.PostsDataPath, FilterOptions).Result;

        foreach(CTableLineM table_line in table)
        {
            posts.Add(new CDBPostM()
            {
                Title = table_line["Title"],
                Date = CHelperM.ParseDate(table_line["Date"]),
                Content = table_line["Content"],
                Author = table_line["Author"],
                CategoriesID = table_line["Categories"],
                Likes = int.Parse(table_line["Likes"])
            });
        }

        return posts;
    }
    public CPostsBuilderM SetCatName(string cat_name)
    {
        if(cat_name == String.Empty) return this;
        FilterOptions.Callbacks.Add((line) => line["Categories"].Contains(cat_name));

        return this;
    }
    public CPostsBuilderM SetCatID(int cat_id)
    {
        if (cat_id > -1)
            FilterOptions.Callbacks.Add((line) => line["Categories"].Split(';').ToList().Contains(cat_id.ToString()));

        return this;
    }
    public CPostsBuilderM SetMinDate(DateTime min_date)
    {
        if (min_date == default) return this;
        FilterOptions.Callbacks.Add((line) => min_date <= CHelperM.ParseDate(line["Date"]));

        return this;
    }
    public CPostsBuilderM SetMaxDate(DateTime max_date)
    {
        if (max_date == default) return this;
        FilterOptions.Callbacks.Add((line) => max_date >= CHelperM.ParseDate(line["Date"]));

        return this;
    }
    public CPostsBuilderM SetNum(int num)
    {
        FilterOptions.Limit = num;

        return this;
    }
    public CPostsBuilderM SetShift(int page, int count)
    {
        if (page > 1) FilterOptions.Shift = (page - 1) * count;

        return this;
    }
    public CPostsBuilderM SetPostNames(params string[] names)
    {
        FilterOptions.Callbacks.Add((line) => 
        {
            foreach(string name in names)
                if (name == line["Title"]) return true;

            return false;
        });

        return this;
    }
}
