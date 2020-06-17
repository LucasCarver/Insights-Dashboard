using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsightsDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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

        public async Task<IActionResult> GetList()
        {
            bool stop = false;
            int i = 0;
            List<MainList> masterList = new List<MainList>();

            while (!stop)
            {
                try
                {

                    MainList masterListing = await _seamlessDAL.GetMainList(i);
                    masterList.Add(masterListing);
                    i++;
                }

                catch (ArgumentOutOfRangeException)
                {
                    stop = true;
                }
            }


            return RedirectToAction("ChartTest", masterList);
        }

        public IActionResult ChartTest(List<MainList> masterList)
        {
            string companyName = "";
            int Raised = 0;
            int i = 0;
            string y = "";
            List<ChartModel> test = new List<ChartModel>();

            foreach (MainList ml in masterList)
            {
                companyName = ml.CompanyName;


                foreach (char c in ml.Raised)
                {
                    if (int.TryParse(c.ToString(), out int p))
                    {
                        y += p;
                    }
                }
                Raised = int.Parse(y);

                ChartModel x = new ChartModel(companyName, Raised);
                test.Add(x);
            }
            return View(test);
        }





        public async Task<IActionResult> Tags()
        {
            Rake r = new Rake("..\\InsightsDashboard\\Controllers\\SmartStoplist.txt");
            bool stop = false;
            int i = 0;
            string inputWords = "";
            Dictionary<string, double> keywords = new Dictionary<string, double>();
            while (!stop)
            {
                try
                {
                    var masterList = await _seamlessDAL.GetMainList(i);
                    inputWords = masterList.TwoLineCompanySummary.ToString();
                    var dictionary = r.Run(inputWords);
                    foreach (KeyValuePair<string, double> kvp in dictionary)
                    {
                        if (keywords.ContainsKey(kvp.Key))
                        {
                            //keywords[kvp.Key];
                        }
                        keywords.Add(kvp.Key, kvp.Value);
                    }
                    i++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    stop = true;
                }
            }
            return View(keywords);
        }
    }
}
