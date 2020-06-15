using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InsightsDashboard.Controllers
{
    public class SeamlessController : Controller
    {

        public SeamlessController()
        {

        }

        public IActionResult Index()
        {
            
            return View();
        }
    }
}
