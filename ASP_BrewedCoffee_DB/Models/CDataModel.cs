namespace ASP_BrewedCoffee_v2.Models;
public class CDataModel
{
    protected CDBContext DB;
    public CDataModel(CDBContext db_context) { DB = db_context; }
}
