using Microsoft.EntityFrameworkCore;

namespace ASP_BrewedCoffee_DB.Models;
public class CFilesM : IData
{
    private static readonly Lazy<CFilesM> _Instance = new Lazy<CFilesM>(() => new CFilesM());
    public static CFilesM Instance { get => _Instance.Value; }
    private CFilesM() { }
    //--------------------------------------------------------------
    private IDataAccess DataAcess { get; set; }
    private CancellationTokenSource CancelTS = new CancellationTokenSource();
    //--------------------------------------------------------------
    private IDataAccess SetFile(string Path = "") => DataAcess = CConf.DataAccessFactory.Create(Path);
    //--------------------------------------------------------------
    public void SaveData(string Path, CTableM data) => SetFile(Path).Save(data);
    public void SaveData() => DataAcess.Save();
    //--------------------------------------------------------------
    public int GetCount(string Path, CFilterOptionsM options = null) => GetData(Path, options).Result.Count;
    public int GetCount<T>(DbSet<T> db_set, CFilterOptionsM options = null) where T : class, IDBItem
        => GetData(db_set, options).Result.Count;
    //--------------------------------------------------------------
    private bool Filter(CTableLineM table_line, CFilterOptionsM options = null)
    {
        if (options == null) return true;

        foreach (IData.DFilter filter in options.Callbacks)
            if (!filter(table_line)) return false;

        return true;
    }
    public async Task<CTableM?> GetData(string Path, CFilterOptionsM options = null)
    {
        int curr_shift = 0;
        int curr_limit = 0;

        if (options != null)
        {
            if (options.Limit == 0) return null;
            curr_limit = options.Limit;
            curr_shift = options.Shift;
        }

        CancelTS.Cancel();
        CancelTS = new CancellationTokenSource();
        var table = new CTableM();

        await foreach (CTableLineM table_line in SetFile(Path).Load(CancelTS))
        {
            if (!Filter(table_line, options)) continue;
            if (options != null && curr_shift-- > 0) continue;
            table.Add(table_line);
            if (options != null && --curr_limit == 0) break;
        }

        return table;
    }
    public async Task<CTableM?> GetData<T>(DbSet<T> db_set, CFilterOptionsM options = null)
        where T : class, IDBItem
    {
        int curr_shift = 0;
        int curr_limit = 0;

        if (options != null)
        {
            if (options.Limit == 0) return null;
            curr_limit = options.Limit;
            curr_shift = options.Shift;
        }

        CancelTS.Cancel();
        CancelTS = new CancellationTokenSource();
        var table = new CTableM();

        await foreach (CTableLineM table_line in CConf.DB.Load(db_set, CancelTS))
        {
            if (!Filter(table_line, options)) continue;
            if (options != null && curr_shift-- > 0) continue;
            table.Add(table_line);
            if (options != null && --curr_limit == 0) break;
        }

        return table;
    }
}
