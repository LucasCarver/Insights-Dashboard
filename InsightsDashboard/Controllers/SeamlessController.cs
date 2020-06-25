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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public async Task<IActionResult> UserFavorites()
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

            if (TempData["Status"] != null)
            {
                ViewBag.Status = TempData["Status"].ToString();
            }

            return View(finalDisplayList);
        }

        [Authorize]
        public IActionResult RemoveUserStartUp(int id)
        {
            UserStartup removeStartUp = _context.UserStartup.Find(id);

            List<StartupComments> c = _context.StartupComments.Where(x => x.StartupId == id).Where(y => y.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
            foreach (StartupComments y in c)
            {
                _context.StartupComments.Remove(y);
            }
            // StartupComments conflict column startupId
            _context.UserStartup.Remove(removeStartUp);
            _context.SaveChanges();
            return RedirectToAction("UserFavorites");
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
                startUp.DateAdded = startUp.DateAdded;
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

                _context.Entry(startUp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(startUp);
                _context.SaveChanges();
            }
            return RedirectToAction("UserFavorites");
        }

        public async Task<IActionResult> SeamlessStartupDetails(string key)
        {
            if (key == null)
            {
                return RedirectToAction("DisplaySeamlessStartup");
            }
         
            Dictionary<string, MainEntry> seamlessDictionary = await _seamlessDAL.GetMainDictionary();
            KeyValuePair<string, MainEntry> startupDetails = new KeyValuePair<string, MainEntry>(key, seamlessDictionary[key]);
            
            List<UserStartup> definedUserStartUps = new List<UserStartup>();
            string uId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            definedUserStartUps = _context.UserStartup.Where(x => x.UserId == uId).Where(x => x.Identifier == null).ToList();
            List<UserStartup> userSavedSeamlessStartups = _context.UserStartup.Where(x => x.UserId == uId).Where(x => x.Identifier != null).ToList();
            List<UserStartup> finalDisplayList = new List<UserStartup>();
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

           foreach(UserStartup us in finalDisplayList)
            {

                if (startupDetails.Value.CompanyName == us.CompanyName)
                {
                    int id = us.Id;   
                    return RedirectToAction("StartupDetails",new { id = id });
                }
            }
            return View(startupDetails);
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
            List<UserStartup> userStartups = await _context.UserStartup.Where(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync();
            bool found = false;
            foreach (UserStartup y in userStartups)
            {
                if (y.CompanyName == us.CompanyName)
                {
                    found = true;
                }
            }
            if (!found)
            {
                _context.UserStartup.Add(us);
                _context.SaveChanges();
                return RedirectToAction("UserFavorites");
            }
            else
            {
                TempData["Status"] = "This Favorite has already been added!";

                return RedirectToAction("UserFavorites");
            }
        }

        public async Task<IActionResult> TechDetails(string Tech)
        {
            TempData["Tech"] = Tech;

            Dictionary<string, MainEntry> seamlessDictionary = await _seamlessDAL.GetMainDictionary();
            Dictionary<string, MainEntry> techDictionary = new Dictionary<string, MainEntry>();
            string[] techs = new string[] { };
            int i = 0;
            foreach (KeyValuePair<string, MainEntry> x in seamlessDictionary)
            {
                try
                {
                    if (x.Value.Themes != null)
                    {
                        i++;
                        string y = x.Value.Themes;
                        techs = y.Split(',');
                        x.Value.Themes = "";
                        foreach (string s in techs)
                        {

                            x.Value.Themes += s.Trim() + " ";
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }

                if (x.Value.Themes != null)
                {
                    if (x.Value.Themes.Contains(Tech))
                    {
                        techDictionary.Add(x.Key, x.Value);
                    }
                }

                try
                {
                    if (x.Value.Landscape != null)
                    {
                        i++;
                        string y = x.Value.Landscape;
                        techs = y.Split(',');
                        x.Value.Landscape = "";
                        foreach (string s in techs)
                        {

                            x.Value.Landscape += s.Trim() + " ";
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }

                if (x.Value.Landscape != null)
                {
                    if (x.Value.Landscape.Contains(Tech))
                    {
                        techDictionary.Add(x.Key, x.Value);
                    }
                }

                try
                {
                    if (x.Value.TechnologyAreas != null)
                    {
                        i++;
                        string y = x.Value.TechnologyAreas;
                        techs = y.Split(',');
                        x.Value.TechnologyAreas = "";
                        foreach (string s in techs)
                        {

                            x.Value.TechnologyAreas += s.Trim() + " ";
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }

                if (x.Value.TechnologyAreas != null)
                {
                    if (x.Value.TechnologyAreas.Contains(Tech))
                    {
                        techDictionary.Add(x.Key, x.Value);
                    }
                }
            }
            if (TempData["Tech"] != null)
            {
                ViewBag.Tech = TempData["Tech"].ToString();
                return View(techDictionary);
            }

            return View(techDictionary);
        }

        public async Task<IActionResult> AlignmentDetails(string alignment)
        {
            TempData["Alignment"] = alignment;

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

            if (TempData["Alignment"] != null)
            {
                ViewBag.Alignment = TempData["Alignment"].ToString();
                return View(alignmentDictionary);
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
                        foreach (string s in alignments)
                        {

                            x.Value.Alignment += s.Trim() + " ";
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
            if (TempData["Status"] != null)
            {
                ViewBag.Status = TempData["Status"].ToString();
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
            UserStartupComments userStartupComments = new UserStartupComments()
            {
                Startups = specificStartup,
                Comments = _context.StartupComments.Where(x => x.StartupId == id).ToList(),
            };
            return View(userStartupComments);
        }

        public IActionResult AddStartupComments(int id, string comment)
        {
            StartupComments startupComment = new StartupComments();
            startupComment.StartupId = id;
            startupComment.Comment = comment;
            startupComment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                _context.StartupComments.Add(startupComment);
                _context.SaveChanges();
                return RedirectToAction("StartupDetails", new { id = id });
            }
            else
            {
                return RedirectToAction("AddStartupComments");
            }
        }

        public IActionResult RemoveStartupRatings(int id)
        {
            UserStartup specificStartup = _context.UserStartup.Find(id);
            StartupComments userStartupComment = _context.StartupComments.Find(id);

            if (ModelState.IsValid)
            {
                _context.StartupComments.Remove(userStartupComment);
                _context.SaveChanges();

                return RedirectToAction("UserFavorites");
            }
            else
            {
                return RedirectToAction("DisplaySeamlessStartups");
            }
        }

        public IActionResult RemoveStartupComments(int id)
        {
            UserStartup specificStartup = _context.UserStartup.Find(id);
            StartupComments userStartupComment = _context.StartupComments.Find(id);

            if (ModelState.IsValid)
            {
                _context.StartupComments.Remove(userStartupComment);
                _context.SaveChanges();
              
                return RedirectToAction("UserFavorites");
            }
            else
            {
                return RedirectToAction("DisplaySeamlessStartups");
            }
        }

        public IActionResult AddStartupRating(int id, int rating)
        {
            StartupComments startupComment = new StartupComments();
            startupComment.StartupId = id;
            startupComment.Rating = rating;
            startupComment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                _context.StartupComments.Update(startupComment);
                _context.SaveChanges();
                return RedirectToAction("StartupDetails", new { id = id });
            }
            else
            {
                return RedirectToAction("AddStartupRating");
            }
        }

        public async Task<IActionResult> Keywords(string sortType)
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
            List<string> allWords = inputWords.Split(invalidChars).ToList();
            Dictionary<string, int> wordFreq = new Dictionary<string, int>();

            // REMOVE COMMON WORDS
            List<string> stopList = new List<string>();
            List<StopList> sqlStopList = _context.StopList.Where(x => x.StopWord.Length > 0).ToList();
            foreach (StopList stopListObject in sqlStopList)
            {
                stopList.Add(stopListObject.StopWord);
            }
            foreach (string stopWord in stopList)
            {
                while (allWords.Remove(stopWord)) { }
            }
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
            List<KeyValuePair<string, int>> keywordList = new List<KeyValuePair<string, int>>();
            // ORDER BY BLOCK
            if (sortType == "keyword")
            {
                keywordList = wordFreq.OrderBy(key => key.Key).ToList<KeyValuePair<string, int>>();
            }
            else
            {
                keywordList = wordFreq.OrderByDescending(key => key.Value).ToList<KeyValuePair<string, int>>();
            }
            return View(keywordList);
        }

        public async Task<IActionResult> KeywordDetails(string keyword, int frequency)
        {
            KeyValuePair<string, int> keywordDetails = new KeyValuePair<string, int>(keyword, frequency);
            // FIND ALL STARTUP OCCURENCES OF THAT WORD
            Dictionary<string, MainEntry> occurrences = await FindKeywordOccurrences(keyword);
            Dictionary<string, MainEntry> sortedOccurrences = occurrences.OrderBy(x => DateTime.Parse(x.Value.DateAdded)).ToDictionary(z => z.Key, y => y.Value);
            KeywordDetailsAndOccurrences viewModel = new KeywordDetailsAndOccurrences()
            {
                KeywordDetails = keywordDetails,
                Occurrences = sortedOccurrences
            };
            return View(viewModel);
        }

        public async Task<Dictionary<string, MainEntry>> FindKeywordOccurrences(string keyword)
        {
            Dictionary<string, MainEntry> mainDictionary = await _seamlessDAL.GetMainDictionary();
            List<string> twoLineWords;
            Dictionary<string, MainEntry> subDictionary = new Dictionary<string, MainEntry>();
            char[] invalidChars = " !@#$%^&*()_+“”~{}|:\"<>?`1234567890-=[]\\;',./".ToCharArray();
            foreach (var kvp in mainDictionary)
            {
                twoLineWords = kvp.Value.TwoLineCompanySummary.ToLower().Split(invalidChars).ToList();
                foreach (string word in twoLineWords)
                {
                    try
                    {
                        if (word == keyword)
                        {
                            subDictionary.Add(kvp.Key, kvp.Value);
                        }
                    }
                    catch (ArgumentException) { }
                }
            }
            return subDictionary;
        }

        public IActionResult DisplayCustomStartups()
        {
            List<UserStartup> userStartupList = new List<UserStartup>();
            userStartupList = _context.UserStartup.Where(x => x.Identifier == null).ToList();
            List<string> usernameList = new List<string>();
            foreach (UserStartup us in userStartupList)
            {
                usernameList.Add(_context.AspNetUsers.Find(us.UserId).UserName);
            }
            CustomStartupVM customStartupVM = new CustomStartupVM();
            customStartupVM.UserNameList = usernameList;
            customStartupVM.UserStartupList = userStartupList;
            return View(customStartupVM);
        }

        // THIS METHOD WAS USED TO PUT THE STOPLIST INTO A SQL TABLE FROM A TEXT FILE
        //public void PutStoplistInTable()
        //{
        //    StreamReader streamReader = new StreamReader("wwwroot\\SmartStoplist.txt");
        //    List<string> stopList = new List<string>();
        //    // DISCARD THE FIRST LINE
        //    streamReader.ReadLine();
        //    string line;
        //    while ((line = streamReader.ReadLine()) != null)
        //    {
        //        stopList.Add(line.Trim().ToLower());
        //    }
        //    streamReader.Close();

        //    StopList stopList1 = new StopList();
        //    foreach (string word in stopList)
        //    {
        //        // ADD THE WORD TO THE STOPLIST TABLE
        //        stopList1.StopWord = word;
        //        _context.StopList.Add(stopList1);
        //        _context.SaveChanges();
        //    }
        //}
    }
}
