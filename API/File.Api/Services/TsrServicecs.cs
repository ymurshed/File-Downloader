using File.Api.Models;
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

        public int GetRecordCount()
        {
            return _reportingContext.TransmissionStatusReports.Count();
        }

        public IQueryable<TransmissionStatusReport> GetRecords(int takeCount = 4000000)
        {
            var data = _reportingContext.TransmissionStatusReports.AsQueryable().OrderBy(x => x.Id).Take(takeCount);
            return data;
        }

        public List<TransmissionStatusReportShort> GetUdfRecords(int skipCount = 0, int takeCount = 0, bool showMostRecent = false)
        {
            using var context = _contextFactory.CreateDbContext();
            var data = context.udf_GetTransmissionStatusReport(showMostRecent).OrderBy(x => x.Id).Skip(skipCount).Take(takeCount).ToList();
            return data;
        }

        public List<TransmissionStatusReport> GetRecordsWithContextFactory(int skipCount = 0, int takeCount = 0)
        {
            using var context = _contextFactory.CreateDbContext();
            var data = context.TransmissionStatusReports.AsQueryable().OrderBy(x => x.Id).Skip(skipCount).Take(takeCount).ToList(); // ToList() faster than ToListAsync()
            return data;
        }
    }
}
