using System;
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
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;

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
        public async Task<IActionResult> UserList()
        {
            List<UserStartup> definedUserStartUps = new List<UserStartup>();
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            definedUserStartUps = _context.UserStartup.Where(x => x.UserId == uId).Where(x => x.Identifier == null).ToList();
            List<UserStartup> userSavedSeamlessStartups = _context.UserStartup.Where(x => x.UserId == uId).Where(x => x.Identifier != null).ToList();
            List<UserStartup> finalDisplayList = new List<UserStartup>();
            Dictionary<string, MainEntry> seamlessDictionary = await _seamlessDAL.GetMainDictionary();
            foreach (UserStartup us in userSavedSeamlessStartups)
            {
                //GET THE REST OF THE DETAILS
                MainEntry me = seamlessDictionary[us.Identifier];
                us.Alignment = me.Alignment;
                us.City = me.City;
                us.CompanyWebsite = me.CompanyWebsite;
                us.Country = me.Country;
                us.Landscape = me.Landscape;
                us.Raised = me.Raised;
                if (DateTime.TryParse(me.ReviewDate, out DateTime r))
                {
                    us.ReviewDate = r;
                }
                us.Scout = me.Scout;
                us.Source = me.Source;
                us.StateProvince = me.StateProvince;
                if (int.TryParse(me.Team, out int t))
                {
                    us.Team = t;
                }
                us.Technology = me.TechnologyAreas;
                us.Theme = me.Themes;
                if (int.TryParse(me.Uniqueness, out int u))
                {
                    us.Uniqueness = u;
                }
                us.TwoLineSummary = me.TwoLineCompanySummary;
                finalDisplayList.Add(us);
            }
            foreach (UserStartup us in definedUserStartUps)
            {
                finalDisplayList.Add(us);
            }
            return View(finalDisplayList);
        }

        [Authorize]
        public IActionResult RemoveUserStartUp(int id)
        {
            UserStartup removeStartUp = _context.UserStartup.Find(id);
            _context.UserStartup.Remove(removeStartUp);
            _context.SaveChanges();
            return RedirectToAction("UserList");
        }

        public IActionResult ConfirmRemoveUserStartUp(int id)
        {
            UserStartup removeStartUp = _context.UserStartup.Find(id);
            return View(removeStartUp);
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
                startUp.CompanyWebsite = newStartUp.CompanyWebsite;
                startUp.DateAdded = newStartUp.DateAdded;
                startUp.ReviewDate = newStartUp.ReviewDate;
                startUp.Scout = newStartUp.Scout;
                startUp.Source = newStartUp.Source;
                startUp.City = newStartUp.City;
                startUp.StateProvince = newStartUp.StateProvince;
                startUp.Country = newStartUp.Country;
                startUp.TwoLineSummary = newStartUp.TwoLineSummary;
                startUp.Alignment = newStartUp.Alignment;
                startUp.Theme = newStartUp.Theme;
                startUp.Technology = newStartUp.Technology;
                startUp.Landscape = newStartUp.Landscape;
                startUp.Uniqueness = newStartUp.Uniqueness;
                startUp.Team = newStartUp.Team;
                startUp.Raised = newStartUp.Raised;
                //DO THIS LATER GUYS
                //startUp.Comments = newStartUp.Comments;
                //startUp.Rating = newStartUp.Rating;

                _context.Entry(startUp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(startUp);
                _context.SaveChanges();
            }
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> AddSeamlessStartupEntry(string key)
        {
            if (key == null)
            {
                return RedirectToAction("DisplaySavedSeamlessStartupEntries");
            }
            Dictionary<string, MainEntry> seamlessDictionary = await _seamlessDAL.GetMainDictionary();
            MainEntry startupDetails = seamlessDictionary[key];
            UserStartup us = new UserStartup()
            {
                Identifier = key,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                DateAdded = DateTime.Parse(startupDetails.DateAdded),
                CompanyName = startupDetails.CompanyName
            };
            _context.UserStartup.Add(us);
            _context.SaveChanges();
            return RedirectToAction("UserList");
        }



        public async Task<IActionResult> AlignmentDetails(string alignment)
        {
            Dictionary<string, MainEntry> seamlessDictionary = await _seamlessDAL.GetMainDictionary();
            Dictionary<string, MainEntry> alignmentDictionary = new Dictionary<string, MainEntry>();
            string[] alignments = new string[] { };
            int i = 0;
            foreach (KeyValuePair<string, MainEntry> x in seamlessDictionary)
            {

                try
                {
                    if (x.Value.Alignment != null)
                    {
                        i++;
                        string y = x.Value.Alignment;
                        alignments = y.Split(',');
                        x.Value.Alignment = "";
                        foreach (string s in alignments)
                        {

                            x.Value.Alignment += s.Trim() + " ";
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }



                if (x.Value.Alignment != null)
                {
                    if (x.Value.Alignment.Contains(alignment))
                    {
                        alignmentDictionary.Add(x.Key, x.Value);
                    }
                }
            }
            return View(alignmentDictionary);
        }


        public async Task<IActionResult> DisplaySeamlessStartups()
        {
            Dictionary<string, MainEntry> seamlessDictionary = await _seamlessDAL.GetMainDictionary();
            string[] alignments = new string[] { };
            int i = 0;
            foreach (KeyValuePair<string, MainEntry> x in seamlessDictionary)
            {           
                    try
                    {
                    if (x.Value.Alignment != null)
                    {
                        i++;
                        string y = x.Value.Alignment;
                        alignments = y.Split(',');                 
                        x.Value.Alignment = "";
                        foreach(string s in alignments)
                        {

                            x.Value.Alignment += s.Trim() + " ";
                        }
                    }
                    }
                    catch (IndexOutOfRangeException) { }
            }
            return View(seamlessDictionary);
        }

        public async Task<IActionResult> StartupDetails(int id)
        {
            UserStartup specificStartup = _context.UserStartup.Find(id);
            if (specificStartup.Identifier != null)
            {
                Dictionary<string, MainEntry> seamlessDictionary = await _seamlessDAL.GetMainDictionary();
                MainEntry me = seamlessDictionary[specificStartup.Identifier];
                specificStartup.Alignment = me.Alignment;
                specificStartup.City = me.City;
                specificStartup.CompanyWebsite = me.CompanyWebsite;
                specificStartup.Country = me.Country;
                specificStartup.Landscape = me.Landscape;
                specificStartup.Raised = me.Raised;
                if (DateTime.TryParse(me.ReviewDate, out DateTime r))
                {
                    specificStartup.ReviewDate = r;
                }
                specificStartup.Scout = me.Scout;
                specificStartup.Source = me.Source;
                specificStartup.StateProvince = me.StateProvince;
                if (int.TryParse(me.Team, out int t))
                {
                    specificStartup.Team = t;
                }
                specificStartup.Technology = me.TechnologyAreas;
                specificStartup.Theme = me.Themes;
                if (int.TryParse(me.Uniqueness, out int u))
                {
                    specificStartup.Uniqueness = u;
                }
                specificStartup.TwoLineSummary = me.TwoLineCompanySummary;
            }
            return View(specificStartup);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddStartupComments(int id)
        {
            UserStartup startupToComment = _context.UserStartup.Find(id);
            return View(startupToComment);
        }

        [HttpPost]
        public IActionResult AddStartupComments(UserStartup startupToComment)
        {
            UserStartup startUp = _context.UserStartup.Find(startupToComment.Id);
            if (ModelState.IsValid)
            {
                _context.UserStartup.Add(startUp);
                _context.SaveChanges();
                return RedirectToAction("UserList", startupToComment);
            }
            else
            {
                return RedirectToAction("AddStartupComments");
            }
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
