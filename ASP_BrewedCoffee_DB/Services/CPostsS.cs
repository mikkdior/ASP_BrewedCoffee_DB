namespace ASP_BrewedCoffee_DB.Services
{
    public class CPostsS : CDataModel
    {
        public CPostsS(CDBContext db_context) : base(db_context) { }
        public IEnumerable<CDBPostM> GetAll() => DB.Posts.ToList();
        public void Add(CDBPostM post)
        {
            DB.Posts.Add(post);
            DB.SaveChanges();
        }
    }
}
