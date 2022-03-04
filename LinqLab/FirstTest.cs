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

            // �ŏ��̗v�f "first" ���Ԃ�
            var first = array.First();
            Assert.Equal("first", first);

            // �v�f������Ȃ��ꍇ�� InvalidOperationException ����������
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

            // �ŏ��̋��� (2) ���擾
            var firstEven = array.First(x => x % 2 == 0);
            Assert.Equal(2, firstEven);

            // �����Ɉ�v����v�f���Ȃ���� InvalidOperationException ����������
            Assert.Throws<InvalidOperationException>(() =>
            {
                // 10 ���傫���v�f�͑��݂��Ȃ�
                _ = array.First(x => x > 10);
            });
        }

        [Fact]
        public void SimpleFirstOrDefaultTest()
        {
            int[] array = new[] { 1, 2, 3, 4, 5 };

            // �ŏ��̗v�f (1) ���Ԃ�
            var first = array.FirstOrDefault();
            Assert.Equal(1, first);

            int[] emptyArray = Array.Empty<int>();

            // ��̔z��ɑ΂��Ă̓f�t�H���g�l (0) ���Ԃ�
            var firstOfEmptyArray = emptyArray.FirstOrDefault();
            Assert.Equal(0, firstOfEmptyArray);

            // �f�t�H���g�l�͎w�肷�邱�Ƃ��ł���
            var firstOfEmptyArray2 = emptyArray.FirstOrDefault(100);
            Assert.Equal(100, firstOfEmptyArray2);
        }

        [Fact]
        public void FirstOrDefaultWithPredicateTest()
        {
            int[] array = new[] { 1, 2, 3, 4, 5 };

            // �ŏ��̋��� (2) ���擾
            var firstEven = array.FirstOrDefault(x => x % 2 == 0);
            Assert.Equal(2, firstEven);

            // �����Ɉ�v����v�f���Ȃ���΃f�t�H���g�l��Ԃ�
            // 10 ���傫���v�f�͑��݂��Ȃ��̂� 0 ���Ԃ�
            var greaterThan10 = array.FirstOrDefault(x => x > 10);
            Assert.Equal(0, greaterThan10);

            // �f�t�H���g�l���w�肷�邱�Ƃ��ł���
            var greaterThan10Or999 = array.FirstOrDefault(x => x > 10, 999);
            Assert.Equal(999, greaterThan10Or999);
        }
    }
}