using addonlib;
using BenchmarkDotNet.Attributes;

namespace nullextension_benchmark
{
    public class NullTestBenchmarks
    {        
        private object[] _objs = new object [] { null, ""};
        private int counter;

        [Benchmark(Baseline = true)]
        public bool IsNull_Local_True()
        {
            return _objs[counter++ % 2].IsNullLocal();
        }

        [Benchmark]
        public bool IsNull_DLL()
        {
            return _objs[counter++ % 2].IsNullFromOtherDLL();
        }
    }
}
