namespace ASP_BrewedCoffee_v2.Models;
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
