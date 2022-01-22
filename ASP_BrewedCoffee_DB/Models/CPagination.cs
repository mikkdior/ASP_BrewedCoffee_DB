using Microsoft.AspNetCore.Hosting.Server;

namespace ASP_BrewedCoffee_DB.Models
{
    public class CPagination : CPaginationData
    {
        public CPagination(HttpContext Context) { this.Context = Context; }
        public List<CPaginationItem>? GetPages(string page_key, string current_class_name, string uncurrent_class_name, string last_class_name, string dotted_class_name, string dotted)
        {
            List<CPaginationItem> items = new List<CPaginationItem>();

            if (MaxPages <= 5)
            {
                for (int i = 1; i <= MaxPages; i++)
                    items.Add(CurrentPage == i ? 
                        new CPaginationItem($"#", i.ToString(), current_class_name) : new CPaginationItem($"?{page_key}={i}", i.ToString(), uncurrent_class_name));
                
                return items;
            }
            if (CurrentPage == 1) items.Add(new CPaginationItem($"#", "1", current_class_name));
            else if (CurrentPage > 1)
            {
                if (CurrentPage == 3) items.Add(new CPaginationItem($"?{page_key}=1", "1", uncurrent_class_name));
                else if (CurrentPage > 3)
                {
                    items.Add(new CPaginationItem($"?{page_key}=1", "1", uncurrent_class_name));
                    items.Add(new CPaginationItem($"?{page_key}={DottBackward}", dotted, dotted_class_name));
                }
                if (CurrentPage == MaxPages - 1) items.Add(new CPaginationItem($"?{page_key}={CurrentPage - 2}", (CurrentPage - 2).ToString(), uncurrent_class_name));
                if (CurrentPage > 2)
                {
                    if (CurrentPage == MaxPages && MaxPages >= 4) 
                    {
                        items.Add(new CPaginationItem($"?{page_key}={CurrentPage - 3}", (CurrentPage - 3).ToString(), uncurrent_class_name));
                        items.Add(new CPaginationItem($"?{page_key}={CurrentPage - 2}", (CurrentPage - 2).ToString(), uncurrent_class_name));
                    } 
                    if (MaxPages < 5) items.Add(new CPaginationItem($"?{page_key}={CurrentPage - 2}", (CurrentPage - 2).ToString(), uncurrent_class_name));
                }
                items.Add(new CPaginationItem($"?{page_key}={CurrentPage - 1}", (CurrentPage - 1).ToString(), uncurrent_class_name));
                items.Add(new CPaginationItem($"#", CurrentPage.ToString(), current_class_name));
            }
            if (MaxPages > CurrentPage + 1)
            {
                items.Add(new CPaginationItem($"?{page_key}={CurrentPage + 1}", (CurrentPage + 1).ToString(), uncurrent_class_name));
                if (CurrentPage == 1)
                    items.Add(new CPaginationItem($"?{page_key}={CurrentPage + 2}", (CurrentPage + 2).ToString(), uncurrent_class_name));
                if (CurrentPage <= 2) items.Add(new CPaginationItem($"?{page_key}=4", "4", uncurrent_class_name));
                if (MaxPages > 4 && CurrentPage < MaxPages - 2)
                    items.Add(new CPaginationItem($"?{page_key}={DottForward}", dotted, dotted_class_name));
            }
            if (MaxPages > CurrentPage) items.Add(new CPaginationItem($"?{page_key}={MaxPages}", MaxPages.ToString(), last_class_name));

            return items;
        }
    }
    public class CPaginationData
    {
        protected HttpContext Context;
        public int AllPostsCount { get => int.Parse(Context.Items["AllFilteredPostsNum"].ToString()); }
        public int PostsPerPage { get => int.Parse(Context.Items["PostsPerPage"].ToString()); }
        public int CurrentPage { get => !Context.Request.Query.ContainsKey("page") ? 1 : int.Parse(Context.Request.Query["page"].ToString()); }
        public int MaxPages { get => (int)Math.Ceiling(decimal.Divide(AllPostsCount, PostsPerPage)); }
        public int DottForward { get => (int)Math.Ceiling(decimal.Divide(MaxPages + CurrentPage, 2)); }
        public int DottBackward { get => (int)Math.Ceiling(decimal.Divide(1 + CurrentPage, 2)); }
        public int PrevPage { get => CurrentPage - 1; }
        public int NextPage { get => CurrentPage + 1; }
    }
    public record CPaginationItem(string Url, string Title, string ClassName);
}
