using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsightsDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SeamlessApi.Models;

namespace InsightsDashboard.Controllers
{
    public class SeamlessController : Controller
    {
        private readonly InsightsDbContext _context;
        private readonly SeamlessDAL _seamlessDAL;

        public SeamlessController(IConfiguration configuration, InsightsDbContext context)
        {
            _seamlessDAL = new SeamlessDAL(configuration.GetSection("APIKeys")["SeamlessAPIKey"]);
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Tags()
        {
            Rake r = new Rake("SmartStoplist.txt");
            var masterList = await _seamlessDAL.GetMasterList();
            string inputWords = "";
            for (int i = 0; i < masterList.Records.Length; i++)
            {
                inputWords += masterList.Records[i].MainList.TwoLineCompanySummary;
            }
            var keywords = r.Run(inputWords);
            return View();
        }
    }
}
