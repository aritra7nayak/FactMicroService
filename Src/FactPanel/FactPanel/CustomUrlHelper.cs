using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FactPanel
{
    public static class CustomUrlHelper
    {
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string GenerateUrl(string controller, string action, object routeValues)
        {

            string url = "/" + controller + "/" + action + "?";

            PropertyInfo[] properties = routeValues.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(routeValues, null);
                if (value != null && value.ToString() != "")
                {
                    if (value.GetType() == typeof(DateTime))
                    {
                        value = string.Format("{0:dd/MM/yyyy}", DateTime.Parse(value.ToString()));
                    }
                    url += property.Name + "=" + value + "&";
                }
            }

            if (!url.Contains("&"))
            {
                url = url.Replace("?", "");
            }

            url = url.TrimEnd('&');
            return url;
        }

    }
}
