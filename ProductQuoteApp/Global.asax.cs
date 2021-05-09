using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ProductQuoteApp.App_Start;
using ProductQuoteApp.Helpers;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Controllers;

namespace ProductQuoteApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();


            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //https://haacked.com/archive/2011/03/19/fixing-binding-to-decimals.aspx/
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(int), new IntegerModelBinder());


            //https://stackoverflow.com/questions/793459/how-to-set-decimal-separators-in-asp-net-mvc-controllers
            //ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            //ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());



            //https://blog.murilogomes.com/2015/03/24/globalizing-datetime-and-decimal-in-asp-net-mvc-using-modelbinder/
            //System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            //ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            //ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

            DependenciesConfig.RegisterDependencies();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbConfiguration.SetConfiguration(new ProductQuoteApp.Persistence.ProductQuoteAppConfiguration());

            //Para los mensaje de error se muestren en el idioma correcto dependiendo de Culture.
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "Messages";
            DefaultModelBinder.ResourceClassKey = "Messages";

            DbInterception.Add(new ProductQuoteAppInterceptorLogging());
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //https://stackoverflow.com/questions/1171035/asp-net-mvc-custom-error-handling-application-error-global-asax
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Exception exception = Server.GetLastError();
            //Response.Clear();
            //HttpException httpException = exception as HttpException;

            //if (httpException != null)
            //{
            //    string action;

            //    switch (httpException.GetHttpCode())
            //    {
            //        case 404:
            //            // page not found
            //            action = "HttpError404";
            //            break;
            //        case 500:
            //            // server error
            //            action = "HttpError500";
            //            break;
            //        default:
            //            action = "General";
            //            break;
            //    }

            //    // clear error on server
            //    Server.ClearError();

            //    Response.Redirect(String.Format("~/Error/{0}/?message={1}", action, exception.Message));
            //}

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //https://www.red-gate.com/simple-talk/dotnet/asp-net/pragmatic-web-error-handling-asp-net-mvc/
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //// Grab information about the last error occurred 
            //var exception = Server.GetLastError();

            //// Clear the response stream 
            //var httpContext = ((HttpApplication)sender).Context;
            //httpContext.Response.Clear();
            //httpContext.ClearError();
            //httpContext.Response.TrySkipIisCustomErrors = true;

            //// Manage to display a friendly view 
            //InvokeErrorAction(httpContext, exception);
        }

        protected void InvokeErrorAction(HttpContext httpContext, Exception exception)
        {
            var routeData = new RouteData();
            routeData.Values["controller"] = "home";
            routeData.Values["action"] = "error";
            routeData.Values["exception"] = exception;
            using (var controller = new HomeController(null))
            {
                ((IController)controller).Execute(
                new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
        }

    }
}
