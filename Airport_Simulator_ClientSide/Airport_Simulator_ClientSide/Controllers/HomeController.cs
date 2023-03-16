using Airport_Simulator_ClientSide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Airport_Simulator_ClientSide.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly ILogger<HomeController> _logger;
        #endregion

        #region Consturctors
        public HomeController(ILogger<HomeController> logger) => _logger = logger;
        #endregion

        #region Actions
        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        #endregion
    }
}
