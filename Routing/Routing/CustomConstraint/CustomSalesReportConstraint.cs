
using System.Text.RegularExpressions;

namespace Routing.CustomConstraint
{
    public class CustomSalesReportConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            Console.WriteLine(routeKey);
            if(!values.ContainsKey(routeKey))
            {
                return false; // not matching
            }
            Regex monthRegex = new Regex("^(apr|jul|sep|dec)$");
            string? monthValue = values[routeKey].ToString();

            if (monthRegex.IsMatch(monthValue))
            {
                return true;
            }
            else
            {
                return false;
            }
            //throw new NotImplementedException();
        }
    }
}
