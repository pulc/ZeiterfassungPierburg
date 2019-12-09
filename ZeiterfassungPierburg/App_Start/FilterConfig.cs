using System.Web;
using System.Web.Mvc;

namespace ZeiterfassungPierburg
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
