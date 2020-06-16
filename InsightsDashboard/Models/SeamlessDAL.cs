﻿using Microsoft.Extensions.Configuration;
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

        public async Task<SeamlessData> GetMasterList()
        {
            var client = GetClient();
            var response = await client.GetAsync($"Master%20List?api_key={_seamlessAPIKey}");
            var result = await response.Content.ReadAsAsync<SeamlessData>();
            return result;
        }

        public async Task<SeamlessData> GetFeedback()
        {
            var client = GetClient();
            var response = await client.GetAsync($"Feedback?api_key={_seamlessAPIKey}");
            var result = await response.Content.ReadAsAsync<SeamlessData>();
            return result;
        }
        
        public async Task<SeamlessData> GetProjects()
        {
            var client = GetClient();
            var response = await client.GetAsync($"Projects?api_key={_seamlessAPIKey}");
            var result = await response.Content.ReadAsAsync<SeamlessData>();
            return result;
        }
    }
}
