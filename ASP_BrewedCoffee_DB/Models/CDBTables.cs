namespace ASP_BrewedCoffee_v2.Models;
public class CCategoriesModel : List<CDBCategoryM> { }
public class CPostsModel : List<CDBPostM> { }
public class COptionsModel : Dictionary<string, string> { }
public class CRoutesModel : Dictionary<string, CRouteDataM> { }
public class CDBPostM : IDBItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public string CategoriesID { get; set; }
    public int Likes { get; set; }
}
public class CDBCategoryM : IDBItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
}

public class CDBOptionM: IDBItem
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}
public class CDBRouteM : IDBItem
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}
public class CRouteDataM
{
    public string Template { get; set; }
    public string Controller { get => Controller; set { Controller = value == "" ? "Home" : value; } }
    public string Action { get => Action; set { Action = value == "" ? "Index" : value; } }
}
