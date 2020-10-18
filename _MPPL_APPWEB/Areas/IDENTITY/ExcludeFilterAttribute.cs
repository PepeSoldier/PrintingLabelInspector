using System;
using System.Web.Mvc;

namespace _LABELINSP_APPWEB.Areas.IDENTITY
{
    public class ExcludeFilterAttribute : FilterAttribute
    {
        private Type filterType;
        public ExcludeFilterAttribute()
        {

        }
        public ExcludeFilterAttribute(Type filterType)
        {
            this.filterType = filterType;
        }

        public Type FilterType
        {
            get
            {
                return this.filterType;
            }
        }
    }
}