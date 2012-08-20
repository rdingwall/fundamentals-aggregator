using System;
using System.Web.Mvc;

namespace FundamentalsAggregator.Mvc
{
    /// <summary>
    /// Converts other stuff like "1" and "yes" into boolean true/false.
    /// </summary>
    public class TruthyBooleanModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof (bool))
                return null;

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null)
                return false;

            var b = value.AttemptedValue;

            if (String.IsNullOrWhiteSpace(b))
                return false;

            b = b.Trim();

            if (b == "1" || String.Equals(b, "yes", StringComparison.CurrentCultureIgnoreCase))
                return true;

            bool result;
            Boolean.TryParse(b, out result);
            return result;
        }
    }
}