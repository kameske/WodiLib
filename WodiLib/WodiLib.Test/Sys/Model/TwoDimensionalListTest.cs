using System;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using TestTools = WodiLib.Test.Sys.TwoDimensionalListTest_Tools;
using TestRecord = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecord;
using TestRecordList = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecordList;
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

            TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> result = null;

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

        #endregion
    }
}
