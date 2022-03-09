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


public class MyBenchmark2
{
    private int[] _array;
    private IList<int> _arrayAsIList;
    private IEnumerable<int> _arrayAsIEnumerable;
    private List<int> _list;
    private IEnumerable<int> _listAsIEnumerable;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _array = Enumerable.Range(1, 100).ToArray();
        _arrayAsIList = _array;
        _arrayAsIEnumerable = _array;
        _list = _array.ToList();
        _listAsIEnumerable = _list;
    }

    [Benchmark]
    public bool IsArrayIList() => _arrayAsIEnumerable is IList<int>;

    [Benchmark]
    public int GetArrayCount() => _arrayAsIList.Count;

    [Benchmark]
    public int GetArrayLength() => _array.Length;

    [Benchmark]
    public bool IsListIList() => _listAsIEnumerable is IList<int>;

    [Benchmark]
    public int GetListCount() => _list.Count;
}
