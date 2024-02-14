using File.Api.Models;

namespace File.Api.Services
{
    public interface ITsrService
    {
        int GetRecordCount();
        IQueryable<TransmissionStatusReport> GetRecords(int takeCount = 4000000);
        List<TransmissionStatusReportShort> GetUdfRecords(bool showMostRecent = true);
        List<TransmissionStatusReport> GetRecordsWithContextFactory(int skipCount = 0, int takeCount = 0);
    }
}
