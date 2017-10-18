# Teshtink

## Creating new SqlConnection vs. reusing an already opened one

Should we believe MS and just always create a new connection when one is needed?

Yes we should. The difference is 10-20us and if you're going over network, your
performance is fucked anyways.

``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 8.1 (6.3.9600)
Processor=Intel Core i7-6700K CPU 4.00GHz (Skylake), ProcessorCount=8
Frequency=3914067 Hz, Resolution=255.4887 ns, Timer=TSC
  [Host]     : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2114.0
  DefaultJob : .NET Framework 4.7 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2114.0


```
 |              Method |     Mean |    Error |    StdDev | Scaled | ScaledSD |
 |-------------------- |---------:|---------:|----------:|-------:|---------:|
 | OneConnectionOpened | 68.72 us | 1.115 us | 1.0433 us |   1.00 |     0.00 |
 | AlwaysNewConnection | 81.58 us | 1.052 us | 0.9323 us |   1.19 |     0.02 |


## C# foo == null vs. foo.IsNull<TFoo>()

I got called out on [SO](https://stackoverflow.com/) while spreading FUD on a extension that 
(only!) tests for `this` parameter being null becoming an expensive operation in a loop. A user 
reminded me that JIT doesn't necessarily care about DLL boundary.

The result is that yes, **modern C# JIT can inline** across DLL boundaries.

Relevant diagnostic result from [BenchmarkDotNet](https://github.com/PerfDotNet/BenchmarkDotNet) InliningDiagnoser:

```
--------------------
NullTestBenchmarks_IsNull_Local_True_Mode-SingleRun_Platform-Host_Jit-Host_Runtime-Host_GcMode-Host_WarmupCount-1_TargetCount-1_LaunchCount-1_IterationTime-Auto_Affinity-Auto
 ...
Inliner: nullextension_benchmark.NullTestBenchmarks.IsNull_Local_True - instance bool  ()
Inlinee: nullextension_benchmark.NullTestExtensionLocal.IsNullLocal - generic bool  (!!0)
--------------------

--------------------
NullTestBenchmarks_IsNull_DLL_Mode-SingleRun_Platform-Host_Jit-Host_Runtime-Host_GcMode-Host_WarmupCount-1_TargetCount-1_LaunchCount-1_IterationTime-Auto_Affinity-Auto
 ...
Inliner: nullextension_benchmark.NullTestBenchmarks.IsNull_DLL - instance bool  ()
Inlinee: addonlib.NullTestExtension.IsNullFromOtherDLL - generic bool  (!!0)
--------------------
```

Benchmarking code in /nullextension-benchmark/, you'll need to look at diagnostic output to find above.
Actual speed of functions is not measurable because instruction count is so low compared to other setups.
