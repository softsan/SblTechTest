using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SblTechTest.Hubs;
using SblTechTest.Models;
using SblTechTest.Repository;

namespace SblTechTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IDatabaseRepository _databaseRepository;
        IHubContext<BuyerHub> _hubContext;
        public HomeController(ILogger<HomeController> logger, 
                            IDatabaseRepository databaseRepository, 
                            IHubContext<BuyerHub> hubContext,
                            IDatabaseSubscription databaseSubscription, IConfiguration  configuration)
        {
            _logger = logger;
            _databaseRepository = databaseRepository;
            _hubContext = hubContext;
            databaseSubscription.Configure(configuration.GetConnectionString("DbInterview"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Events()
        {
            var eventValue = _databaseRepository.Events;
             
            return new JsonResult(eventValue);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitData(string buyerName)
        {
            await _databaseRepository.AddBuyer(buyerName, "san.thanki");
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
