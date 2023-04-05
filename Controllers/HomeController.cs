using FormulaOneStats.API;
using FormulaOneStats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace FormulaOneStats.Controllers
{
    public class HomeController : Controller
    {
        private readonly APIGetMethods Ergast = new APIGetMethods();


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<JsonResult> GetRecentResults()
        {
            var driverStandings = await Ergast.GetDriverStandingsRace("current");
            return Json(driverStandings[0]);
        }

        [HttpGet]
        public async Task<JsonResult> GetRaceResults(int round)
        {
            var raceResult = await Ergast.GetRaceResult("current", round);
            return Json(raceResult.Races[0]);
        }

        [HttpGet]
        public async Task<JsonResult> GetDriverInfo(string id)
        {
            var driver = await Ergast.GetDriverInfo(id);
            return Json(driver.Drivers[0]);
        }

        [HttpGet]
        public async Task<JsonResult> GetLapTime(string year, int round, string driverId, int? lap = null)
        {
            var lapTimes = await Ergast.GetLapTime(year, round, driverId, lap);
            return Json(lapTimes.Races[0]);
        }
    }
}