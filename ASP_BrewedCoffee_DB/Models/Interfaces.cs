using Microsoft.EntityFrameworkCore;

namespace ASP_BrewedCoffee_v2.Models;
public interface IData
{
    delegate bool DFilter(CTableLineM line);
    class CFilters : List<DFilter> { }
    Task<CTableM> GetData(string Path, CFilterOptionsM options = null);
    void SaveData(string Path, CTableM data);
    int GetCount(string Path, CFilterOptionsM options = null);
}
public interface IDataAccessFactory
{
    IDataAccess Create(string Path);
}
public interface IDataAccess
{
    delegate bool DFilterAction(CTableLineM table_line, CFilterOptionsM options = null);
    IAsyncEnumerable<CTableLineM> Load(CancellationTokenSource CancelTS);
    void Save(CTableM table = default);
}
public interface IBuildMenuStrategy
{
    CMenuM GetMenuData();
    int GetCount(CMenuItemM menu_item);
}
public interface IDBItem
{
    int Id { get; set; }
}
