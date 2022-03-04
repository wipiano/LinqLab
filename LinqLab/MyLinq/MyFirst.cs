using System;
using System.Collections.Generic;
using Xunit;

namespace LinqLab.MyLinq;

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

    // 最初の要素の取得を試み、取得できれば out 引数で結果を返します。
    private static bool TryGetFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource? first)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        using var enumerator = source.GetEnumerator();
        // 条件を満たす要素が見つかれば返す
        while (enumerator.MoveNext())
        {
            if (predicate(enumerator.Current))
            {
                first = enumerator.Current;
                return true;
            }
        }

        // 条件を満たす要素がなければ false
        first = default;
        return false;
    }
}

public class MyFirstTest
{
    [Fact]
    public void SimpleMyFirstTest()
    {
        string[] array = new[] { "first", "second", "third" };

        // 最初の要素 "first" が返る
        var first = array.MyFirst();
        Assert.Equal("first", first);

        // 要素が一つもない場合は InvalidOperationException が発生する
        string[] emptyArray = Array.Empty<string>();

        Assert.Throws<InvalidOperationException>(() =>
        {
            _ = emptyArray.MyFirst();
        });
    }

    [Fact]
    public void MyFirstWithPredicateTest()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };

        // 最初の偶数 (2) を取得
        var firstEven = array.MyFirst(x => x % 2 == 0);
        Assert.Equal(2, firstEven);

        // 条件に一致する要素がなければ InvalidOperationException が発生する
        Assert.Throws<InvalidOperationException>(() =>
        {
            // 10 より大きい要素は存在しない
            _ = array.MyFirst(x => x > 10);
        });
    }

    [Fact]
    public void SimpleMyFirstOrDefaultTest()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };

        // 最初の要素 (1) が返る
        var first = array.MyFirstOrDefault();
        Assert.Equal(1, first);

        int[] emptyArray = Array.Empty<int>();

        // 空の配列に対してはデフォルト値 (0) が返る
        var firstOfEmptyArray = emptyArray.MyFirstOrDefault();
        Assert.Equal(0, firstOfEmptyArray);

        // デフォルト値は指定することもできる
        var firstOfEmptyArray2 = emptyArray.MyFirstOrDefault(100);
        Assert.Equal(100, firstOfEmptyArray2);
    }

    [Fact]
    public void MyFirstOrDefaultWithPredicateTest()
    {
        int[] array = new[] { 1, 2, 3, 4, 5 };

        // 最初の偶数 (2) を取得
        var firstEven = array.MyFirstOrDefault(x => x % 2 == 0);
        Assert.Equal(2, firstEven);

        // 条件に一致する要素がなければデフォルト値を返す
        // 10 より大きい要素は存在しないので 0 が返る
        var greaterThan10 = array.MyFirstOrDefault(x => x > 10);
        Assert.Equal(0, greaterThan10);

        // デフォルト値を指定することもできる
        var greaterThan10Or999 = array.MyFirstOrDefault(x => x > 10, 999);
        Assert.Equal(999, greaterThan10Or999);
    }
}
