using File.Api.Models;
using Microsoft.Data.SqlClient;

namespace File.Api.Services
{
    public interface ITsrService
    {
        IQueryable<TransmissionStatusReport> GetRecords(int takeCount = 4000000);
        List<TransmissionStatusReport> GetRecordsWithContextFactory(int skipCount = 0, int takeCount = 0);
        Task<IList<TransmissionStatusReport>> GetRecordsUsingSqlCommand(SqlConnection sqlConnection, string query);
    }
}
