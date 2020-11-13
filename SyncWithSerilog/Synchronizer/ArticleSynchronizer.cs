using System;

namespace SyncWithSerilog.Synchronizer
{
    public class ArticleSynchronizer
    {
        public void Run() => RandomAction()();

        private Action RandomAction()
            => new Random().Next(4) switch
            {
                0 => () => Console.WriteLine("Successful upload"),
                1 => () => Console.WriteLine("Conversion error"),
                _ => () => Console.WriteLine("Something happened")
            };
    }
}
