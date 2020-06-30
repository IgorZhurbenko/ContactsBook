using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForms
{
    public static class HelpersExtension
    {
        public static string DefineInputAttributes(this IHtmlHelper helper, string FieldName)
        {
            string res = "class=\"TextBoxBottomLine\"";
            res = res + $" type=\"{(FieldName.ToLower().Contains("phonenumber") ? "tel" : "text")}\" ";
            if (FieldName.ToLower().Contains("phonenumber"))
            {
                res = res + "pattern=\"" +
                    @"/^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$/" +
                    "\" ";
            }
            res = res + "v-model=\"" + FieldName + "\"";

            return res;
        }
        
    }
}
