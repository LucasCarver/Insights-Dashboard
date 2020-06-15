using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeamlessApi.Models
{

    public class SeamlessData
    {
        public Record[] Records { get; set; }
    }

    public class Record
    {
        public string Id { get; set; }
        public MasterList MainList { get; set; }
        public Feedback Feedback { get; set; }
        public Projects Projects { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public class MasterList
    {
        /*
         * MASTER LIST FIELDS
         */
        public string CompanyName { get; set; }
        public string DateAdded { get; set; }
        public string Scout { get; set; }
        public string Source { get; set; }
        public string CompanyWebsite { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string TwoLineCompanySummary { get; set; }
        public string Alignment { get; set; }
        public string Themes { get; set; }
        public string Uniqueness { get; set; }
        public string Team { get; set; }
        public string Raised { get; set; }
        public string ReviewDate { get; set; }
        public string TechnologyAreas { get; set; }
        public string Landscape { get; set; }
        public string Stage { get; set; }
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