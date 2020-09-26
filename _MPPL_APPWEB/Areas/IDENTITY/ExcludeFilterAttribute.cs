using System;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.IDENTITY
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