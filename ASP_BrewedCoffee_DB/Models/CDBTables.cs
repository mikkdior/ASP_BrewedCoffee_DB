namespace ASP_BrewedCoffee_DB.Models;
public class CPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedDate { get; set; }
}
public class CCategory
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
}
public class COption
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}