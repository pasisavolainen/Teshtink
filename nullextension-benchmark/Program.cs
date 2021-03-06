﻿using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace nullextension_benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //nulltest();
            //conntest();
            fddbench();
        }

        private static void fddbench()
        {
            var summary = BenchmarkRunner.Run<FloatDoubleDecimalBench>(
                ManualConfig.Create(DefaultConfig.Instance)
                            //.With(Job.Dry)
                            );
        }

        static void nulltest()
        {
            var summary = BenchmarkRunner.Run<NullTestBenchmarks>(
                ManualConfig.Create(DefaultConfig.Instance)
                            .With(Job.Dry)
                            .With(new InliningDiagnoser()));
        }
        static void conntest()
        {
            var summary = BenchmarkRunner.Run<ConnectionPoolingBenchmark>(
                ManualConfig.Create(DefaultConfig.Instance)
                            //.With(Job.Dry)
                            );
        }
    }
}
