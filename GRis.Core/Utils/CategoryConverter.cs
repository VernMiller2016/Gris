using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRis.Core.Utils
{
    public static class CategoryConverter
    {
        public static int? ConvertFromCategoryNameToId(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return null;
            }
            else
            {
                if(categoryName.ToLower() == "Combined-admin".ToLower())
                {
                    return 1;
                }
                else if(categoryName.ToLower() == "MH-admin".ToLower())
                {
                    return 2;
                }
                else if(categoryName.ToLower() == "MED-admin".ToLower())
                {
                    return 3;
                }
                else if(categoryName.ToLower() == "CD-Clinical".ToLower())
                {
                    return 5;
                }
                else if(categoryName.ToLower() == "MH-Clinical".ToLower())
                {
                    return 6;
                }
                else
                {
                    return 4;
                }
            }
        }
    }
}
