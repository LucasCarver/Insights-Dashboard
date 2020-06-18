﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using InsightsDashboard.Models;
using Microsoft.AspNetCore.Authorization;
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



        [Authorize]
        [HttpGet]
        public IActionResult AddUserDefinedStartup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUserDefinedStartup(UserStartup startUp)
        {
            startUp.DateAdded = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.UserStartup.Add(startUp);
                _context.SaveChanges();
                return RedirectToAction("SubmissionComplete", startUp);
            }
            else
            {
                return RedirectToAction("AddUserDefinedStartup");
            }
        }

        public IActionResult SubmissionComplete(UserStartup startUp)
        {
            return View(startUp);
        }

        [Authorize]
        public IActionResult UserList()
        {
            List<UserStartup> userStartUps = new List<UserStartup>();
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            userStartUps = _context.UserStartup.Where(x => x.UserId == uId).ToList();
            return View(userStartUps);
        }

        [Authorize]
        public IActionResult RemoveUserStartUp(int id)
        {
            UserStartup removeStartUp = _context.UserStartup.Find(id);
            _context.UserStartup.Remove(removeStartUp);
            _context.SaveChanges();
            return RedirectToAction("UserList");
        }

        [HttpGet]
        public IActionResult UpdateUserStartUp(int id)
        {
            UserStartup editStartUp = _context.UserStartup.Find(id);
            return View(editStartUp);
        }

        [HttpPost]
        public IActionResult UpdateUserStartUp(UserStartup newStartUp)
        {
            UserStartup startUp = _context.UserStartup.Find(newStartUp.Id);
            if (ModelState.IsValid)
            {
                startUp.CompanyName = newStartUp.CompanyName;
                _context.Entry(startUp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(startUp);
                _context.SaveChanges();
            }
            return RedirectToAction("UserList");
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
                if (word != "" && word != null)
                {
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
            List<KeyValuePair<string, int>> keywordList = wordFreq.OrderByDescending(key => key.Value).ToList<KeyValuePair<string, int>>();
            return View(keywordList);
        }
    }
}
