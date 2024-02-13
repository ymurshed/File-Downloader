namespace File.Api.Models
{
    public class TaskModel
    {
        public int TaskId { get; set; }
        public int Offset { get; set; }
        public int BatchSize { get; set; }
        public Task<List<TransmissionStatusReport>>? Tsr { get; set; }

        public TaskModel(int taskId, int offset, int batchSize, Task<List<TransmissionStatusReport>>? tsr)
        {
            TaskId = taskId;
            Offset = offset;
            BatchSize = batchSize;
            Tsr = tsr;
        }
    }
}
