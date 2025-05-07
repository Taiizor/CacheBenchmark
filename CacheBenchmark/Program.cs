using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using StackExchange.Redis;

namespace CacheBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<CacheBenchmarks>();
        }
    }

    [RankColumn]
    //[RPlotExporter]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RuntimeMoniker.HostProcess, launchCount: 3, warmupCount: 3, iterationCount: 3)]
    public class CacheBenchmarks
    {
        private string testValue;

        private IDatabase redisDb;
        private IDatabase garnetDb;
        private IDatabase dragonflyDb;

        private const int KeyCount = 1000;

        private ConnectionMultiplexer redisConnection;
        private ConnectionMultiplexer garnetConnection;
        private ConnectionMultiplexer dragonflyConnection;

        [GlobalSetup]
        public void Setup()
        {
            redisConnection = ConnectionMultiplexer.Connect("localhost:6380,allowAdmin=true,abortConnect=false");
            garnetConnection = ConnectionMultiplexer.Connect("localhost:6379,allowAdmin=true,abortConnect=false");
            dragonflyConnection = ConnectionMultiplexer.Connect("localhost:6381,allowAdmin=true,abortConnect=false");

            redisDb = redisConnection.GetDatabase();
            garnetDb = garnetConnection.GetDatabase();
            dragonflyDb = dragonflyConnection.GetDatabase();

            // 1KB data
            testValue = "test:value:" + new string('x', 1000);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            redisConnection.Dispose();
            garnetConnection.Dispose();
            dragonflyConnection.Dispose();
        }

        // Redis Benchmarks
        [Benchmark(Description = "Redis-SET")]
        public async Task RedisSet()
        {
            for (int i = 0; i < KeyCount; i++)
            {
                await redisDb.StringSetAsync($"redis:test:key:{i}", testValue);
            }
        }

        [Benchmark(Description = "Redis-GET")]
        public async Task RedisGet()
        {
            for (int i = 0; i < KeyCount; i++)
            {
                await redisDb.StringGetAsync($"redis:test:key:{i}");
            }
        }

        // Garnet Benchmarks
        [Benchmark(Description = "Garnet-SET")]
        public async Task GarnetSet()
        {
            for (int i = 0; i < KeyCount; i++)
            {
                await garnetDb.StringSetAsync($"garnet:test:key:{i}", testValue);
            }
        }

        [Benchmark(Description = "Garnet-GET")]
        public async Task GarnetGet()
        {
            for (int i = 0; i < KeyCount; i++)
            {
                await garnetDb.StringGetAsync($"garnet:test:key:{i}");
            }
        }

        // Dragonfly Benchmarks
        [Benchmark(Description = "Dragonfly-SET")]
        public async Task DragonflySet()
        {
            for (int i = 0; i < KeyCount; i++)
            {
                await dragonflyDb.StringSetAsync($"dragonfly:test:key:{i}", testValue);
            }
        }

        [Benchmark(Description = "Dragonfly-GET")]
        public async Task DragonflyGet()
        {
            for (int i = 0; i < KeyCount; i++)
            {
                await dragonflyDb.StringGetAsync($"dragonfly:test:key:{i}");
            }
        }
    }
}
