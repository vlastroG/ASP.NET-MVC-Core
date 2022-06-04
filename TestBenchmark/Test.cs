using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBenchmark
{
    [MemoryDiagnoser]
    public class Test
    {
        [Benchmark(Baseline = true)]
        public void TestFirst()
        {

        }

        [Benchmark]
        public void TestSecond()
        {

        }
    }
}
