﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

            List<MainEntry> masterList = await _seamlessDAL.GetMainEntryList();

            return RedirectToAction("ChartTest", masterList);
        }

        public IActionResult ChartTest(List<MainEntry> masterList)
        {
            string companyName = "";
            int Raised = 0;
            int i = 0;
            string y = "";
            List<ChartModel> test = new List<ChartModel>();

            foreach (MainEntry me in masterList)
            {
                companyName = me.CompanyName;


                foreach (char c in me.Raised)
                {
                    if (int.TryParse(c.ToString(), out int p))
                    {
                        y += p;
                        Raised = int.Parse(y);
                    }
                }
                ChartModel x = new ChartModel(companyName, Raised);
                test.Add(x);
            }

            ChartModel sendit = test[1];

            return View(sendit);
        }





        public async Task<IActionResult> Tags()
        {
            bool stop = false;
            int i = 0;
            string inputWords = "";

            List<MainEntry> mainEntryList = await _seamlessDAL.GetMainEntryList();
            //READ IN ALL WORDS
            while (!stop)
            {
                try
                {
                    inputWords += mainEntryList[i].TwoLineCompanySummary.ToString().ToLower() + " ";
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

            // ADD KEYWORDS AND CALCULATE KEYWORD FREQUENCY
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
            // ORDER KEYWORDS BY FREQUENCY
            List<KeyValuePair<string,int>>keywordList = wordFreq.OrderByDescending(key => key.Value).ToList<KeyValuePair<string, int>>();
            return View(keywordList);
        }
    }
}
