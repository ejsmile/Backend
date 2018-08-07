using System.Web;

namespace DAL
{
    public static class CustomRequestScope
    {
        public static Ninject.Syntax.IBindingNamedWithOrOnSyntax<T> InCustomRequestScope<T>(this Ninject.Syntax.IBindingInSyntax<T> syntax) => syntax
            .InScope(ctx => HttpContext.Current?.Handler == null ? null : HttpContext.Current.Request);
    }
}