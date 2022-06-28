using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NRIIS.API.TESTAPP.Models;

namespace NRIIS.API.TESTAPP.Controllers;

public class ProposalController : Controller
{
    private readonly ILogger<ProposalController> _logger;

    public ProposalController(ILogger<ProposalController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var profile = HttpContext.Session.GetString("profile");
        if(profile == null)
            return RedirectToAction("Index","Home");

        ViewData["profile"] = profile;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
