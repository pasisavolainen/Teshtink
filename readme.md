# Teshtink

## Speed of float / double /decimal

Saw a [question asked on reddit](https://www.reddit.com/r/csharp/comments/9ksvzo/other_than_the_limited_range_of_values_are_there/)
and decided to test actual hardware speed of float/double/decimal

Using an approximation of [Euler constant](https://en.wikipedia.org/wiki/E_\(mathematical_constant\)),
specifically `Sum(0 .. steps, 1/step!)` where steps=26 is for all variations of float/double/decimal

Tested function is a variation of 
``` csharp
float ApproximateEulerFloat(int n)
{
    float kFactorial = 1;

    if (n <= 1)
        return 2;
    int i = 1;
    float eSum = 1.0F + 1.0F / kFactorial;

    while(n-- > 0)
    {        
        kFactorial *= ++i;
        eSum += 1.0F / kFactorial;
    }

    return eSum;
}
```

The used `step=26` is limitation of `decimal` because the decimal variation of `ApproximateEulerDecimal` runs out of precision and gets overflowexceptions at that stage.

The end results are: 
``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 8.1 (6.3.9600.0)
Intel Core i7-6700K CPU 4.00GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=3914064 Hz, Resolution=255.4889 ns, Timer=TSC
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3163.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3163.0


```
|       Method |        Mean |     Error |    StdDev | Scaled | ScaledSD |
|------------- |------------:|----------:|----------:|-------:|---------:|
|   NeperFloat |    23.96 ns | 0.1587 ns | 0.1239 ns |   1.00 |     0.00 |
|  NeperDouble |    23.65 ns | 0.0605 ns | 0.0566 ns |   0.99 |     0.01 |
| NeperDecimal | 3,324.17 ns | 3.5696 ns | 3.3390 ns | 138.76 |     0.70 |



So the Decimal variation is slower by a **huge** margin. double/float give pretty much the same performance.

---

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

---

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
