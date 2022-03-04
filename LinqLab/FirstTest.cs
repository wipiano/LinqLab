using System;
using System.Linq;
using Xunit;

namespace LinqLab
{
    public class FirstTest
    {
        [Fact]
        public void SimpleFirstTest()
        {
            string[] array = new[] { "first", "second", "third" };

            // 最初の要素 "first" が返る
            var first = array.First();
            Assert.Equal("first", first);

            // 要素が一つもない場合は InvalidOperationException が発生する
            string[] emptyArray = Array.Empty<string>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = emptyArray.First();
            });
        }

        [Fact]
        public void FirstWithPredicateTest()
        {
            int[] array = new[] { 1, 2, 3, 4, 5 };

            // 最初の偶数 (2) を取得
            var firstEven = array.First(x => x % 2 == 0);
            Assert.Equal(2, firstEven);

            // 条件に一致する要素がなければ InvalidOperationException が発生する
            Assert.Throws<InvalidOperationException>(() =>
            {
                // 10 より大きい要素は存在しない
                _ = array.First(x => x > 10);
            });
        }

        [Fact]
        public void SimpleFirstOrDefaultTest()
        {
            int[] array = new[] { 1, 2, 3, 4, 5 };

            // 最初の要素 (1) が返る
            var first = array.FirstOrDefault();
            Assert.Equal(1, first);

            int[] emptyArray = Array.Empty<int>();

            // 空の配列に対してはデフォルト値 (0) が返る
            var firstOfEmptyArray = emptyArray.FirstOrDefault();
            Assert.Equal(0, firstOfEmptyArray);

            // デフォルト値は指定することもできる
            var firstOfEmptyArray2 = emptyArray.FirstOrDefault(100);
            Assert.Equal(100, firstOfEmptyArray2);
        }

        [Fact]
        public void FirstOrDefaultWithPredicateTest()
        {
            int[] array = new[] { 1, 2, 3, 4, 5 };

            // 最初の偶数 (2) を取得
            var firstEven = array.FirstOrDefault(x => x % 2 == 0);
            Assert.Equal(2, firstEven);

            // 条件に一致する要素がなければデフォルト値を返す
            // 10 より大きい要素は存在しないので 0 が返る
            var greaterThan10 = array.FirstOrDefault(x => x > 10);
            Assert.Equal(0, greaterThan10);

            // デフォルト値を指定することもできる
            var greaterThan10Or999 = array.FirstOrDefault(x => x > 10, 999);
            Assert.Equal(999, greaterThan10Or999);
        }
    }
}