using File.Api.Models;

namespace File.Api.Services
{
    public interface ITsrService
    {
        int GetRecordCount();
        IQueryable<TransmissionStatusReport> GetRecords(int takeCount = 4000000);
        List<TransmissionStatusReportShort> GetUdfRecords(int skipCount = 0, int takeCount = 0, bool showMostRecent = false);
        List<TransmissionStatusReport> GetRecordsWithContextFactory(int skipCount = 0, int takeCount = 0);
    }
}
