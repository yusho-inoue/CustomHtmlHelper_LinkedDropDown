using LinkedDropdownExample.ExtensionModels;
using LinkedDropdownExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace LinkedDropdownExample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var exampleViewModel = new ExampleViewModel();
            exampleViewModel.RegionListItems = new List<SelectListItem>() {
                new SelectListItem("アジア", "1"),
                new SelectListItem("ヨーロッパ", "2"),
                new SelectListItem("アフリカ", "3"),
            };
            exampleViewModel.CountryListItems = new List<LinkedSelectListItem>() {
                new LinkedSelectListItem("日本", "1", "1"),
                new LinkedSelectListItem("大韓民国", "2", "1"),
                new LinkedSelectListItem("台湾", "3", "1"),
                new LinkedSelectListItem("イギリス", "4", "2"),
                new LinkedSelectListItem("ドイツ", "5", "2"),
                new LinkedSelectListItem("エジプト", "6", "3"),
                new LinkedSelectListItem("エチオピア", "7", "3"),
            };
            return View(exampleViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}