using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public static class Tools
{
    public static string ToStringProperty<T>(this T obj)
    {
        string result = "";
        var x = obj.GetType();
        var ls = x.GetProperties();
        foreach (var prop in ls)
        {
            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                result += $"{prop.Name}: ";
                var list = (IEnumerable<T>)prop.GetValue(obj);
                foreach (var item in list)
                {
                    result += $"{item}. \n ";
                }
            }
            else
                result += $"{prop.Name}: {prop.GetValue(obj)}. \n";
        }
        return result;
    }
}
