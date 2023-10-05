namespace SFA.DAS.Support.Indexer.Worker
{
    public class Program
    {
        static void Main(string[] args)
        {
            var workerRole = new WorkerRole();
            workerRole.OnStart();
            workerRole.Run();
        }
    }
}
