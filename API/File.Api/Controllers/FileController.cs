using CsvHelper;
using CsvHelper.Configuration;
using File.Api.Models;
using File.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostEnvironment;

namespace File.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FileController : ControllerBase
    {
        #region constans
        public const string FileType = "text/csv";

        public bool FetchFullData = false;
        public int IterationCount = 3;
        public int BatchSize = 400000;
        public int NumberOfTasks = 10;
        
        private ITsrService _tsrService { get; set; }
        public ILogger<FileController> _logger { get; set; }
        public IHostingEnvironment _hostingEnvironment { get; set; }

        public ConfigurationManager _configurationManager { get; set; }
        #endregion

        public FileController(ILogger<FileController> logger, 
                              IHostingEnvironment hostingEnvironment, 
                              ConfigurationManager configurationManager,
                              ITsrService tsrService)
        {
            _logger = logger;
            _tsrService = tsrService;
            _hostingEnvironment = hostingEnvironment;
            _configurationManager = configurationManager;
        }

        #region Using FileStream + Thread
        [HttpGet("Download_TSR_V2")]
        public async Task<ActionResult> Download_TSR_V2()
        {
            var fstart = DateTime.Now;
            var donwloadFileName = $"{Guid.NewGuid()}.csv";
            var filePath = Path.Join(_hostingEnvironment.ContentRootPath, "wwwroot", donwloadFileName);

            try
            {
                _logger.LogInformation($"\nInvoking V2 API (FileStream) --->>>\n");

                #region DB + CSV operation
                var startOffset = 0;

                var count = FetchFullData ? _tsrService.GetRecordCount() : IterationCount * NumberOfTasks * BatchSize;
                
                while (startOffset < count)
                {
                    var taskModels = await ReadDataInBulkWithEfCoreAsync(startOffset);
                    var results = taskModels.OrderBy(x => x.TaskId).Select(x => x.Tsr!.Result).ToArray();

                    if (results == null || !results.Any()) break;

                    if (startOffset == 0)
                    {
                        await SaveDataInCsv(results, filePath, true);
                    }
                    else 
                    {
                        await SaveDataInCsv(results, filePath);
                    }

                    startOffset += results.Sum(x => x.Count);
                }
                #endregion

                #region Send file stream
                _logger.LogInformation($"Start sending filestream at: {fstart}");

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
                var fileStreamResult = new FileStreamResult(fileStream, FileType)
                {
                    FileDownloadName = donwloadFileName,
                    EnableRangeProcessing = true // Enable range requests for resumable downloads
                };

                return fileStreamResult;
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError("\nError in file download".ToString() + ex.Message + "\n", ex.Message);
                return Content("Error occurred while processing the file.");
            }
            finally
            {
                var fend = DateTime.Now;
                var totalTime = (fend - fstart).TotalSeconds;
                _logger.LogInformation($"Completed sending filestream at: {fend}. Total time in secs: {totalTime}");
            }
        }

        private async Task<IList<TaskModel>> ReadDataInBulkWithEfCoreAsync(int startOffset, bool useAsyncDbCall = false)
        {
            var taskModels = new List<TaskModel>();
            
            var startTime = DateTime.Now;
            _logger.LogInformation($"\nStarted reading data from TSR table at: {startTime} using batchSize = {BatchSize}, numberOfTasks = {NumberOfTasks}, total rows = {BatchSize * NumberOfTasks}.\n");

            for (int i = 0; i < NumberOfTasks; i++)
            {
                var offset = startOffset + (i * BatchSize);
                
                if (useAsyncDbCall) // slower
                {
                    var task = Task.Run(async () => await _tsrService.GetRecordsWithContextFactoryAsync(offset, BatchSize));
                    var taskModel = new TaskModel(i, offset, BatchSize, task);
                    taskModels.Add(taskModel);
                }
                else // faster
                {
                    var task = Task.Run(() => _tsrService.GetRecordsWithContextFactory(offset, BatchSize));
                    var taskModel = new TaskModel(i, offset, BatchSize, task);
                    taskModels.Add(taskModel);
                }
            }

            var allTasks = taskModels.Select(x => x.Tsr).ToArray();
            var results = await Task.WhenAll(allTasks!);

            var endTime = DateTime.Now;
            _logger.LogInformation($"\nCompleted reading data from TSR table at: {endTime}. Total time taken: {(endTime - startTime).TotalSeconds} secs.\n");

            return taskModels;
        }

        private async Task<IList<TransmissionStatusReport>[]?> ReadDataInBulkWithSqlCommand()
        {
            var batchSize = 500000;
            var numberOfTasks = 4;

            IList<TransmissionStatusReport>[]? results = null;
            var tasks = new List<Task<IList<TransmissionStatusReport>>>();
            
            var connectionString = _configurationManager.GetConnectionString("ReportingConnection");

            var startTime = DateTime.Now;
            _logger.LogInformation($"\nStarted reading data from TSR table at: {startTime}\n");

            using (var connection = new SqlConnection(connectionString)) 
            {
                await connection.OpenAsync();

                for (int i = 0; i < numberOfTasks; i++)
                {
                    var offset = i * batchSize;
                    var query = $"select * from [SafetyReporting].[dbo].[TransmissionStatusReport] order by Id offset {offset} rows fetch next {batchSize} rows only;";

                    tasks.Add(_tsrService.GetRecordsUsingSqlCommand(connection, query));
                }

                results = await Task.WhenAll(tasks);
            }

            var endTime = DateTime.Now;
            _logger.LogInformation($"\nCompleted reading data from TSR table at: {endTime}. Total time taken: {(endTime - startTime).TotalSeconds} secs.\n");

            return results;
        }

        private async Task SaveDataInCsv(List<TransmissionStatusReport>[] results, string filePath, bool isFirstBatch = false)
        {
            var startTime = DateTime.Now;
            _logger.LogInformation($"\nStarted writing data in CSV file at: {startTime}\n");

            using (var fileStream = new FileStream(filePath, FileMode.Append))
            {
                using var writer = new StreamWriter(fileStream);

                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };

                if (isFirstBatch) 
                {
                    csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
                }
                
                using var csv = new CsvWriter(writer, csvConfig);

                foreach (var result in results)
                {
                    await csv.WriteRecordsAsync(result);
                }
            }

            var endTime = DateTime.Now;
            _logger.LogInformation($"\nCompleted writing data in CSV file at: {endTime}. Total time taken: {(endTime - startTime).TotalSeconds} secs.\n");
        }
        #endregion

        
        #region Using MemoryStream
        [HttpGet("Download_TSR_V1")]
        public IActionResult Download_TSR_V1()
        {
            try
            {
                _logger.LogInformation($"\nInvoking V1 API (MemoryStream) --->>>\n");

                var result = ExportData_V1();

                if (result != null)
                {
                    var fileName = $"{Guid.NewGuid()}.csv";
                    return File((MemoryStream)result, FileType, fileName);
                }

                return Ok(null);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in file download".ToString() + ex.Message + ex.InnerException, ex);
                return StatusCode(500, ex.Message);
            }
        }

        private object ExportData_V1()
        {
            var stream = new MemoryStream();

            var writeFile = new StreamWriter(stream);
            var csv = new CsvWriter(writeFile, CultureInfo.InvariantCulture);

            #region DB operation
            var startTime = DateTime.Now;
            _logger.LogInformation($"\nStarted reading data from TSR table at: {startTime} using batchSize = {BatchSize}.\n");

            var transmissionStatusReports = _tsrService.GetRecords(BatchSize).ToList();

            var endTime = DateTime.Now;
            _logger.LogInformation($"\nCompleted reading data from TSR table at: {endTime}. Total time taken: {(endTime - startTime).TotalSeconds} secs.\n");
            #endregion

            #region CSV operation
            startTime = DateTime.Now;
            _logger.LogInformation($"\nStarted writing data in CSV at: {startTime}\n");

            csv.WriteRecords(transmissionStatusReports);
            writeFile.Flush();

            stream.Position = 0;
            object content = stream;

            endTime = DateTime.Now;
            _logger.LogInformation($"\nCompleted writing data in CSV at: {endTime}. Total time taken: {(endTime - startTime).TotalSeconds} secs.\n");
            #endregion 

            return content;
        }
        #endregion
    }
}
