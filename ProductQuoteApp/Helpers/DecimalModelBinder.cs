using System;
using System.Globalization;
using System.Web.Mvc;

namespace ProductQuoteApp.Helpers
{

    //ANDAAAA
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                //Check if this is a nullable decimal and a null or empty string has been passed
                var isNullableAndNull = (bindingContext.ModelMetadata.IsNullableValueType &&
                                         string.IsNullOrEmpty(valueResult.AttemptedValue));

                //If not nullable and null then we should try and parse the decimal
                if (!isNullableAndNull)
                {
                    actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
                }
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }

    public class IntegerModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object attemptedValue = null;
            try
            {
                //Check if this is a nullable decimal and a null or empty string has been passed
                var isNullableAndNull = (bindingContext.ModelMetadata.IsNullableValueType &&
                                         string.IsNullOrEmpty(valueResult.AttemptedValue));

                //If not nullable and null then we should try and parse the decimal
                if (!isNullableAndNull)
                {
                    string modelName = bindingContext.ModelName;
                    attemptedValue = bindingContext.ValueProvider.GetValue(modelName)?.AttemptedValue;
                    if (attemptedValue != null)
                    {
                        //attemptedValue = attemptedValue.ToString().Replace(".", "").Replace(",", "");
                        attemptedValue = attemptedValue.ToString().Replace(".", "").Split(',')[0];
                        attemptedValue = int.Parse(attemptedValue.ToString(), NumberStyles.Any);
                    }
                }
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            //attemptedValue = Convert.ToInt32(attemptedValue);
            //attemptedValue = int.Parse(attemptedValue.ToString(), NumberStyles.Any);
            return attemptedValue;
        }
    }

    //public class DecimalModelBinder : IModelBinder
    //{
    //    public object BindModel(ControllerContext controllerContext,
    //        ModelBindingContext bindingContext)
    //    {
    //        ValueProviderResult valueResult = bindingContext.ValueProvider
    //            .GetValue(bindingContext.ModelName);
    //        ModelState modelState = new ModelState { Value = valueResult };
    //        object actualValue = null;
    //        try
    //        {
    //            actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
    //                CultureInfo.CurrentCulture);
    //        }
    //        catch (FormatException e)
    //        {
    //            modelState.Errors.Add(e);
    //        }

    //        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
    //        return actualValue;
    //    }
    //}

    //public class DecimalModelBinder : IModelBinder
    //{
    //    public object BindModel(ControllerContext controllerContext,
    //        ModelBindingContext bindingContext)
    //    {
    //        ValueProviderResult valueResult = bindingContext.ValueProvider
    //            .GetValue(bindingContext.ModelName);
    //        ModelState modelState = new ModelState { Value = valueResult };
    //        object actualValue = null;
    //        try
    //        {
    //            actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
    //                CultureInfo.CurrentCulture);
    //        }
    //        catch (FormatException e)
    //        {
    //            modelState.Errors.Add(e);
    //        }

    //        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
    //        return actualValue;
    //    }
    //}

    //public class DecimalModelBinder : IModelBinder
    //{
    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var cultureCookie = controllerContext.HttpContext.Request.Cookies["_culture"];

    //        //var culture = "en-US";
    //        var culture = "es";

    //        if (cultureCookie != null)
    //            culture = cultureCookie.Value;

    //        decimal value;

    //        var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

    //        if (valueProvider == null)
    //            return null;

    //        if (String.IsNullOrEmpty(valueProvider.AttemptedValue))
    //            return null;

    //        if (Decimal.TryParse(valueProvider.AttemptedValue, NumberStyles.Currency, new CultureInfo(culture), out value))
    //        {
    //            return value;
    //        }

    //        bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalida decimal");

    //        return null;
    //    }
    //}






    //public class DecimalModelBinder : DefaultModelBinder
    //{
    //    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        object result = null;

    //        // Don't do this here!
    //        // It might do bindingContext.ModelState.AddModelError
    //        // and there is no RemoveModelError!
    //        // 
    //        // result = base.BindModel(controllerContext, bindingContext);

    //        string modelName = bindingContext.ModelName;
    //        string attemptedValue = bindingContext.ValueProvider.GetValue(modelName)?.AttemptedValue;

    //        // in decimal? binding attemptedValue can be Null
    //        if (attemptedValue != null)
    //        {
    //            // Depending on CultureInfo, the NumberDecimalSeparator can be "," or "."
    //            // Both "." and "," should be accepted, but aren't.
    //            string wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
    //            string alternateSeperator = (wantedSeperator == "," ? "." : ",");

    //            if (attemptedValue.IndexOf(wantedSeperator, StringComparison.Ordinal) == -1
    //                && attemptedValue.IndexOf(alternateSeperator, StringComparison.Ordinal) != -1)
    //            {
    //                attemptedValue = attemptedValue.Replace(alternateSeperator, wantedSeperator);
    //            }

    //            try
    //            {
    //                if (bindingContext.ModelMetadata.IsNullableValueType && string.IsNullOrWhiteSpace(attemptedValue))
    //                {
    //                    return null;
    //                }

    //                result = decimal.Parse(attemptedValue, NumberStyles.Any);
    //            }
    //            catch (FormatException e)
    //            {
    //                bindingContext.ModelState.AddModelError(modelName, e);
    //            }
    //        }

    //        return result;
    //    }
    //}

    //public class IntegerModelBinder : IModelBinder
    //{
    //    public object BindModel(ControllerContext controllerContext,
    //            ModelBindingContext bindingContext)
    //    {
    //        ValueProviderResult valueResult = bindingContext.ValueProvider
    //            .GetValue(bindingContext.ModelName);
    //        ModelState modelState = new ModelState { Value = valueResult };
    //        //object actualValue = null;
    //        object attemptedValue = null;
    //        try
    //        {
    //            //Check if this is a nullable decimal and a null or empty string has been passed
    //            var isNullableAndNull = (bindingContext.ModelMetadata.IsNullableValueType &&
    //                                     string.IsNullOrEmpty(valueResult.AttemptedValue));

    //            //If not nullable and null then we should try and parse the decimal
    //            if (!isNullableAndNull)
    //            {
    //                string modelName = bindingContext.ModelName;
    //                attemptedValue = bindingContext.ValueProvider.GetValue(modelName)?.AttemptedValue;
    //                if (attemptedValue != null)
    //                {
    //                    attemptedValue = attemptedValue.ToString().Replace(".", "").Replace(",", "");
    //                }
    //            }
    //        }
    //        catch (FormatException e)
    //        {
    //            modelState.Errors.Add(e);
    //        }

    //        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
    //        //attemptedValue = Convert.ToInt32(attemptedValue);
    //        attemptedValue = int.Parse(attemptedValue.ToString(), NumberStyles.Any);
    //        return attemptedValue;
    //    }
    //}


}