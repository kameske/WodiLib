using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using TestTools = WodiLib.Test.Sys.TwoDimensionalListTest_Tools;
using TestRecord = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecord;
using TestDoubleEnumerableInstanceType = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestDoubleEnumerableInstanceType;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class TwoDimensionalListTest
    {
        /*
         * TwoDimensionalList が実装するメソッドのうち
         * 「TwoDimensionalListValidator による引数検証が行われるメソッド」
         * 「Get 系メソッド」以外のメソッドが正しく実装されていることを検証する。
         * コンストラクタについても対象外とする。
         * 対象外のメソッドや処理前後の通知の正しさは検証しない。
         * (TwoDimensionalListOperationValidateTest, TwoDimensionalListOperationResultTest,
         * TwoDimensionalListNotifyTestで確認)
         *
         * 引数が不正な場合にエラーが発生すること、引数が正しい場合に意図した動作をすることを検証する。
         */

        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        #region Property

        #region Count

        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic))]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.EmptyRows))]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Empty))]
        public static void CountTest(string testType)
        {
            var initRowLength = TestTools.InitRowLengthFrom(testType, false);

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var result = -1;

            var instance = TestTools.MakeTwoDimensionalList(testType, funcMakeDefaultItem);

            try
            {
                result = instance.RowCount;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 取得結果が正しいこと
            Assert.AreEqual(initRowLength, result);
        }

        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic))]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.EmptyRows))]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Empty))]
        public static void ItemCountTest(string testType)
        {
            var initColumnLength = TestTools.InitColumnLengthFrom(testType, false);

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var result = -1;

            var instance = TestTools.MakeTwoDimensionalList(testType, funcMakeDefaultItem);

            try
            {
                result = instance.ColumnCount;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 取得結果が正しいこと
            Assert.AreEqual(initColumnLength, result);
        }

        #endregion

        #region IsEmpty

        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic))]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.EmptyRows))]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Empty))]
        public static void IsEmptyTest(string testType)
        {
            var isEmpty = TestTools.IsEmptyFrom(testType, false);

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var result = false;

            var instance = TestTools.MakeTwoDimensionalList(testType, funcMakeDefaultItem);

            try
            {
                result = instance.IsEmpty;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 取得結果が正しいこと
            Assert.AreEqual(isEmpty, result);
        }

        #endregion

        #endregion

        #region ItemEquals

        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Null), false)]
        public static void ItemEqualsTest_TwoDimensionalList(string testType, bool answer)
        {
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                funcMakeDefaultItem);

            var otherItem = TestTools.MakeTwoDimensionalList(testType, funcMakeDefaultItem);

            var result = false;

            try
            {
                result = instance.ItemEquals((ITwoDimensionalList<TestRecord, TestRecord>) otherItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 取得結果が正しいこと
            Assert.AreEqual(answer, result);
        }

        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.HasNullRow), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.HasNullColumn), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Null), false)]
        public static void ItemEqualsTest_IEnumerable(string testType, bool answer)
        {
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                funcMakeDefaultItem);

            var otherItem = TestTools.MakeTestRecordList(testType, false, funcMakeDefaultItem);

            var result = false;

            try
            {
                result = instance.ItemEquals(otherItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 取得結果が正しいこと
            Assert.AreEqual(answer, result);
        }

        #endregion

        #region ToTwoDimensionalArray

        [Test]
        public static void ToTwoDimensionalArrayTest()
        {
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                funcMakeDefaultItem);
            var originalCount = instance.RowCount;
            var originalItemCount = instance.ColumnCount;

            TestRecord[][] result = null;

            try
            {
                result = instance.ToTwoDimensionalArray();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 取得結果が正しいこと
            Assert.AreEqual(originalCount, result.Length);
            Assert.AreEqual(originalItemCount, result.GetInnerArrayLength());
            for (var r = 0; r < originalCount; r++)
            for (var c = 0; c < originalItemCount; c++)
            {
                Assert.IsTrue(ReferenceEquals(instance[r, c], result[r][c]));
            }
        }

        #endregion

        #region DeepClone

        [Test]
        public static void DeepCloneTest()
        {
            // 対象リストの内包型（ここでは TestRecord クラス）が IDeepCloneable<T> を実装している必要がある
            Assert.True(typeof(IDeepCloneable<TestRecord>).IsAssignableFrom(typeof(TestRecord)));

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                funcMakeDefaultItem);
            var originalCount = instance.RowCount;
            var originalItemCount = instance.ColumnCount;

            TwoDimensionalList<TestRecord> result = null;

            try
            {
                result = instance.DeepClone();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 取得結果が正しいこと
            Assert.AreEqual(originalCount, result.RowCount);
            Assert.AreEqual(originalItemCount, result.ColumnCount);
            Assert.False(ReferenceEquals(instance, result));
            for (var r = 0; r < originalCount; r++)
            for (var c = 0; c < originalItemCount; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(result[r, c]));
                Assert.False(ReferenceEquals(instance[r, c], result[r, c]));
            }
        }

        private static readonly object[] DeepCloneWithTestCaseSource =
        {
            new object[] {null, null, null},
            new object[] {5, null, null},
            new object[] {null, 2, null},
            new object[]
            {
                null, null, new[]
                {
                    (1, 1, "ClonedItem"),
                }
            },
            new object[] {5, 2, null},
            new object[]
            {
                5, null, new[]
                {
                    (1, 1, "ClonedItem"),
                }
            },
            new object[]
            {
                null, 2, new[]
                {
                    (1, 1, "ClonedItem"),
                }
            },
            new object[]
            {
                5, 2, new[]
                {
                    (1, 1, "ClonedItem"),
                    (5, 0, "ClonedItem2"),
                    (0, 2, "ClonedItem3"),
                }
            },
        };

        [TestCaseSource(nameof(DeepCloneWithTestCaseSource))]
        public static void DeepCloneWithTest(int? rowLength, int? colLength,
            (int row, int column, string prefix)[] valuesSources)
        {
            /*
             * TestRecord が internal static なクラス内で定義されているため、
             * テストメソッドでは object 型で受け取り、変換してから使用する。
             */
            Dictionary<(int row, int col), TestRecord> castedValues;
            if (valuesSources is null)
            {
                castedValues = null;
            }
            else
            {
                castedValues = new Dictionary<(int row, int col), TestRecord>();
                valuesSources.ForEach(src =>
                {
                    var (row, column, prefix) = src;
                    castedValues[(row, column)] = TestTools.MakeItem(row, column, prefix);
                });
            }

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                funcMakeDefaultItem);
            var originalCount = instance.RowCount;
            var originalItemCount = instance.ColumnCount;

            TwoDimensionalList<TestRecord> result = null;

            var assumedRowLength = rowLength ?? originalCount;
            var assumedColumnLength = colLength ?? originalItemCount;

            try
            {
                result = instance.DeepCloneWith(rowLength, colLength, castedValues);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 取得結果が正しいこと
            Assert.AreEqual(assumedRowLength, result.RowCount);
            Assert.AreEqual(assumedColumnLength, result.ColumnCount);
            Assert.False(ReferenceEquals(instance, result));
            for (var r = 0; r < Math.Min(rowLength ?? int.MaxValue, originalCount); r++)
            for (var c = 0; c < Math.Min(colLength ?? int.MaxValue, originalItemCount); c++)
            {
                if (castedValues?.ContainsKey((r, c)) ?? false)
                {
                    // 上書き指定した要素が編集されていること
                    var value = castedValues[(r, c)];
                    Assert.IsTrue(value.ItemEquals(result[r, c]));
                    Assert.True(ReferenceEquals(value, result[r, c]));
                }
                else
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(result[r, c]));
                    Assert.False(ReferenceEquals(instance[r, c], result[r, c]));
                }
            }
        }

        #endregion
    }
}
