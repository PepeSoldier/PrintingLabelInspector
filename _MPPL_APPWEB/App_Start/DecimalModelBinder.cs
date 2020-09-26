using System;
using System.Globalization;
using System.Web.Mvc;

namespace _MPPL_WEB_START.App_Start
{
    public class CustomModelBinder : DefaultModelBinder
    {

        public CustomModelBinder()
          : base()
        {
        }

        public override object BindModel(ControllerContext controllerContext,
          ModelBindingContext bindingContext)
        {

            object result = null;

            if (bindingContext.ModelType == typeof(decimal))
            {
                
                string modelName = bindingContext.ModelName;
                string attemptedValue = bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;
               

                // Depending on cultureinfo the NumberDecimalSeparator can be "," or "."
                // Both "." and "," should be accepted, but aren't.
                string wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                string alternateSeperator = (wantedSeperator == "," ? "." : ",");

                if (attemptedValue.IndexOf(wantedSeperator) == -1
                  && attemptedValue.IndexOf(alternateSeperator) != -1)
                {
                    attemptedValue = attemptedValue.Replace(alternateSeperator, wantedSeperator);
                }

                try
                {
                    result = decimal.Parse((attemptedValue != "")? attemptedValue : "0", NumberStyles.Any);
                }
                catch (FormatException e)
                {
                    bindingContext.ModelState.AddModelError(modelName, e);
                }

            }
            else if (bindingContext.ModelType == typeof(string))
            {
                string modelName = bindingContext.ModelName;
                var value = bindingContext.ValueProvider.GetValue(modelName);

                try
                {
                    result = Convert.ToString(value == null? "" : value.AttemptedValue);
                }
                catch (FormatException e)
                {
                    bindingContext.ModelState.AddModelError(modelName, e);
                }
            }
            else
            {
                try
                {
                    string modelName = bindingContext.ModelName;
                    var value = bindingContext.ValueProvider.GetValue(modelName);
                    if (value != null)
                        result = base.BindModel(controllerContext, bindingContext);
                    else
                        result = base.BindModel(controllerContext, bindingContext);
                }
                catch (FormatException e)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, e);
                }
            }

            return result;
        }
    }
}