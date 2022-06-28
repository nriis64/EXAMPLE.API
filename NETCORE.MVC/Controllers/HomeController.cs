using System.Diagnostics;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NRIIS.API.TESTAPP.Models;

namespace NRIIS.API.TESTAPP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Profile()
    {
        var profile = HttpContext.Session.GetString("profile");
        if(profile == null)
            return RedirectToAction("Index");

        ViewData["profile"] = profile;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username,string password)
    {
        if(!string.IsNullOrEmpty(username))
        { 
            string token = "";
            HttpClient client = new HttpClient ();
            var data = new StringContent(JsonConvert.SerializeObject(new
                    {
                        LoginName = username,
                        LoginPassword = password,
                    }), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", token);
            string url = "https://api.nriis.go.th/service/user/v2/authenticate";
            HttpResponseMessage response = await client.PostAsync (url, data);

            if (response.IsSuccessStatusCode) {
                var result = response.Content.ReadAsStringAsync().Result;
                HttpContext.Session.SetString("profile", result);  
                HttpContext.Session.SetString("token", ""); 
                return RedirectToAction("Profile");
            }else{
                HttpContext.Session.Clear();
                TempData["msg"] = "The username or password is incorrect.";
            }
        }
        else
        {
            TempData["msg"] = "Sorry, Please input username and password. ";
        }

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
