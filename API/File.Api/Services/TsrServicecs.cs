using CsvHelper.Configuration;
using CsvHelper;
using File.Api.Models;
using Microsoft.Data.SqlClient;
using System.Globalization;
using File.Api.Helper;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace File.Api.Services
{
    public class TsrServicecs : ITsrService
    {
        public ReportingContext _reportingContext { get; set; }
        private IDbContextFactory<ReportingContext> _contextFactory;

        public TsrServicecs(ReportingContext reportingContext, IDbContextFactory<ReportingContext> contextFactory)
        {
            _reportingContext = reportingContext;
            _contextFactory = contextFactory;
        }

        public IQueryable<TransmissionStatusReport> GetQueryableTsrRecords(int takeCount = 4000000)
        {
            var data = _reportingContext.TransmissionStatusReports
                           .AsQueryable().OrderBy(x => x.Id).Take(takeCount);
            return data;
        }

        public List<TransmissionStatusReport> GetTsrRecords(int skipCount = 0, int takeCount = 0)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var data = context.TransmissionStatusReports
                           .AsQueryable().OrderBy(x => x.Id).Skip(skipCount).Take(takeCount).ToList();
                return data;
            }
        }
        
        public async Task WriteTsrRecordsUsingSqlCommand(string filePath, string connectionString, string query)
        {
            var records = new List<TransmissionStatusReport>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(query, connection)) 
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) 
                        {
                            while (reader.Read())
                            {
                                var record = new TransmissionStatusReport
                                {
                                    Id = (long)(reader["Id"]),
                                    PartitionToggleField = (int)reader["partition_toggle_field"],
                                    Sdhid = Converter.GetDbValue<int>(reader["SDHId"]),
                                    DocumentName = Converter.GetStringDbValue<string>(reader["DocumentName"]),
                                    DocumentHistoryStatus = Converter.GetStringDbValue<string>(reader["DocumentHistoryStatus"]),
                                    DocumentType = Converter.GetStringDbValue<string>(reader["DocumentType"]),
                                    DocumentUploadDate = Converter.GetDbValue<DateTime>(reader["DocumentUploadDate"]),
                                    PackageDate = Converter.GetDbValue<DateTime>(reader["PackageDate"]),
                                    EventDate = Converter.GetDbValue<DateTime>(reader["EventDate"]),
                                    DateSponsorNotified = Converter.GetDbValue<DateTime>(reader["DateSponsorNotified"]),
                                    CompoundId = Converter.GetDbValue<int>(reader["CompoundId"]),
                                    Compound = Converter.GetStringDbValue<string>(reader["Compound"]),
                                    Drug = Converter.GetStringDbValue<string>(reader["Drug"]),
                                    Mcn = Converter.GetStringDbValue<string>(reader["MCN"]),
                                    Slid = Converter.GetDbValue<int>(reader["SLId"]),
                                    SiteDetails = Converter.GetStringDbValue<string>(reader["SiteDetails"]),
                                    UserId = Converter.GetDbValue<int>(reader["UserId"]),
                                    UserName = Converter.GetStringDbValue<string>(reader["UserName"]),
                                    Spid = Converter.GetDbValue<int>(reader["SPId"]),
                                    RecipientName = Converter.GetStringDbValue<string>(reader["RecipientName"]),
                                    RecipientEmail = Converter.GetStringDbValue<string>(reader["RecipientEmail"]),
                                    RecipientCtprole = Converter.GetStringDbValue<string>(reader["RecipientCTPRole"]),
                                    TransmissionStatus = Converter.GetStringDbValue<string>(reader["TransmissionStatus"]),
                                    DocumentAccessStatusAtSite = Converter.GetStringDbValue<string>(reader["DocumentAccessStatusAtSite"]),
                                    NameOfInvestigator = Converter.GetStringDbValue<string>(reader["NameOfInvestigator"]),
                                    CrouserName = Converter.GetStringDbValue<string>(reader["CROUserName"]),
                                    CrouserAssigned = Converter.GetStringDbValue<string>(reader["CROUserAssigned"]),
                                    SitePhone = Converter.GetStringDbValue<string>(reader["SitePhone"]),
                                    SiteFax = Converter.GetStringDbValue<string>(reader["SiteFax"]),
                                    SiteMailingAddress = Converter.GetStringDbValue<string>(reader["SiteMailingAddress"]),
                                    CountryCod = Converter.GetStringDbValue<string>(reader["CountryCod"]),
                                    Country = Converter.GetStringDbValue<string>(reader["Country"]),
                                    Protocol = Converter.GetStringDbValue<string>(reader["Protocol"]),
                                    EventCountryId = Converter.GetDbValue<int>(reader["EventCountryId"]),
                                    EventCountry = Converter.GetStringDbValue<string>(reader["EventCountry"]),
                                    CountryId = Converter.GetDbValue<int>(reader["CountryId"]),
                                    SentTransmissionDate = Converter.GetDbValue<DateTime>(reader["SentTransmissionDate"]),
                                    UnanticipatedFlag = Converter.GetStringDbValue<string>(reader["UnanticipatedFlag"]),
                                    SponsorCausalityAssessmentFlag = Converter.GetStringDbValue<string>(reader["SponsorCausalityAssessmentFlag"]),
                                    EventProtocol = Converter.GetStringDbValue<string>(reader["EventProtocol"]),
                                    IsUnblindedDocument = Converter.GetDbValue<bool>(reader["IsUnblindedDocument"])
                                };

                                records.Add(record);
                            }
                        }
                    }
                }
                
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using var writer = new StreamWriter(fileStream);
                    using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
                    await csv.WriteRecordsAsync(records);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in WriteTsrRecordsUsingSqlCommand. Error details: " + ex.Message);
            }
        }

        public async Task<IList<TransmissionStatusReport>> GetTsrRecordsUsingSqlCommand(SqlConnection sqlConnection, string query)
        {
            var records = new List<TransmissionStatusReport>();

            try
            {
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandTimeout = 60;
                    command.CommandText = query;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var record = new TransmissionStatusReport
                            {
                                Id = (long)(reader["Id"]),
                                PartitionToggleField = (int)reader["partition_toggle_field"],
                                Sdhid = Converter.GetDbValue<int>(reader["SDHId"]),
                                DocumentName = Converter.GetStringDbValue<string>(reader["DocumentName"]),
                                DocumentHistoryStatus = Converter.GetStringDbValue<string>(reader["DocumentHistoryStatus"]),
                                DocumentType = Converter.GetStringDbValue<string>(reader["DocumentType"]),
                                DocumentUploadDate = Converter.GetDbValue<DateTime>(reader["DocumentUploadDate"]),
                                PackageDate = Converter.GetDbValue<DateTime>(reader["PackageDate"]),
                                EventDate = Converter.GetDbValue<DateTime>(reader["EventDate"]),
                                DateSponsorNotified = Converter.GetDbValue<DateTime>(reader["DateSponsorNotified"]),
                                CompoundId = Converter.GetDbValue<int>(reader["CompoundId"]),
                                Compound = Converter.GetStringDbValue<string>(reader["Compound"]),
                                Drug = Converter.GetStringDbValue<string>(reader["Drug"]),
                                Mcn = Converter.GetStringDbValue<string>(reader["MCN"]),
                                Slid = Converter.GetDbValue<int>(reader["SLId"]),
                                SiteDetails = Converter.GetStringDbValue<string>(reader["SiteDetails"]),
                                UserId = Converter.GetDbValue<int>(reader["UserId"]),
                                UserName = Converter.GetStringDbValue<string>(reader["UserName"]),
                                Spid = Converter.GetDbValue<int>(reader["SPId"]),
                                RecipientName = Converter.GetStringDbValue<string>(reader["RecipientName"]),
                                RecipientEmail = Converter.GetStringDbValue<string>(reader["RecipientEmail"]),
                                RecipientCtprole = Converter.GetStringDbValue<string>(reader["RecipientCTPRole"]),
                                TransmissionStatus = Converter.GetStringDbValue<string>(reader["TransmissionStatus"]),
                                DocumentAccessStatusAtSite = Converter.GetStringDbValue<string>(reader["DocumentAccessStatusAtSite"]),
                                NameOfInvestigator = Converter.GetStringDbValue<string>(reader["NameOfInvestigator"]),
                                CrouserName = Converter.GetStringDbValue<string>(reader["CROUserName"]),
                                CrouserAssigned = Converter.GetStringDbValue<string>(reader["CROUserAssigned"]),
                                SitePhone = Converter.GetStringDbValue<string>(reader["SitePhone"]),
                                SiteFax = Converter.GetStringDbValue<string>(reader["SiteFax"]),
                                SiteMailingAddress = Converter.GetStringDbValue<string>(reader["SiteMailingAddress"]),
                                CountryCod = Converter.GetStringDbValue<string>(reader["CountryCod"]),
                                Country = Converter.GetStringDbValue<string>(reader["Country"]),
                                Protocol = Converter.GetStringDbValue<string>(reader["Protocol"]),
                                EventCountryId = Converter.GetDbValue<int>(reader["EventCountryId"]),
                                EventCountry = Converter.GetStringDbValue<string>(reader["EventCountry"]),
                                CountryId = Converter.GetDbValue<int>(reader["CountryId"]),
                                SentTransmissionDate = Converter.GetDbValue<DateTime>(reader["SentTransmissionDate"]),
                                UnanticipatedFlag = Converter.GetStringDbValue<string>(reader["UnanticipatedFlag"]),
                                SponsorCausalityAssessmentFlag = Converter.GetStringDbValue<string>(reader["SponsorCausalityAssessmentFlag"]),
                                EventProtocol = Converter.GetStringDbValue<string>(reader["EventProtocol"]),
                                IsUnblindedDocument = Converter.GetDbValue<bool>(reader["IsUnblindedDocument"])
                            };

                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetTsrRecordsUsingSqlCommand. Error details: " + ex.Message);
            }

            return records;
        }
    }
}
