using System;
using System.Collections.Generic;

namespace File.Api.Models
{
    public partial class TransmissionStatusReport
    {
        public long Id { get; set; }
        public int PartitionToggleField { get; set; }
        public int? Sdhid { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentHistoryStatus { get; set; }
        public string? DocumentType { get; set; }
        public DateTime? DocumentUploadDate { get; set; }
        public DateTime? PackageDate { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? DateSponsorNotified { get; set; }
        public int? CompoundId { get; set; }
        public string? Compound { get; set; }
        public string? Drug { get; set; }
        public string? Mcn { get; set; }
        public int? Slid { get; set; }
        public string? SiteDetails { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? Spid { get; set; }
        public string? RecipientName { get; set; }
        public string? RecipientEmail { get; set; }
        public string? RecipientCtprole { get; set; }
        public string? TransmissionStatus { get; set; }
        public string? DocumentAccessStatusAtSite { get; set; }
        public string? NameOfInvestigator { get; set; }
        public string? CrouserName { get; set; }
        public string? CrouserAssigned { get; set; }
        public string? SitePhone { get; set; }
        public string? SiteFax { get; set; }
        public string? SiteMailingAddress { get; set; }
        public string? CountryCod { get; set; }
        public string? Country { get; set; }
        public string? Protocol { get; set; }
        public int? EventCountryId { get; set; }
        public string? EventCountry { get; set; }
        public int? CountryId { get; set; }
        public DateTime? SentTransmissionDate { get; set; }
        public string? UnanticipatedFlag { get; set; }
        public string? SponsorCausalityAssessmentFlag { get; set; }
        public string? EventProtocol { get; set; }
        public bool? IsUnblindedDocument { get; set; }
    }
}
