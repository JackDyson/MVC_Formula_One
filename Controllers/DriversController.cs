using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ErgastApi.Client;
using ErgastApi.Requests;
using FormulaOneStats.Models;

public class DriversController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}