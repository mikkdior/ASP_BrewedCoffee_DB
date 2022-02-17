namespace ASP_BrewedCoffee_DB.Models
{
    public class CMySessionData
    {
        private RequestDelegate Next;
        public CMySessionData(RequestDelegate next) { Next = next; }
        private string[] SessionKeys = { "likes", "favorites", "hiddenmenus" };
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method != "POST")
                foreach (var key in SessionKeys) RegSessionData(key, context);

            Next.Invoke(context);
        }
        // update session data /OR/ change value and update session data
        public static void RegSessionData(string key, HttpContext context, string value = "")
        {
            List<string> data;
            bool IsEmpty = true;

            if (context.Session.Keys.Contains<string>(key))
            {
                data = JsonSerializer.Deserialize<List<string>>(context.Session.GetString(key));
                IsEmpty = false;
            }
            else data = new List<string>();

            if (value != "")
            {
                bool in_list = (IsEmpty) ? false : data.Contains(value);

                if (in_list) data.Remove(value);
                else data.Add(value);
            }

            if (context.Items.ContainsKey(key)) context.Items[key] = data;
            else context.Items.Add(key, data);

            context.Session.SetString(key, JsonSerializer.Serialize(data));
        }
    }
    public static class CMySessionDataExt
    {
        public static void UseMySessionData(this IApplicationBuilder self)
        {
            self.UseMiddleware<CMySessionData>();
        }
    }
}
