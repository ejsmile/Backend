using DAL;
using Ninject;
using Ninject.Web.Common.WebHost;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Backend
{
    //public class WebApiApplication : System.Web.HttpApplication
    public class WebApiApplication : NinjectHttpApplication
    {
        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();
        //    GlobalConfiguration.Configure(WebApiConfig.Register);
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);

        //    //var registrations = new DALmodule(ConfigurationManager.ConnectionStrings["SQLLite"].ConnectionString);
        //    //var kernel = new StandardKernel(registrations);
        //    //var ninjectResolver = new NinjectDependencyResolver(kernel);
        //    //DependencyResolver.SetResolver(ninjectResolver);
        //    //GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver;
        //}

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new DALmodule(ConfigurationManager.ConnectionStrings["SQLLite"].Name));
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private void RegisterServices(IKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly());
        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}