using BenchmarkDotNet.Running;
using BinaryPack.Benchmark.Implementation;
using BinaryPack.Models;

namespace BinaryPack.Benchmark;

public class Program
{
    public static void Main()
    {
        // Run via CLI: dotnet run --project BinaryPack.Benchmark.csproj -c Release --framework net472
        BenchmarkRunner.Run<Benchmark<JsonResponseModel>>();
    }
}
