using System;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class TwoDimensionalArrayExtensionTest
    {
        private static Logger logger = default!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ToTransposedArrayTestCaseSource =
        {
            new object[] {(int[][]) Array.Empty<int[]>(),},
            new object[] {(int[][]) new[] {Array.Empty<int>()},},
            new object[] {(int[][]) new[] {Array.Empty<int>(), Array.Empty<int>()},},
            new object[] {new[] {new[] {1, 2, 3}},},
            new object[] {new[] {new[] {1}, new[] {2}},},
            new object[] {new[] {new[] {1, 2, 3, 4}, new[] {11, 12, 13, 14}},},
            new object[] {new[] {new[] {1, 2}, new[] {11, 12}, new[] {21, 22}, new[] {31, 32}}},
        };

        [TestCaseSource(nameof(ToTransposedArrayTestCaseSource))]
        public static void ToTransposedArrayTest(int[][] target)
        {
            // 【事前条件】すべての行について列数が一致すること
            if (target.Length > 1)
            {
                Enumerable.Range(1, target.Length - 1)
                    .ForEach(row => Assert.AreEqual(target[0].Length, target[row].Length));
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            var getRowCount = (Func<int[][], int>) (items => items.Length);
            var getColumnCount = (Func<int[][], int>) (items =>
            {
                if (getRowCount(items) == 0) return 0;
                return items[0].Length;
            });

            var targetClone = target.Select(x => x.ToArray()).ToArray();

            var result = target.ToTransposedArray();

            logger.Debug($"target: {target}\nresult: {result}");

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            var targetRowCount = getRowCount(target);
            var targetColumnCount = getColumnCount(target);

            // もとの二次元配列の要素が変化していないこと
            Assert.AreEqual(targetRowCount, getRowCount(targetClone));
            if (targetRowCount > 0)
            {
                Enumerable.Range(0, targetRowCount).ForEach(row =>
                {
                    Assert.IsTrue(target[row].SequenceEqual(targetClone[row]));
                });
            }

            // 転置行列の行数および列数が正しいこと
            var resultRowCount = getRowCount(result);
            var resultColumnCount = getColumnCount(result);
            if (targetRowCount == 0 || targetColumnCount == 0)
            {
                /*
                 * もとの配列の 行数 == 0 かつ 列数 == 0 の場合空配列が返却される
                 */
                Assert.AreEqual(0, resultRowCount);
                Assert.AreEqual(0, getColumnCount(result));
            }
            else
            {
                Assert.AreEqual(targetColumnCount, resultRowCount);
                Assert.AreEqual(targetRowCount, getColumnCount(result));
            }

            // 転置行列のすべての行について列数が一致すること
            if (resultRowCount > 1)
            {
                Enumerable.Range(1, resultRowCount - 1).ForEach(row =>
                    Assert.AreEqual(resultColumnCount, result[row].Length));
            }

            // 各要素が正しく編集されていること
            Enumerable.Range(0, resultRowCount).ForEach(resultRow =>
            {
                Enumerable.Range(0, resultColumnCount).ForEach(resultColumn =>
                    Assert.AreEqual(target[resultColumn][resultRow], result[resultRow][resultColumn]));
            });
        }
    }
}
