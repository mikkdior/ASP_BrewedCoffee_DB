using Microsoft.AspNetCore.Hosting.Server;

namespace ASP_BrewedCoffee_DB.Models
{
    public enum EDirection
    {
        Previous,
        Next
    }
    public class CPagination : CPaginationData
    {
        public CPagination(HttpContext Context) { this.Context = Context; }
        public List<string>? GetPages(string page_key, string current_class_name, string last_class_name, string dotted_class, string dotted)
        {
            List<string> items = new List<string>();

            if (MaxPages <= 4)
            {
                for (int i = 1; i <= MaxPages; i++)
                {
                    if (CurrentPage == i)
                        items.Add($"<span class='{current_class_name}'><a href='#'>{i}</a></span>");
                    else items.Add($"<a href='?{page_key}={i}'>{i}</a>");
                }
            }
            else
            {
                if (CurrentPage == 1)
                    items.Add ($"<span class='{current_class_name}'><a href='#'>1</a></span>");
                else if (CurrentPage > 1)
                {
                    if (CurrentPage == 3)
                        items.Add($"<a href='?{page_key}=1'>1</a>");
                    
                    if (CurrentPage >= 4)
                    {
                        items.Add($"<a href='?{page_key} = 1'>1</a>");
                        items.Add($"<span class='{dotted_class}'><a href='?{page_key}={DottBackward}'>{dotted}</a></span>");
                    }
                    if (CurrentPage >= MaxPages - 1)
                        items.Add($"<a href='?{page_key}={CurrentPage - 2}'>{CurrentPage - 2})</a>");
                    
                    if (CurrentPage >= 3)
                    {
                        if (CurrentPage == MaxPages && MaxPages >= 4)
                            items.Add($"<a href='?{page_key}={CurrentPage - 3}'>{CurrentPage - 3}</a>");
                        
                        if (MaxPages <= 4)
                            items.Add($"<a href='?{page_key}={CurrentPage - 2}'>{CurrentPage - 2}</a>");
                    }

                    items.Add($"<a href='?{page_key}={CurrentPage - 1}'>{CurrentPage - 1}</a>");
                    items.Add($"<span class='{current_class_name}'><a href='#'>{CurrentPage}</a></span>");
                }
                if (MaxPages > CurrentPage + 1)
                {
                    items.Add($"<a href='?{page_key}={CurrentPage + 1}'>{CurrentPage + 1}</a>");
                    if (CurrentPage == 1)
                        items.Add($"<a href='?{page_key}={CurrentPage + 2}'>{CurrentPage + 2}</a>");
                    if (CurrentPage <= 2)
                        items.Add($"<a href='?{page_key}=4'>4</a>");
                    if (MaxPages > 4 && CurrentPage < MaxPages - 2)
                        items.Add($"<span class='{dotted_class}'><a href='?{page_key}={DottForward}'>{dotted}</a></span>");
                }
                if (MaxPages > CurrentPage)
                    items.Add($"<a class='{last_class_name}' href='?{page_key}={MaxPages}'>{MaxPages}</a>");
            }

            //foreach(string i in items) i = "";

            return items;
        }
        public string GetArrow(string arrow_class_name, string a_class_name, string i_class_name, EDirection direction)
        {
            int num_page = direction == EDirection.Next ? CurrentPage + 1 : CurrentPage - 1;

            return $"<!DOCTYPE html><div class='{arrow_class_name}'>" +
                        $"<a class='{a_class_name}' href='?page={num_page}'>" +
                            $"<i class='{i_class_name}'></i>" +
                        $"</a>" +
                    $"</div>";
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
    }
}
