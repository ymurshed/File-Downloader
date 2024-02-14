using System.ComponentModel;

namespace File.Api.Models
{
    public class TransmissionStatusReportShort
    {
        [Description("DocumentName")]
        public string DocumentName { get; set; }

        [Description("UnanticipatedFlag")]
        public string UnanticipatedFlag { get; set; }

        [Description("SponsorCausalityAssessmentFlag")]
        public string SponsorCausalityAssessmentFlag { get; set; }

        [Description("CountryOfSUSAREvent")]
        public string CountryOfSusarEvent { get; set; }

        [Description("DocumentVersion")]
        public string DocumentVersion { get; set; }

        [Description("DocumentUploadDate")]
        public DateTime? DocumentUploadDate { get; set; }

        [Description("SentTransmissionDate")]
        public DateTime? SentTransmissionDate { get; set; }

        [Description("PackageDate_DueDate")]
        public DateTime? PackageDate { get; set; }

        [Description("Compound")]
        public string Compound { get; set; }

        [Description("EventProtocol")]
        public string EventProtocol { get; set; }

        [Description("SiteDetails")]
        public string SiteDetails { get; set; }

        [Description("RecipientName_LastFirst")]
        public string RecipientName { get; set; }

        [Description("RecipientEmail")]
        public string RecipientEmail { get; set; }

        [Description("UserName")]
        public string UserName { get; set; }

        [Description("RecipientRole")]
        public string RecipientRole { get; set; }

        [Description("TransmissionStatus_SentFailed")]
        public string TransmissionStatus { get; set; }

        [Description("IsUnblindedDocument")]
        public bool? IsUnblindedDocument { get; set; }

        [Description("DocumentViewStatusAtSite")]
        public string DocumentViewStatusAtSite { get; set; }

        [Description("NameOfInvestigator")]
        public string NameOfInvestigator { get; set; }

        [Description("MonitorOrCRAAssigned")]
        public string MonitorCraAssigned { get; set; }

        [Description("SitePhone")]
        public string SitePhone { get; set; }

        [Description("SiteFax")]
        public string SiteFax { get; set; }

        [Description("SiteMailingAddress")]
        public string SiteMailingAddress { get; set; }

        [Description("ProtocolNumberDistributedTo")]
        public string ProtocolDistributedTo { get; set; }

        [Description("CountryDistributedTo")]
        public string CountryDistributedTo { get; set; }
    }
}
