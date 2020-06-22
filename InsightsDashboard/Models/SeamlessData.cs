using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightsDashboard.Models
{
    public class MainEntry
    {
        /*
         * MAIN ENTRY FIELDS
         */
        [JsonProperty(PropertyName = "Company Name")]
        public string CompanyName { get; set; }
        [JsonProperty(PropertyName = "Date Added")]
        public string DateAdded { get; set; }
        [JsonProperty(PropertyName = "Scout")]
        public string Scout { get; set; }
        [JsonProperty(PropertyName = "Source")]
        public string Source { get; set; }
        [JsonProperty(PropertyName = "Company Website")]
        public string CompanyWebsite { get; set; }
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "Two Line Company Summary")]
        public string TwoLineCompanySummary { get; set; }
        [JsonProperty(PropertyName = "Alignment")]
        public string Alignment { get; set; }
        [JsonProperty(PropertyName = "Theme(s)")]
        public string Themes { get; set; }
        [JsonProperty(PropertyName = "Uniqueness")]
        public string Uniqueness { get; set; }
        [JsonProperty(PropertyName = "Team")]
        public string Team { get; set; }
        [JsonProperty(PropertyName = "Raised")]
        public string Raised { get; set; }
        [JsonProperty(PropertyName = "Review Date")]
        public string ReviewDate { get; set; }
        [JsonProperty(PropertyName = "Technology Areas")]
        public string TechnologyAreas { get; set; }
        [JsonProperty(PropertyName = "Landscape")]
        public string Landscape { get; set; }
        [JsonProperty(PropertyName = "Stage")]
        public string Stage { get; set; }
        [JsonProperty(PropertyName = "State/Province")]
        public string StateProvince { get; set; }
    }

    public class Feedback
    {
        /*
         * FEEDBACK FIELDS
         */
        [JsonProperty(PropertyName = "Intro Date")]
        public string IntroDate { get; set; }
        [JsonProperty(PropertyName = "Startup")]
        public string Startup { get; set; }
        [JsonProperty(PropertyName = "Your Organization")]
        public string YourOrganization { get; set; }
        [JsonProperty(PropertyName = "Alignment to your organization")]
        public string Alignmenttoyourorganization { get; set; }
        [JsonProperty(PropertyName = "Uniqueness")]
        public string UniquenessofTech { get; set; }
        [JsonProperty(PropertyName = "Team Strength")]
        public string TeamStrength { get; set; }
        [JsonProperty(PropertyName = "Business Potential")]
        public string BusinessPotential { get; set; }
        [JsonProperty(PropertyName = "Level of Interest in Project")]
        public string LevelofInterestinProject { get; set; }
        [JsonProperty(PropertyName = "Questions")]
        public string Questions { get; set; }
    }

    public class Projects
    {
        /*
         * PROJECTS FIELDS
         */
        [JsonProperty(PropertyName = "Project Name")]
        public string ProjectName { get; set; }
        [JsonProperty(PropertyName = "Startup Engaged")]
        public string StartupEngaged { get; set; }
        [JsonProperty(PropertyName = "Partners Involved")]
        public string PartnersInvolved { get; set; }
        [JsonProperty(PropertyName = "Project Summary")]
        public string ProjectSummary { get; set; }
        [JsonProperty(PropertyName = "Engagement Phase")]
        public string EngagementPhase { get; set; }
        [JsonProperty(PropertyName = "Property Name")]
        public string EngagementProgress { get; set; }
        [JsonProperty(PropertyName = "Enagagement Type")]
        public string EngagementType { get; set; }
        [JsonProperty(PropertyName = "Ongoing Status")]
        public string OngoingStatus { get; set; }
        [JsonProperty(PropertyName = "Project Lead")]
        public string ProjectLead { get; set; }
        [JsonProperty(PropertyName = "Kickoff Date")]
        public string KickoffDate { get; set; }
        [JsonProperty(PropertyName = "Status Schedule")]
        public string StatusSchedule { get; set; }
    }
}