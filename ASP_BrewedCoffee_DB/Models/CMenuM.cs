namespace ASP_BrewedCoffee_DB.Models;
public class CMenuItemM
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public int Count { get; set; }
}
public class CMenuM : List<CMenuItemM>
{
    public string Title { get; set; }
    public bool ShowCount { get; set; }
}
public class CMenuFactoryM
{
    public CMenuM Create(IBuildMenuStrategy strategy, string title, bool show_count)
    {
        CMenuM menu = strategy.GetMenuData();
        menu.Title = title;
        menu.ShowCount = show_count;

        if (!show_count) return menu;

        foreach (CMenuItemM menu_item in menu)
            menu_item.Count = strategy.GetCount(menu_item);

        return menu;
    }
}
public class CBuildCategoryStrategyM : IBuildMenuStrategy
{
    public CMenuM GetMenuData()
    {
        Task<CTableM?> table; 
        if (CConf.DB != null) table = CFilesM.Instance.GetData(CConf.DB.Categories, new CFilterOptionsBuilderM().Build());
        else table = CFilesM.Instance.GetData(CConf.CategoriesDataPath, new CFilterOptionsBuilderM().Build());

        var menu = new CMenuM();

        foreach (CTableLineM table_line in table.Result)
        {
            if (CConf.DB != null) menu.Add(new CMenuItemM() 
            {
                ID = int.Parse(table_line["ID"]), 
                Title = table_line["Title"], 
                Url = table_line["Url"] 
            });
            else menu.Add(new CMenuItemM() 
            { 
                Title = table_line["Title"], 
                Url = table_line["Url"] 
            });
        }

        return menu;
    }
    public int GetCount(CMenuItemM menu_item) => 
        (CConf.DB != null) ? 
        CFilesM.Instance.GetCount(CConf.DB.Categories, new CFilterOptionsBuilderM().AddFilter((table_line) => table_line["Categories"].Contains(menu_item.Title)).Build())
        : CFilesM.Instance.GetCount(CConf.PostsDataPath, new CFilterOptionsBuilderM().AddFilter((table_line) => table_line["Categories"].Contains(menu_item.Title)).Build());
    
}
public class CBuildArchiveStrategyM : IBuildMenuStrategy
{
    public CMenuM GetMenuData()  
    {
        return new CMenuM()
        {
            new CMenuItemM(){ Title = "January", Url = $"/archive/jan" },
            new CMenuItemM(){ Title = "February", Url = $"/archive/feb" },
            new CMenuItemM(){ Title = "March", Url = $"/archive/mar" },
            new CMenuItemM(){ Title = "April", Url = $"/archive/apr" },
            new CMenuItemM(){ Title = "May", Url = $"/archive/may" },
            new CMenuItemM(){ Title = "June", Url = $"/archive/jun" },
            new CMenuItemM(){ Title = "July", Url = $"/archive/jul" },
            new CMenuItemM(){ Title = "August", Url = $"/archive/aug" },
            new CMenuItemM(){ Title = "September", Url = $"/archive/sep" },
            new CMenuItemM(){ Title = "October", Url = $"/archive/oct" },
            new CMenuItemM(){ Title = "November", Url = $"/archive/nov" },
            new CMenuItemM(){ Title = "December", Url = $"/archive/dec" },
        };
    }

    public int GetCount(CMenuItemM menu_item)
    {
        return CFilesM.Instance.GetCount(CConf.PostsDataPath, new CFilterOptionsBuilderM().AddFilter((table_line) =>
        {
            int curr_month_num = CHelperM.ParseDate(table_line["Date"]).Month;
            string curr_month_name = ((CConf.EMonths)curr_month_num).ToString();

            if (curr_month_name == menu_item.Title) return true;
            return false;
        }).Build());
    }
}
