using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeamlessApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InsightsDashboard.Models
{
    public class SeamlessDAL
    {
        private readonly string _seamlessAPIKey;

        public SeamlessDAL(string seamlessAPIKey)
        {
            _seamlessAPIKey = seamlessAPIKey;
        }

        public HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://api.airtable.com/v0/appFo187B73tuYhyg/");
            return client;
        }

        public async Task<List<MainEntry>> GetMainEntryList()
        {
            var client = GetClient();
            var response = await client.GetAsync($"Master%20List?api_key={_seamlessAPIKey}");
            var result = await response.Content.ReadAsStringAsync();
            JObject jsonMainList = JObject.Parse(result);
            bool stop = false;
            JToken mainListEntry;
            int i = 0;
            List<MainEntry> mainEntryList = new List<MainEntry>();
            while (!stop)
            {
                try
                {
                    mainListEntry = jsonMainList["records"][i]["fields"];
                    var singleEntry = JsonConvert.DeserializeObject<MainEntry>(mainListEntry.ToString());
                    mainEntryList.Add(singleEntry);
                    i++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    stop = true;
                }
            }
            return mainEntryList;
        }

        public async Task<Feedback> GetFeedback()
        {
            var client = GetClient();
            var response = await client.GetAsync($"Feedback?api_key={_seamlessAPIKey}");
            var result = await response.Content.ReadAsAsync<Feedback>();
            return result;
        }

        public async Task<Projects> GetProjects()
        {
            var client = GetClient();
            var response = await client.GetAsync($"Projects?api_key={_seamlessAPIKey}");
            var result = await response.Content.ReadAsAsync<Projects>();
            return result;
        }
    }
}
