#nullable disable

using System.Collections;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Parsers;

BenchmarkRunner.Run<MyBenchmark>(new ManualConfig()
    .AddJob(Job.ShortRun.WithWarmupCount(3).WithIterationCount(3))
    .AddColumnProvider(DefaultColumnProviders.Instance)
    .AddDiagnoser(MemoryDiagnoser.Default)
    .AddExporter(MarkdownExporter.GitHub)
    .AddLogger(ConsoleLogger.Unicode));

public class MyBenchmark
{
    private int[] _array;
    private IList<int> _arrayAsIList;
    private IEnumerable<int> _arrayAsIEnumerable;
    private List<int> _list;
    private IList<int> _listAsIList;
    private IEnumerable<int> _listAsIEnumerable;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _array = Enumerable.Range(1, 100).ToArray();
        _arrayAsIList = _array;
        _arrayAsIEnumerable = _array;
        _list = _array.ToList();
        _listAsIList = _list;
        _listAsIEnumerable = _list;
    }

    [Benchmark]
    public IList<int> ArrayAsIList() => _arrayAsIEnumerable as IList<int>;

    [Benchmark]
    public bool IsArrayIList() => _arrayAsIEnumerable is IList<int>;

    [Benchmark]
    public bool IsArrayIPartition() => _arrayAsIEnumerable is IPartitionDummy<int>;

    [Benchmark]
    public IEnumerable<int> ArrayAsIEnumerable() => _array;

    [Benchmark]
    public IList<int> ListAsIList() => _listAsIEnumerable as IList<int>;

    [Benchmark]
    public bool IsListIList() => _listAsIEnumerable is IList<int>;

    [Benchmark]
    public bool IsListIPartition() => _listAsIEnumerable is IPartitionDummy<int>;

    [Benchmark]
    public IEnumerable<int> ListAsIEnumerable() => _list;

    [Benchmark]
    public int FirstElementOfArray() => _array[0];

    [Benchmark]
    public int FirstElementOfArrayAsIList() => _arrayAsIList[0];

    [Benchmark]
    public int FirstElementOfArrayUsingLinq() => _array.First();

    [Benchmark]
    public int FirstElementOfList() => _list[0];

    [Benchmark]
    public int FirstElementOfListAsIList() => _listAsIList[0];

    [Benchmark]
    public int FirstElementOfListUsingLinq() => _list.First();
}

public interface IPartitionDummy<T>
{
    bool Dummy();
}

public class PartitionDummy<T> : IPartitionDummy<T>, IEnumerable<T>
{
    public bool Dummy()
    {
        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Enumerable.Empty<T>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
