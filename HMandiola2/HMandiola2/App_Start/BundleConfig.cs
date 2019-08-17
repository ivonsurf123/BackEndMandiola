using System.Web;
using System.Web.Optimization;

namespace HMandiola2
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/popper", "https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Assets/js/menulateral.js"));
            bundles.Add(new ScriptBundle("~/bundles/dataTable", "https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/dataTableResponsive", "https://cdn.datatables.net/responsive/2.2.3/js/dataTables.responsive.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/Bootstrapt", "http://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"));


            bundles.Add(new StyleBundle("~/Content/dataTable", "https://cdn.datatables.net/1.10.19/css/jquery.dataTables.css"));
            bundles.Add(new StyleBundle("~/Content/dataTableResponsive", "https://cdn.datatables.net/responsive/2.2.3/css/responsive.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/Bootstrapt", "http://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css", "~/Assets/css/menulateral.css"));
            bundles.Add(new StyleBundle("~/Content/fontAwesome", "https://use.fontawesome.com/releases/v5.0.6/css/all.css"));
        }
    }
}