
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<MyBenchmark>(new ManualConfig()
    .AddJob(Job.ShortRun)
    .AddColumnProvider(DefaultColumnProviders.Instance)
    .AddDiagnoser(MemoryDiagnoser.Default)
    .AddExporter(MarkdownExporter.GitHub)
    .AddExporter(MarkdownExporter.Console)
    .AddLogger(ConsoleLogger.Unicode));

public class MyBenchmark
{
    private readonly int[] _array = Enumerable.Range(1, 100).ToArray();
    private readonly List<int> _list = Enumerable.Range(1, 100).ToList();
    private readonly ImmutableList<int> _immutableList = Enumerable.Range(1, 100).ToImmutableList();
    private readonly ImmutableArray<int> _immutableArray = Enumerable.Range(1, 100).ToImmutableArray();
    private readonly ReadOnlyCollection<int> _readOnlyCollection = Enumerable.Range(1, 100).ToList().AsReadOnly();
    private readonly ConcurrentBag<int> _concurrentBag = new ConcurrentBag<int>(Enumerable.Range(1, 100));
    private readonly IEnumerable<int> _enumerable = new MyEnumerable();

    [Benchmark]
    public int LinqFirstOfArray() => _array.First();

    [Benchmark]
    public int LinqFirstOfList() => _list.First();

    [Benchmark]
    public int LinqFirstOfImmutableList() => _immutableList.First();

    [Benchmark]
    public int LinqFirstOfImmutableArray() => _immutableArray.First();

    [Benchmark]
    public int LinqFirstOfReadOnlyCollection() => _readOnlyCollection.First();

    [Benchmark]
    public int LinqFirstOfConcurrentBag() => _concurrentBag.First();

    [Benchmark]
    public int LinqFirstOfEnumerable() => _enumerable.First();

    [Benchmark]
    public int IndexAccessArray() => _array[0];

    [Benchmark]
    public int IndexAccessOfList() => _list[0];

    [Benchmark]
    public int IndexAccessOfImmutableList() => _immutableList[0];

    [Benchmark]
    public int IndexAccessOfImmutableArray() => _immutableArray[0];

    [Benchmark]
    public int IndexAccessOfReadOnlyCollection() => _readOnlyCollection[0];

    [Benchmark]
    public int MyFirstOfArray() => _array.MyFirst();

    [Benchmark]
    public int MyFirstOfList() => _list.MyFirst();

    [Benchmark]
    public int MyFirstOfImmutableList() => _immutableList.MyFirst();

    [Benchmark]
    public int MyFirstOfImmutableArray() => _immutableArray.MyFirst();

    [Benchmark]
    public int MyFirstOfReadOnlyCollection() => _readOnlyCollection.MyFirst();

    [Benchmark]
    public int MyFirstOfConcurrentBag() => _concurrentBag.MyFirst();

    [Benchmark]
    public int MyFirstOfEnumerable() => _enumerable.MyFirst();

    // 1 から順に整数を列挙する Enumerable
    private sealed class MyEnumerable : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            return new MyEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class MyEnumerator : IEnumerator<int>
        {
            public bool MoveNext()
            {
                Current++;
                return true;
            }

            public void Reset()
            {
                Current = 0;
            }

            public int Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}

public static partial class MyEnumerable
{
    public static TSource MyFirst<TSource>(this IEnumerable<TSource> source)
    {
        return source.TryGetFirst(out var first)
            ? first!
            : throw new InvalidOperationException("シーケンスが空です"); // 要素がなければ例外
    }

    public static TSource MyFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return source.TryGetFirst(predicate, out var first)
            ? first!
            : throw new InvalidOperationException("シーケンスが空です"); // 要素がなければ例外
    }

    public static TSource? MyFirstOrDefault<TSource>(this IEnumerable<TSource> source) =>
        source.TryGetFirst(out var first) ? first : default;

    // デフォルト値を指定できるメソッド
    public static TSource? MyFirstOrDefault<TSource>(this IEnumerable<TSource> source, TSource? defaultValue) =>
        source.TryGetFirst(out var first) ? first : defaultValue;

    public static TSource? MyFirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
        source.TryGetFirst(predicate, out var first) ? first : default;

    public static TSource? MyFirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource? defaultValue) =>
        source.TryGetFirst(predicate, out var first) ? first : defaultValue;

    // 最初の要素の取得を試み、取得できれば out 引数で結果を返します。
    private static bool TryGetFirst<TSource>(this IEnumerable<TSource> source, out TSource? first)
    {
        ArgumentNullException.ThrowIfNull(source);

        using var enumerator = source.GetEnumerator();
        // 要素が見つかれば返す
        if (enumerator.MoveNext())
        {
            first = enumerator.Current;
            return true;
        }

        // 要素がなければ false
        first = default;
        return false;
    }

    // 条件を満たす最初の要素の取得を試み、取得できれば out 引数で結果を返します。
    private static bool TryGetFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource? first)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        // 条件を満たす要素が見つかれば返す
        foreach (var element in source)
        {
            if (predicate(element))
            {
                first = element;
                return true;
            }
        }

        // 条件を満たす要素がなければ false
        first = default;
        return false;
    }
}

