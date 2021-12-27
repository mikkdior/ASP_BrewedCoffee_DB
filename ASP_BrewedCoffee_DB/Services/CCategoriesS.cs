namespace ASP_BrewedCoffee_v2.Services
{
    public class CCategoriesS : CDataModel
    {
        public CCategoriesS(CDBContext db_context) : base(db_context) { }
        public void Add(CDBCategoryM cat)
        {
            DB.Categories.Add(cat);
            DB.SaveChanges();
        }
        public IEnumerable<CDBCategoryM> GetAll()
        {
            return DB.Categories.ToList();
        }
    }
}
