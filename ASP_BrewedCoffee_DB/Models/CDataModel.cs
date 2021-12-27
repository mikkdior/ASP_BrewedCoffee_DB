namespace ASP_BrewedCoffee_DB.Models;
public class CDataModel
{
    protected CDBContext DB;
    public CDataModel(CDBContext db_context) { DB = db_context; }
}
