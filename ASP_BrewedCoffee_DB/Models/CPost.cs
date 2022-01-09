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
