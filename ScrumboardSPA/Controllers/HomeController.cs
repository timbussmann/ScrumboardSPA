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
    }
}
