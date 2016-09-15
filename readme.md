# Teshtink

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
