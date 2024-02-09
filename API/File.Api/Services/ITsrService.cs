using File.Api.Models;
using Microsoft.Data.SqlClient;

namespace File.Api.Services
{
    public interface ITsrService
    {
        IQueryable<TransmissionStatusReport> GetQueryableTsrRecords(int takeCount = 4000000);
        List<TransmissionStatusReport> GetTsrRecords(int skipCount = 0, int takeCount = 0);
        Task WriteTsrRecordsUsingSqlCommand(string filePath, string connectionString, string query);
        Task<IList<TransmissionStatusReport>> GetTsrRecordsUsingSqlCommand(SqlConnection sqlConnection, string query);
    }
}
