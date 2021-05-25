using System.Web;
using System.Web.Mvc;

namespace skirsesort.kitzbuehel
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
