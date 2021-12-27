namespace ASP_BrewedCoffee_DB.Models;
public class CFilterOptionsBuilderM
{
    private CFilterOptionsM FilterOptions = new CFilterOptionsM();
    public CFilterOptionsM Build()
    {
        CFilterOptionsM options = FilterOptions;
        FilterOptions = new CFilterOptionsM();

        return options;
    }
    public CFilterOptionsBuilderM SetShift(int shift)
    {
        FilterOptions.Shift = shift;

        return this;
    }
    public CFilterOptionsBuilderM SetLimit(int limit)
    {
        FilterOptions.Limit = limit;

        return this;
    }
    public CFilterOptionsBuilderM AddFilter(IData.DFilter callback)
    {
        FilterOptions.Callbacks.Add(callback);

        return this;
    }
}
