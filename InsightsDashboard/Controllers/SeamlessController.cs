using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            bool stop = false;
            int i = 0;
            string inputWords = "";
            //READ IN ALL WORDS
            while (!stop)
            {
                try
                {
                    var singleEntry = await _seamlessDAL.GetMainList(i);
                    inputWords += singleEntry.TwoLineCompanySummary.ToString().ToLower() + " ";
                    i++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    stop = true;
                }
            }
            // CHARS TO REMOVE
            char[] invalidChars = " !@#$%^&*()_+“”~{}|:\"<>?`1234567890-=[]\\;',./".ToCharArray();
            string[] allWords = inputWords.Split(invalidChars);
            Dictionary<string, int> wordFreq = new Dictionary<string, int>();
            // CALCULATE KEYWORD FREQUENCY
            foreach (string word in allWords)
            {
                if (word != "" && word != null) {
                    if (!wordFreq.ContainsKey(word))
                    {
                        wordFreq.Add(word, 1);
                    }
                    else
                    {
                        wordFreq[word] += 1;
                    }
                }
            }
            // ORDER WORDS
            List<KeyValuePair<string,int>>keywordList = wordFreq.OrderByDescending(key => key.Value).ToList<KeyValuePair<string, int>>();
            return View(keywordList);
        }
    }
}
