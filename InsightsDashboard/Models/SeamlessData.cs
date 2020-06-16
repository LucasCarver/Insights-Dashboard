using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamlessApi.Models
{
    public class MainList
    {
        /*
         * MASTER LIST FIELDS
         */
        [JsonProperty(PropertyName = "Company Name")]
        public string CompanyName { get; set; }
        [JsonProperty(PropertyName = "Date Added")]
        public string DateAdded { get; set; }
        public string Scout { get; set; }
        public string Source { get; set; }
        [JsonProperty(PropertyName = "Company Website")]
        public string CompanyWebsite { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [JsonProperty(PropertyName = "Two Line Company Summary")]
        public string TwoLineCompanySummary { get; set; }
        public string Alignment { get; set; }
        [JsonProperty(PropertyName = "Theme(s)")]
        public string Themes { get; set; }
        public string Uniqueness { get; set; }
        public string Team { get; set; }
        public string Raised { get; set; }
        [JsonProperty(PropertyName = "Review Date")]
        public string ReviewDate { get; set; }
        [JsonProperty(PropertyName = "Technology Areas")]
        public string TechnologyAreas { get; set; }
        public string Landscape { get; set; }
        public string Stage { get; set; }
        [JsonProperty(PropertyName = "State/Province")]
        public string StateProvince { get; set; }
    }

    public class Feedback
    {
        /*
         * FEEDBACK FIELDS
         */
        public string IntroDate { get; set; }
        public string Startup { get; set; }
        public string YourOrganization { get; set; }
        public string Alignmenttoyourorganization { get; set; }
        public string UniquenessofTech { get; set; }
        public string TeamStrength { get; set; }
        public string BusinessPotential { get; set; }
        public string LevelofInterestinProject { get; set; }
        public string Questions { get; set; }
    }

    public class Projects
    {
        /*
         * PROJECTS FIELDS
         */
        public string ProjectName { get; set; }
        public string StartupEngaged { get; set; }
        public string PartnersInvolved { get; set; }
        public string ProjectSummary { get; set; }
        public string EngagementPhase { get; set; }
        public string EngagementProgress { get; set; }
        public string EngagementType { get; set; }
        public string OngoingStatus { get; set; }
        public string ProjectLead { get; set; }
        public string KickoffDate { get; set; }
        public string StatusSchedule { get; set; }
    }
}