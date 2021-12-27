using Microsoft.EntityFrameworkCore;

namespace ASP_BrewedCoffee_v2.Models;
public class CDBFactory: IDataAccessFactory
{
    public IDataAccess Create(string Path) => CConf.DB;
}
public class CDBContext : DbContext, IDataAccess
{
    public DbSet<CDBPostM> Posts { get; set; }
    public DbSet<CDBCategoryM> Categories { get; set; }
    public DbSet<CDBOptionM> Options { get; set; }
    public DbSet<CDBRouteM> Routes { get; set; }

    public CDBContext(DbContextOptions<CDBContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    public void Init()
    {
        //Console.WriteLine("DB Inited");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(CConf.DbConnString);

    public IAsyncEnumerable<CTableLineM> Load(CancellationTokenSource CancelTS)
    { throw new NotImplementedException(); }

    public async IAsyncEnumerable<CTableLineM> Load(DbSet<IDBItem> dbset, CancellationTokenSource CancelTS)
    {
        await foreach (IDBItem item in dbset)
        {
            if (CancelTS.Token.IsCancellationRequested) yield break;
            yield return DBTableLine(item);
        }
    }

    public CTableLineM? DBTableLine<T>(T item) 
    {
        switch (typeof(T).Name)
        {
            case "CDBPostM":
                CDBPostM? post = item as CDBPostM;
                return new CTableLineM()
                {
                    { "Id" , post.Id.ToString() },
                    { "Title" , post.Title },
                    { "Date" , post.Date.ToString() },
                    { "Content" , post.Content },
                    { "Author" , post.Author },
                    { "CategoriesID" , post.CategoriesID },
                    { "Likes" , post.Likes.ToString() }
                };

            case "CDBCategoryM":
                CDBCategoryM? cat = item as CDBCategoryM;
                return new CTableLineM()
                {
                    { "Id" , cat.Id.ToString() },
                    { "Title" , cat.Title },
                    { "Url" , cat.Url.ToString() }
                };
            case "CDBOptionM":
                CDBOptionM? option = item as CDBOptionM;
                return new CTableLineM()
                {
                    { "Id" , option.Id.ToString() },
                    { "Key" , option.Key.ToString() },
                    { "Value" , option.Value },
                };
            case "CDBRouteM":
                CDBRouteM? route = item as CDBRouteM;
                return new CTableLineM()
                {
                    { "Id" , route.Id.ToString() },
                    { "Key" , route.Key.ToString() },
                    { "Value" , route.Value },
                };
        }

        return null;
    } 
    void IDataAccess.Save(CTableM table = default) => SaveChanges();
}
