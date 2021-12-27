namespace ASP_BrewedCoffee_DB.Models;
public class CTableLineM : Dictionary<string, string> { }
public class CTableM : List<CTableLineM>
{
    public void Print()
    {
        foreach (var dict in this)
        {
            foreach (var value in dict.Values)
                Console.Write(value + '\t' + '\t' + '\t');

            Console.WriteLine();
        }
    }
}
