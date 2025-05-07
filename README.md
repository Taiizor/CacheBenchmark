# Cache Server Benchmark Project

This project benchmarks the performance of different cache servers (Redis, Garnet and Dragonfly) using Docker containers and C#.

## Prerequisites

- Windows OS
- Docker Desktop
- Docker Compose
- .NET SDK 10.0 or later

## Cache Servers Tested

- Redis (Latest)
- Garnet (Latest)
- Dragonfly (Latest)

## Project Structure

- `CacheBenchmark/` - C# benchmark project using BenchmarkDotNet
- `docker-compose.yml` - Docker configuration for cache servers

## How to Run

1. Start Docker containers:
```bash
docker-compose up -d
```

2. Run the benchmark:
```bash
cd CacheBenchmark
dotnet run -c Release
```

## Sample Benchmark Results

```plaintext
BenchmarkDotNet v0.14.1-nightly.20250107.205, Windows 11 (10.0.26100.3915)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.100-preview.3.25201.16
  [Host]     : .NET 10.0.0 (10.0.25.17105), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-QTZBJQ : .NET 10.0.0 (10.0.25.17105), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

IterationCount=3  LaunchCount=3  WarmupCount=3

| Method        | Mean     | Error    | StdDev   | Rank | Allocated  |
|-------------- |---------:|---------:|---------:|-----:|-----------:|
| Redis-GET     | 306.3 ms |  9.94 ms |  5.91 ms |    1 | 1427.76 KB |
| Redis-SET     | 367.7 ms | 23.85 ms | 14.19 ms |    1 |  403.16 KB |
| Garnet-SET    | 390.4 ms | 30.77 ms | 18.31 ms |    1 |  404.19 KB |
| Dragonfly-SET | 404.5 ms | 18.82 ms | 11.20 ms |    1 |   411.3 KB |
| Dragonfly-GET | 418.8 ms | 27.70 ms | 16.49 ms |    1 | 1433.59 KB |
| Garnet-GET    | 445.4 ms | 15.15 ms |  9.01 ms |    1 | 1426.48 KB |
```

## Test Configuration

- Each test performs 1000 operations
- Data size: 1KB per value
- Tests include both SET and GET operations
- All tests run with 3 iterations and 3 warmup counts

## Performance Summary

1. Redis shows the best overall performance:
   - Fastest GET operations (490.6 ms)
   - Efficient SET operations (531.1 ms)

2. Dragonfly shows consistent performance:
   - Similar timing for both GET and SET (around 566 ms)
   - Slightly higher memory allocation

3. Garnet:
   - Competitive SET performance (553.0 ms)
   - Slower GET operations (622.1 ms)
   - Efficient memory usage for SET operations

## Memory Usage

- SET operations use ~400-410 KB
- GET operations use ~1.4 MB
- Memory usage is similar across all cache servers

## License

MIT

## Contributing

Feel free to submit issues and pull requests.