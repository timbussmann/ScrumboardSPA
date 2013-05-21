namespace ScrumboardSPA.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        public ViewResult GetView(string viewName)
        {
            return View(viewName);
        }

        [ActionName("GetCacheManifest")]
        public ManifestResult GetCacheManifest()
        {
            return new ManifestResult("1", Server, new[]{"/App", "/Scripts", "/Content"});
        }
    }
}
