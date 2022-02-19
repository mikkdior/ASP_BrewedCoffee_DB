using Microsoft.AspNetCore.Razor.TagHelpers;
namespace ASP_BrewedCoffee_DB.TagHelpers;
public class PaginationTagHelper : TagHelper
{
    public CPagination PaginationModel { get; set; }
    public string PageKey { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (PaginationModel.AllPostsCount <= PaginationModel.PostsPerPage)
        {
            output.TagName = "";
            return;
        }
        //-----------------------------------------------------------------------
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", "md-pag-pagination");

        if (PaginationModel.CurrentPage > 1)
            output.Content.AppendHtml($"<a class='md-pag-prev' id='md-pag-arrow' href='?page={PaginationModel.PrevPage}'> &#60;</a>");

        foreach (var item in PaginationModel.GetPages(PageKey, "md-pag-current", "md-pag-uncurrent", "md-pag-last", "md-pag-dotted", "..."))
                    if (item.Url == "#")
                        output.Content.AppendHtml($"<a class='{item.ClassName}' href='#'>{item.Title}</a>");
                    else if (item.Title == "...")
                        output.Content.AppendHtml($"<a class='{item.ClassName}' href='{item.Url}'>...</a>");
                    else
                        output.Content.AppendHtml($"<a class='{item.ClassName}' href='{item.Url}'>{item.Title}</a>");

        if(PaginationModel.CurrentPage != PaginationModel.MaxPages)
            output.Content.AppendHtml($"<a class='md-pag-next' id='md-pag-arrow' href='?page={PaginationModel.NextPage}'> &#62;</a>");
    }
}
public class CPagination : CPaginationData
{
    public CPagination(IViewModel ViewModel) { this.ViewModel = ViewModel; }
    public List<CPaginationItem>? GetPages(string page_key, string current_class_name, string uncurrent_class_name, string last_class_name, string dotted_class_name, string dots)
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
                items.Add(new CPaginationItem($"?{page_key}={DottBackward}", dots, dotted_class_name));
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
                items.Add(new CPaginationItem($"?{page_key}={DottForward}", dots, dotted_class_name));
        }
        if (MaxPages > CurrentPage) items.Add(new CPaginationItem($"?{page_key}={MaxPages}", MaxPages.ToString(), last_class_name));

        return items;
    }
}
public class CPaginationData
{
    protected IViewModel ViewModel;
    public int CurrentPage { get => ViewModel.Page; }
    public int AllPostsCount { get => ViewModel.AllFilteredPostsNum; }
    public int PostsPerPage { get => ViewModel.PostsPerPage; }
    public int MaxPages { get => (int)Math.Ceiling(decimal.Divide(AllPostsCount, PostsPerPage)); }
    public int DottForward { get => (int)Math.Ceiling(decimal.Divide(MaxPages + CurrentPage, 2)); }
    public int DottBackward { get => (int)Math.Ceiling(decimal.Divide(1 + CurrentPage, 2)); }
    public int PrevPage { get => CurrentPage - 1; }
    public int NextPage { get => CurrentPage + 1; }
}
public record CPaginationItem(string Url, string Title, string ClassName);

//------------------for moving this script-------------------
/*
public interface IViewModel
{
    int PostsPerPage { get; set; }
    int Page { get; set; }
    int AllFilteredPostsNum { get; set; }
    public IEnumerable<CPost>? CurrentPosts { get; set; }
}*/
//-----------------------------------------------------------