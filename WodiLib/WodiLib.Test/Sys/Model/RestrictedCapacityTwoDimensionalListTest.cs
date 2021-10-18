using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class RestrictedCapacityTwoDimensionalListTest
    {
        /*
         * 制約のない二次元リストとしての動作確認は TwoDimensionalListTest の各種ユニットテストに一存する。
         *
         * ここでは容量制限が正しく作用していることを確認する。
         */
        private static TestClass.CapacityInfo _initCapacityInfo;

        /// <summary>
        ///     テスト時に使用する二次元リストの通常容量範囲
        /// </summary>
        private static TestClass.CapacityInfo InitCapacityInfo
        {
            get
            {
                return _initCapacityInfo ??= new TestClass.CapacityInfo(
                    InitMaxCapacity, InitMinCapacity,
                    InitMaxItemCapacity, InitMinItemCapacity);
            }
        }

        private static TestClass.CapacityInfo _initEmptyCapacityInfo;

        /// <summary>
        ///     テスト時に使用する空状態を許容する二次元リストの容量範囲
        /// </summary>
        /// <remarks>
        ///     <see cref="TestClass.CapacityInfo.MinCapacity"/> および
        ///     <see cref="TestClass.CapacityInfo.MinItemCapacity"/> は 0 を返す。
        /// </remarks>
        private static TestClass.CapacityInfo InitEmptyCapacityInfo
        {
            get
            {
                return _initEmptyCapacityInfo ??= new TestClass.CapacityInfo(
                    InitMaxCapacity, 0,
                    InitMaxItemCapacity, 0);
            }
        }

        private const int InitMaxCapacity = 10;
        private const int InitMinCapacity = 4;
        private const int InitMaxItemCapacity = 12;
        private const int InitMinItemCapacity = 2;

        private const int InitCapacity = 6;
        private const int InitItemCapacity = 4;


        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-1, -1, -1, -1, false, true, true)]
        [TestCase(-1, -1, -1, 10, true, true, true)]
        [TestCase(-1, -1, 0, -1, true, true, true)]
        [TestCase(-1, -1, 10, 10, false, true, true)]
        [TestCase(-1, 0, -1, 0, false, true, true)]
        [TestCase(-1, 0, 0, 10, false, true, true)]
        [TestCase(-1, 5, 10, -1, true, true, true)]
        [TestCase(-1, 5, 10, -1, false, true, true)]
        [TestCase(-1, 5, 10, 10, false, true, true)]
        [TestCase(0, -1, 0, 0, false, true, true)]
        [TestCase(0, -1, -1, -1, false, true, true)]
        [TestCase(0, -1, 10, 0, true, true, true)]
        [TestCase(0, 0, 0, 0, false, false, false)]
        [TestCase(0, 5, -1, 10, false, true, true)]
        [TestCase(0, 5, 10, 10, false, false, false)]
        [TestCase(5, -1, -1, 10, false, true, true)]
        [TestCase(5, -1, 10, 10, false, true, true)]
        [TestCase(5, 0, -1, -1, true, true, true)]
        [TestCase(5, 0, 0, -1, false, true, true)]
        [TestCase(5, 5, 0, 0, false, false, false)]
#if DEBUG
        [TestCase(0, 0, 10, 0, false, true, true)]
        [TestCase(6, 5, 10, 0, false, true, true)]
        [TestCase(5, 6, 11, 10, false, true, true)]
#elif RELEASE
        [TestCase(0, 0, 10, 0, false, false, true)]
        [TestCase(6, 5, 10, 0, false, false, true)]
        [TestCase(5, 6, 11, 10, false, false, true)]
#endif
        public static void ConstructorTest1(int minCapacity, int maxCapacity,
            int minItemCapacity, int maxItemCapacity,
            bool funcMakeItemIsNull,
            bool isError, bool isErrorState)
        {
            var errorOccured = false;

            Func<int, int, TestClass.TwoDimItem> makeDefaultValueItem;
            if (funcMakeItemIsNull)
            {
                makeDefaultValueItem = null;
            }
            else
            {
                makeDefaultValueItem = TestClass.MakeDefaultValueItem;
            }

            TestClass.TestTwoDimClass instance = null;

            try
            {
                instance = new TestClass.TestTwoDimClass(
                    new TestClass.CapacityInfo(maxCapacity, minCapacity, maxItemCapacity, minItemCapacity),
                    target =>
                        new RestrictedCapacityTwoDimensionalListValidator<TestClass.TwoDimItem, TestClass.TwoDimItem>(
                            target, "行名",
                            "列名"),
                    makeDefaultValueItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 初期行数・列数が最小行数・列数と一致すること（Releaseビルドかつ異常設定の場合この条件を満たさない可能性あり）
            if (!isErrorState)
            {
                Assert.AreEqual(instance.RowCount, instance.GetMinRowCapacity());

                // Count が 0 の場合は FieldCount は最大・最小設定によらず0
                Assert.AreEqual(instance.ColumnCount,
                    instance.RowCount != 0
                        ? instance.GetMinColumnCapacity()
                        : 0);

                // すべての要素が意図した値で初期化されていること
                instance.ForEach((row, rowIdx) =>
                {
                    row.ForEach((item, colIdx) =>
                        Assert.IsTrue(item.Equals(TestClass.MakeDefaultValueItem(rowIdx, colIdx))));
                });
            }
        }

        private static readonly object[] ConstructorTest2CaseSource =
        {
            new object[] { TestClass.ListType.Normal, InitMinCapacity - 1, InitMinItemCapacity, true },
            new object[] { TestClass.ListType.Normal, InitMinCapacity - 1, InitMaxItemCapacity, true },
            new object[] { TestClass.ListType.Normal, InitMinCapacity, InitMinItemCapacity - 1, true },
            new object[] { TestClass.ListType.Normal, InitMinCapacity, InitMinItemCapacity, false },
            new object[] { TestClass.ListType.Normal, InitMinCapacity, InitMaxItemCapacity, false },
            new object[] { TestClass.ListType.Normal, InitMinCapacity, InitMaxItemCapacity + 1, true },
            new object[] { TestClass.ListType.Normal, InitMaxCapacity, InitMinItemCapacity - 1, true },
            new object[] { TestClass.ListType.Normal, InitMaxCapacity, InitMinItemCapacity, false },
            new object[] { TestClass.ListType.Normal, InitMaxCapacity, InitMaxItemCapacity, false },
            new object[] { TestClass.ListType.Normal, InitMaxCapacity, InitMaxItemCapacity + 1, true },
            new object[] { TestClass.ListType.Normal, InitMaxCapacity + 1, InitMaxItemCapacity, true },
            new object[] { TestClass.ListType.Normal, InitMaxCapacity + 1, InitMinItemCapacity, true },
            new object[] { TestClass.ListType.SelfNull, InitMinCapacity, InitMinItemCapacity, true },
            new object[] { TestClass.ListType.RowHasNull, InitMinCapacity, InitMaxItemCapacity, true },
            new object[] { TestClass.ListType.ColumnHasNull, InitMaxCapacity, InitMinItemCapacity, true },
            new object[] { TestClass.ListType.ColumnSizeDifference, InitMaxCapacity, InitMaxItemCapacity, true },
        };

        [TestCaseSource(nameof(ConstructorTest2CaseSource))]
        public static void ConstructorTest2(
            TestClass.ListType initItemType, int initRowLength, int initColumnLength,
            bool isError)
        {
            var errorOccured = false;
            var initList = initItemType.GetMultiLine(initRowLength, initColumnLength);
            TestClass.TestTwoDimClass instance = null;
            Func<int, int, TestClass.TwoDimItem> funcMakeDefaultValueItem = TestClass.MakeDefaultValueItem;

            try
            {
                instance = new TestClass.TestTwoDimClass(InitCapacityInfo,
                    initList,
                    self =>
                        new RestrictedCapacityTwoDimensionalListValidator<TestClass.TwoDimItem, TestClass.TwoDimItem>(
                            self, "行名", "列名"),
                    funcMakeDefaultValueItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 行数・列数がセットした行数・列数と一致すること
            Assert.AreEqual(instance.RowCount, initRowLength);
            Assert.AreEqual(instance.ColumnCount, initColumnLength);
        }

        [Test]
        public static void ConstructorTest3()
        {
            var errorOccured = false;
            TestClass.TestTwoDimClass instance = null;
            Func<int, int, TestClass.TwoDimItem> funcMakeDefaultValueItem = TestClass.MakeDefaultValueItem;

            try
            {
                instance = new TestClass.TestTwoDimClass(InitCapacityInfo,
                    self =>
                        new RestrictedCapacityTwoDimensionalListValidator<TestClass.TwoDimItem, TestClass.TwoDimItem>(
                            self, "行名", "列名"),
                    funcMakeDefaultValueItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 容量情報が引数で与えた情報と一致すること
            Assert.AreEqual(InitMaxCapacity, instance.GetMaxRowCapacity());
            Assert.AreEqual(InitMinCapacity, instance.GetMinRowCapacity());
            Assert.AreEqual(InitMaxItemCapacity, instance.GetMaxColumnCapacity());
            Assert.AreEqual(InitMinItemCapacity, instance.GetMinColumnCapacity());

            // 行数・列数が最小容量と一致すること
            Assert.AreEqual(instance.RowCount, InitMinCapacity);
            Assert.AreEqual(instance.ColumnCount, InitMinItemCapacity);
        }

        /* Count, ColumnCount プロパティのテストは
         * ConstructorTest1, ConstructorTest2 参照
         */

        [TestCase(0, 0, true)]
        [TestCase(InitMaxCapacity, 0, false)]
        [TestCase(InitMaxCapacity, InitMaxItemCapacity, false)]
        // 行数 == 0 かつ 列数 != 0 状態は存在し得ない
        public static void IsEmptyTest(int initRowLength, int initColumnLength, bool answer)
        {
            var instance = TestClass.MakeTestInstance(
                InitEmptyCapacityInfo,
                initRowLength, initColumnLength);

            // Count, ColumnCount が一致すること
            Assert.AreEqual(instance.RowCount, initRowLength);
            Assert.AreEqual(instance.ColumnCount, initColumnLength);

            // IsEmpty プロパティが意図した値であること
            Assert.AreEqual(instance.IsEmpty, answer);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, InitItemCapacity - 1, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, InitItemCapacity - 1, false)]
        [TestCase(0, InitItemCapacity, true)]
        [TestCase(InitCapacity - 1, -1, true)]
        [TestCase(InitCapacity - 1, 0, false)]
        [TestCase(InitCapacity - 1, InitItemCapacity - 1, false)]
        [TestCase(InitCapacity - 1, InitItemCapacity, true)]
        [TestCase(InitCapacity, 0, true)]
        [TestCase(InitCapacity, InitItemCapacity - 1, true)]
        public static void IndexerGetTest(int row, int column, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo,
                InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                _ = instance[row, column];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, 0, "abc", true)]
        [TestCase(-1, InitItemCapacity - 1, "abc", true)]
        [TestCase(0, -1, "abc", true)]
        [TestCase(0, 0, "abc", false)]
        [TestCase(0, 0, null, true)]
        [TestCase(0, InitItemCapacity - 1, "abc", false)]
        [TestCase(0, InitItemCapacity, "abc", true)]
        [TestCase(InitCapacity - 1, -1, "abc", true)]
        [TestCase(InitCapacity - 1, 0, "abc", false)]
        [TestCase(InitCapacity - 1, InitItemCapacity, "abc", true)]
        [TestCase(InitCapacity - 1, InitItemCapacity - 1, null, true)]
        [TestCase(InitCapacity - 1, InitItemCapacity - 1, "abc", false)]
        [TestCase(InitCapacity, 0, "abc", true)]
        [TestCase(InitCapacity, InitItemCapacity - 1, "abc", true)]
        public static void IndexerSetTest(int row, int column, string setItem, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo,
                InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance[row, column] = setItem;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, -1, true)]
        [TestCase(-1, 1, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, InitCapacity, false)]
        [TestCase(0, InitCapacity + 1, true)]
        [TestCase(InitCapacity - 1, -1, true)]
        [TestCase(InitCapacity - 1, 0, false)]
        [TestCase(InitCapacity - 1, 1, false)]
        [TestCase(InitCapacity - 1, 2, true)]
        [TestCase(InitCapacity, -1, true)]
        [TestCase(InitCapacity, 1, true)]
        public static void GetRowTest(int row, int count, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo,
                InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                _ = instance.GetRow(row, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, -1, true)]
        [TestCase(-1, 1, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, InitItemCapacity, false)]
        [TestCase(0, InitItemCapacity + 1, true)]
        [TestCase(InitItemCapacity - 1, -1, true)]
        [TestCase(InitItemCapacity - 1, 0, false)]
        [TestCase(InitItemCapacity - 1, 1, false)]
        [TestCase(InitItemCapacity - 1, 2, true)]
        [TestCase(InitItemCapacity, -1, true)]
        [TestCase(InitItemCapacity, 1, true)]
        public static void GetColumnTest(int column, int count, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo,
                InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                _ = instance.GetColumn(column, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] AddRowTestCaseSource =
        {
            new object[] { InitCapacity, InitItemCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { InitCapacity, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { InitCapacity, InitItemCapacity + 1, TestClass.ListType.Normal, true },
            new object[] { InitCapacity, InitItemCapacity, TestClass.ListType.SelfNull, true },
            new object[] { InitCapacity, InitItemCapacity, TestClass.ListType.ColumnHasNull, true },
            new object[] { InitMaxCapacity - 1, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { InitMaxCapacity, InitItemCapacity, TestClass.ListType.Normal, true },
        };

        [TestCaseSource(nameof(AddRowTestCaseSource))]
        public static void AddRowTest(int initLength, int addLineItemLength, TestClass.ListType addType, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, initLength, InitItemCapacity);
            var addItem = addType.GetLine(addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.AddRow(addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }


        private static readonly object[] AddColumnTestCaseSource =
        {
            new object[] { InitItemCapacity, InitCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { InitItemCapacity, InitCapacity, TestClass.ListType.Normal, false },
            new object[] { InitItemCapacity, InitCapacity + 1, TestClass.ListType.Normal, true },
            new object[] { InitItemCapacity, InitCapacity, TestClass.ListType.SelfNull, true },
            new object[] { InitItemCapacity, InitCapacity, TestClass.ListType.ColumnHasNull, true },
            new object[] { InitMaxItemCapacity - 1, InitCapacity, TestClass.ListType.Normal, false },
            new object[] { InitMaxItemCapacity, InitCapacity, TestClass.ListType.Normal, true },
        };

        [TestCaseSource(nameof(AddColumnTestCaseSource))]
        public static void AddColumnTest(int initColumnLength, int addLineItemLength, TestClass.ListType addType,
            bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, initColumnLength);
            var addItem = addType.GetLine(addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.AddColumn(addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] AddRowRangeTestCaseSource =
        {
            new object[] { InitCapacity, 0, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity - 1, TestClass.ListType.Normal, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity + 1, TestClass.ListType.Normal, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity + 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.SelfNull, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.RowHasNull, true },
            new object[]
            {
                InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.ColumnHasNull, true
            },
            new object[]
            {
                InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.ColumnSizeDifference,
                true
            },
            new object[] { InitMaxCapacity, 0, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { InitMaxCapacity, 1, InitItemCapacity, TestClass.ListType.Normal, true },
        };

        [TestCaseSource(nameof(AddRowRangeTestCaseSource))]
        public static void AddRowRangeTest(int initRowLength, int addRowLength, int addLineItemLength,
            TestClass.ListType addType, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, initRowLength, InitItemCapacity);
            var addItem = addType.GetMultiLine(addRowLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.AddRow(ConvertIEnumerableArray(addItem));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }


        private static readonly object[] AddColumnRangeTestCaseSource =
        {
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity - 1,
                TestClass.ListType.Normal, true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity, TestClass.ListType.Normal,
                false
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity + 1,
                TestClass.ListType.Normal, true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity + 1, InitCapacity,
                TestClass.ListType.Normal, true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity, TestClass.ListType.SelfNull,
                true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity,
                TestClass.ListType.RowHasNull, true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity,
                TestClass.ListType.ColumnHasNull, true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity,
                TestClass.ListType.ColumnSizeDifference, true
            },
            new object[] { InitMaxItemCapacity, 0, 0, TestClass.ListType.Normal, false },
            new object[] { InitMaxItemCapacity, 1, InitCapacity, TestClass.ListType.Normal, true },
        };

        [TestCaseSource(nameof(AddColumnRangeTestCaseSource))]
        public static void AddColumnRangeTest(int initColumnLength, int addColumnLength, int addLineItemLength,
            TestClass.ListType addType,
            bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, initColumnLength);
            var addItem = addType.GetMultiLine(addColumnLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.AddColumn(ConvertIEnumerableArray(addItem));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] InsertRowTestCaseSource =
        {
            new object[] { InitCapacity, -1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { InitCapacity, 0, InitItemCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { InitCapacity, 0, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { InitCapacity, 0, InitItemCapacity + 1, TestClass.ListType.Normal, true },
            new object[] { InitCapacity, 0, InitItemCapacity, TestClass.ListType.SelfNull, true },
            new object[] { InitCapacity, 0, InitItemCapacity, TestClass.ListType.ColumnHasNull, true },
            new object[] { InitCapacity, InitCapacity, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { InitCapacity, InitCapacity + 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { InitMaxCapacity - 1, 0, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { InitMaxCapacity, 0, InitItemCapacity, TestClass.ListType.Normal, true },
        };

        [TestCaseSource(nameof(InsertRowTestCaseSource))]
        public static void InsertRowTest(int initLength, int index, int addLineItemLength, TestClass.ListType addType,
            bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, initLength, InitItemCapacity);
            var addItem = addType.GetLine(addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.InsertRow(index, addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] InsertColumnTestCaseSource =
        {
            new object[] { InitItemCapacity, -1, InitCapacity, TestClass.ListType.Normal, true },
            new object[] { InitItemCapacity, 0, InitCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { InitItemCapacity, 0, InitCapacity, TestClass.ListType.Normal, false },
            new object[] { InitItemCapacity, 0, InitCapacity + 1, TestClass.ListType.Normal, true },
            new object[] { InitItemCapacity, 0, InitCapacity, TestClass.ListType.SelfNull, true },
            new object[] { InitItemCapacity, 0, InitCapacity, TestClass.ListType.ColumnHasNull, true },
            new object[] { InitItemCapacity, InitItemCapacity, InitCapacity, TestClass.ListType.Normal, false },
            new object[] { InitItemCapacity, InitItemCapacity + 1, InitCapacity, TestClass.ListType.Normal, true },
            new object[] { InitMaxItemCapacity - 1, 0, InitCapacity, TestClass.ListType.Normal, false },
            new object[] { InitMaxItemCapacity, 0, InitCapacity, TestClass.ListType.Normal, true },
        };

        [TestCaseSource(nameof(InsertColumnTestCaseSource))]
        public static void InsertColumnTest(int initItemLength, int insertColumn, int addLineItemLength,
            TestClass.ListType addType, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, initItemLength);
            var addItem = addType.GetLine(addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.InsertColumn(insertColumn, addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] InsertRowRangeTestCaseSource =
        {
            new object[] { -1, 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { 0, 0, 0, TestClass.ListType.Normal, false },
            new object[] { 0, InitMaxCapacity - InitCapacity, InitItemCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { 0, InitMaxCapacity - InitCapacity + 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { InitCapacity, 0, 0, TestClass.ListType.Normal, false },
            new object[] { InitCapacity, 1, InitItemCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { InitCapacity, 1, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { InitCapacity, 1, InitItemCapacity + 1, TestClass.ListType.Normal, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity + 1, TestClass.ListType.Normal, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity + 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { InitCapacity + 1, 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.SelfNull, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.RowHasNull, true },
            new object[]
            {
                InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.ColumnHasNull, true
            },
            new object[]
            {
                InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.ColumnSizeDifference,
                true
            },
        };

        [TestCaseSource(nameof(InsertRowRangeTestCaseSource))]
        public static void InsertRowRangeTest(int index, int addRowLength, int addLineItemLength,
            TestClass.ListType addType, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var addItem = addType.GetMultiLine(addRowLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.InsertRow(index, ConvertIEnumerableArray(addItem));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] InsertColumnRangeTestCaseSource =
        {
            new object[] { -1, 1, InitCapacity, TestClass.ListType.Normal, true },
            new object[] { 0, 0, 0, TestClass.ListType.Normal, false },
            new object[]
                { 0, InitMaxItemCapacity - InitItemCapacity, InitCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxItemCapacity - InitItemCapacity, InitCapacity, TestClass.ListType.Normal, false },
            new object[]
                { 0, InitMaxItemCapacity - InitItemCapacity + 1, InitCapacity, TestClass.ListType.Normal, true },
            new object[] { InitItemCapacity, 0, 0, TestClass.ListType.Normal, false },
            new object[] { InitItemCapacity, 1, InitCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { InitItemCapacity, 1, InitCapacity, TestClass.ListType.Normal, false },
            new object[] { InitItemCapacity, 1, InitCapacity + 1, TestClass.ListType.Normal, true },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity, TestClass.ListType.Normal, false
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity + 1, TestClass.ListType.Normal,
                true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity + 1, InitCapacity, TestClass.ListType.Normal,
                true
            },
            new object[] { InitItemCapacity + 1, 1, InitCapacity, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxItemCapacity - InitCapacity, InitCapacity, TestClass.ListType.SelfNull, true },
            new object[] { 0, InitMaxItemCapacity - InitCapacity, InitCapacity, TestClass.ListType.RowHasNull, true },
            new object[]
                { 0, InitMaxItemCapacity - InitCapacity, InitCapacity, TestClass.ListType.ColumnHasNull, true },
            new object[]
            {
                0, InitMaxItemCapacity - InitCapacity, InitCapacity, TestClass.ListType.ColumnSizeDifference, true
            },
        };

        [TestCaseSource(nameof(InsertColumnRangeTestCaseSource))]
        public static void InsertColumnRangeTest(int index, int addColumnLength, int addLineItemLength,
            TestClass.ListType addType, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var addItem = addType.GetMultiLine(addColumnLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.InsertColumn(index, ConvertIEnumerableArray(addItem));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] OverwriteRowTestCaseSource =
        {
            new object[] { -1, 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { 0, 0, 0, TestClass.ListType.Normal, false },
            new object[] { 0, InitMaxCapacity, InitItemCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxCapacity, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[] { 0, InitMaxCapacity, InitItemCapacity + 1, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxCapacity + 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { InitCapacity, 0, 0, TestClass.ListType.Normal, false },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity - 1, TestClass.ListType.Normal, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity, TestClass.ListType.Normal, false },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity, InitItemCapacity + 1, TestClass.ListType.Normal, true },
            new object[]
                { InitCapacity, InitMaxCapacity - InitCapacity + 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { InitCapacity + 1, 1, InitItemCapacity, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxCapacity, InitItemCapacity, TestClass.ListType.SelfNull, true },
            new object[] { 0, InitMaxCapacity, InitItemCapacity, TestClass.ListType.RowHasNull, true },
            new object[] { 0, InitMaxCapacity, InitItemCapacity, TestClass.ListType.ColumnHasNull, true },
            new object[] { 0, InitMaxCapacity, InitItemCapacity, TestClass.ListType.ColumnSizeDifference, true },
        };

        [TestCaseSource(nameof(OverwriteRowTestCaseSource))]
        public static void OverwriteRowTest(int row, int itemRowLength, int addLineItemLength,
            TestClass.ListType addType,
            bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var items = addType.GetMultiLine(itemRowLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.OverwriteRow(row, ConvertIEnumerableArray(items));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] OverwriteColumnTestCaseSource =
        {
            new object[] { -1, 1, InitCapacity, TestClass.ListType.Normal, true },
            new object[] { 0, 0, 0, TestClass.ListType.Normal, false },
            new object[] { 0, InitMaxItemCapacity, InitCapacity - 1, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxItemCapacity, InitCapacity, TestClass.ListType.Normal, false },
            new object[] { 0, InitMaxItemCapacity, InitCapacity + 1, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxItemCapacity + 1, InitCapacity, TestClass.ListType.Normal, true },
            new object[] { InitItemCapacity, 0, 0, TestClass.ListType.Normal, false },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity - 1, TestClass.ListType.Normal,
                true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity, TestClass.ListType.Normal, false
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity, InitCapacity + 1, TestClass.ListType.Normal,
                true
            },
            new object[]
            {
                InitItemCapacity, InitMaxItemCapacity - InitItemCapacity + 1, InitCapacity, TestClass.ListType.Normal,
                true
            },
            new object[] { InitItemCapacity + 1, 1, InitCapacity, TestClass.ListType.Normal, true },
            new object[] { 0, InitMaxItemCapacity, InitCapacity, TestClass.ListType.SelfNull, true },
            new object[] { 0, InitMaxItemCapacity, InitCapacity, TestClass.ListType.RowHasNull, true },
            new object[] { 0, InitMaxItemCapacity, InitCapacity, TestClass.ListType.ColumnHasNull, true },
            new object[] { 0, InitMaxItemCapacity, InitCapacity, TestClass.ListType.ColumnSizeDifference, true },
        };

        [TestCaseSource(nameof(OverwriteColumnTestCaseSource))]
        public static void OverwriteColumnTest(int column, int itemColumnLength, int addLineItemLength,
            TestClass.ListType addType, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var items = addType.GetMultiLine(itemColumnLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.OverwriteColumn(column, ConvertIEnumerableArray(items));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, 0, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, InitCapacity - InitMinCapacity, false)]
        [TestCase(0, InitCapacity - InitMinCapacity + 1, true)]
        [TestCase(InitCapacity - 1, -1, true)]
        [TestCase(InitCapacity - 1, 0, false)]
        [TestCase(InitCapacity - 1, 1, false)]
        [TestCase(InitCapacity - 1, 2, true)]
        [TestCase(InitCapacity, 0, true)]
        public static void RemoveRowTest(int index, int count, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.RemoveRow(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, 0, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, InitItemCapacity - InitMinItemCapacity, false)]
        [TestCase(0, InitItemCapacity - InitMinItemCapacity + 1, true)]
        [TestCase(InitItemCapacity - 1, -1, true)]
        [TestCase(InitItemCapacity - 1, 0, false)]
        [TestCase(InitItemCapacity - 1, 1, false)]
        [TestCase(InitItemCapacity - 1, 2, true)]
        [TestCase(InitItemCapacity, 0, true)]
        public static void RemoveColumnTest(int column, int count, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.RemoveColumn(column, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] AdjustLengthTestCaseSource =
        {
            new object[] { InitMinCapacity - 1, InitMinItemCapacity, true },
            new object[] { InitMinCapacity - 1, InitMaxItemCapacity, true },
            new object[] { InitMinCapacity, InitMinItemCapacity - 1, true },
            new object[] { InitMinCapacity, InitMinItemCapacity, false },
            new object[] { InitMinCapacity, InitMaxItemCapacity, false },
            new object[] { InitMinCapacity, InitMaxItemCapacity + 1, true },
            new object[] { InitMaxCapacity, InitMinItemCapacity - 1, true },
            new object[] { InitMaxCapacity, InitMinItemCapacity, false },
            new object[] { InitMaxCapacity, InitMaxItemCapacity, false },
            new object[] { InitMaxCapacity, InitMaxItemCapacity + 1, true },
            new object[] { InitMaxCapacity + 1, InitMinItemCapacity, true },
            new object[] { InitMaxCapacity + 1, InitMaxItemCapacity, true },
        };

        [TestCaseSource(nameof(AdjustLengthTestCaseSource))]
        public static void AdjustLengthTest(int rowLength, int columnLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustLength(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCaseSource(nameof(AdjustLengthTestCaseSource))]
        public static void AdjustLengthIfLongTest(int rowLength, int columnLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustLengthIfLong(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCaseSource(nameof(AdjustLengthTestCaseSource))]
        public static void AdjustLengthIfShortTest(int rowLength, int columnLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustLengthIfShort(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] AdjustRowLengthTestCaseSource =
        {
            new object[] { InitMinCapacity - 1, true },
            new object[] { InitMinCapacity, false },
            new object[] { InitMaxCapacity, false },
            new object[] { InitMaxCapacity + 1, true },
        };

        [TestCaseSource(nameof(AdjustRowLengthTestCaseSource))]
        public static void AdjustRowLengthTest(int rowLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustRowLength(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCaseSource(nameof(AdjustRowLengthTestCaseSource))]
        public static void AdjustRowLengthIfLongTest(int rowLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustRowLengthIfLong(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCaseSource(nameof(AdjustRowLengthTestCaseSource))]
        public static void AdjustRowLengthIfShortTest(int rowLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustRowLengthIfShort(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] AdjustColumnLengthTestCaseSource =
        {
            new object[] { InitMinItemCapacity - 1, true },
            new object[] { InitMinItemCapacity, false },
            new object[] { InitMaxItemCapacity, false },
            new object[] { InitMaxItemCapacity + 1, true },
        };

        [TestCaseSource(nameof(AdjustColumnLengthTestCaseSource))]
        public static void AdjustColumnLengthTest(int columnLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustColumnLength(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCaseSource(nameof(AdjustColumnLengthTestCaseSource))]
        public static void AdjustColumnLengthIfLongTest(int columnLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustColumnLengthIfLong(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCaseSource(nameof(AdjustColumnLengthTestCaseSource))]
        public static void AdjustColumnLengthIfShortTest(int columnLength, bool isError)
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.AdjustColumnLengthIfShort(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void ClearTest()
        {
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);
            var errorOccured = false;

            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 行数・列数が最小であること
            Assert.IsTrue(instance.RowCount == instance.GetMinRowCapacity());
            Assert.IsTrue(instance.ColumnCount == instance.GetMinColumnCapacity());
        }

        [TestCase(InitMinCapacity - 1, InitMinItemCapacity, true)]
        [TestCase(InitMinCapacity - 1, InitMaxItemCapacity, true)]
        [TestCase(InitMinCapacity, InitMinItemCapacity - 1, true)]
        [TestCase(InitMinCapacity, InitMinItemCapacity, false)]
        [TestCase(InitMinCapacity, InitMaxItemCapacity, false)]
        [TestCase(InitMinCapacity, InitMaxItemCapacity + 1, true)]
        [TestCase(InitMaxCapacity, InitMinItemCapacity - 1, true)]
        [TestCase(InitMaxCapacity, InitMinItemCapacity, false)]
        [TestCase(InitMaxCapacity, InitMaxItemCapacity, false)]
        [TestCase(InitMaxCapacity, InitMaxItemCapacity + 1, true)]
        [TestCase(InitMaxCapacity + 1, InitMinItemCapacity, true)]
        [TestCase(InitMaxCapacity + 1, InitMaxItemCapacity, true)]
        public static void ResetTest(int rowCount, int columnCount, bool isError)
        {
            var resetItems = TestClass.MakeStringList(rowCount, columnCount);
            var instance = TestClass.MakeTestInstance(InitCapacityInfo, InitCapacity, InitItemCapacity);

            var errorOccured = false;
            try
            {
                instance.Reset(resetItems);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static IEnumerable<TestClass.TwoDimItem>[] ConvertIEnumerableArray(TestClass.TwoDimItem[][] src)
            => src.Cast<IEnumerable<TestClass.TwoDimItem>>().ToArray();

        public static class TestClass
        {
            #region MakeTestInstance

            internal static TwoDimItem MakeDefaultValueItem(int row, int column)
                => $"({row}, {column})";

            private static TwoDimItem MakeDefaultValueItemForList(int row, int column)
                => $"{MakeDefaultValueItem(row, column)} for List";

            internal static TwoDimItem[][] MakeStringList(int rowCount, int columnCount)
            {
                return Enumerable.Range(0, rowCount).Select(rowIdx =>
                        Enumerable.Range(0, columnCount)
                            .Select(colIdx => MakeDefaultValueItemForList(rowIdx, colIdx))
                            .ToArray())
                    .ToArray();
            }

            private static TwoDimItem MakeDefaultValueItemForAddColumn(int row, int column)
                => $"({row}, {column}) for AddColumn";

            #endregion

            internal record CapacityInfo
            {
                public int MaxCapacity { get; }
                public int MinCapacity { get; }
                public int MaxItemCapacity { get; }
                public int MinItemCapacity { get; }

                public CapacityInfo(int maxCapacity, int minCapacity, int maxItemCapacity, int minItemCapacity)
                {
                    MaxCapacity = maxCapacity;
                    MinCapacity = minCapacity;
                    MaxItemCapacity = maxItemCapacity;
                    MinItemCapacity = minItemCapacity;
                }
            }

            internal class TwoDimItem : IEqualityComparable<TwoDimItem>
            {
                private string Value { get; }

                private TwoDimItem(string value)
                {
                    Value = value;
                }

                public bool ItemEquals(TwoDimItem other)
                {
                    return Value.Equals(other.Value);
                }

                public bool ItemEquals(object other)
                {
                    if (other is TwoDimItem casted) return ItemEquals(casted);
                    return false;
                }

                public static implicit operator TwoDimItem(string src)
                {
                    return src is null ? null : new TwoDimItem(src);
                }
            }

            internal class TestTwoDimClass : TwoDimensionalList<TwoDimItem>
            {
                public TestTwoDimClass(CapacityInfo capacityInfo,
                    IEnumerable<IEnumerable<TwoDimItem>> values, InjectValidator injection,
                    Func<int, int, TwoDimItem> funcMakeDefaultItem)
                    : base(values, new Config
                    {
                        ItemFactory = funcMakeDefaultItem,
                        ValidatorFactory = instance => injection(instance),
                        MaxRowCapacity = capacityInfo.MaxCapacity,
                        MinRowCapacity = capacityInfo.MinCapacity,
                        MaxColumnCapacity = capacityInfo.MaxItemCapacity,
                        MinColumnCapacity = capacityInfo.MinItemCapacity
                    })
                {
                }

                public TestTwoDimClass(CapacityInfo capacityInfo,
                    int rowLength, int columnLength, InjectValidator injection,
                    Func<int, int, TwoDimItem> funcMakeDefaultItem) : base(rowLength, columnLength, new Config
                {
                    ItemFactory = funcMakeDefaultItem,
                    ValidatorFactory = instance => injection(instance),
                    MaxRowCapacity = capacityInfo.MaxCapacity,
                    MinRowCapacity = capacityInfo.MinCapacity,
                    MaxColumnCapacity = capacityInfo.MaxItemCapacity,
                    MinColumnCapacity = capacityInfo.MinItemCapacity
                })
                {
                }

                public TestTwoDimClass(CapacityInfo capacityInfo,
                    InjectValidator injection, Func<int, int, TwoDimItem> funcMakeDefaultItem)
                    : base(new Config
                    {
                        ItemFactory = funcMakeDefaultItem,
                        ValidatorFactory = instance => injection(instance),
                        MaxRowCapacity = capacityInfo.MaxCapacity,
                        MinRowCapacity = capacityInfo.MinCapacity,
                        MaxColumnCapacity = capacityInfo.MaxItemCapacity,
                        MinColumnCapacity = capacityInfo.MinItemCapacity
                    })
                {
                }
            }

            internal static TestTwoDimClass MakeTestInstance(
                CapacityInfo capacityInfo,
                int initRowLength, int initColumnLength)
            {
                if (initRowLength == 0 && initColumnLength != 0) Assert.Ignore();

                var result = new TestTwoDimClass(capacityInfo, initRowLength, initColumnLength,
                    target => new RestrictedCapacityTwoDimensionalListValidator<TwoDimItem, TwoDimItem>(target, "行名",
                        "列名"),
                    MakeDefaultValueItemForList);

                return result;
            }

            /// <summary>
            ///     引数で与えるリストのnull種別
            /// </summary>
            public class ListType : Commons.TypeSafeEnum<ListType>
            {
                /// <summary>通常（null要素なし、列要素数のばらつきなし）</summary>
                public static ListType Normal;

                /// <summary>リスト自身がnull</summary>
                public static ListType SelfNull;

                /// <summary>n行要素ullあり（行数2以上の場合に有効）</summary>
                public static ListType RowHasNull;

                /// <summary>列要素nullあり（列数2以上の場合に有効）</summary>
                public static ListType ColumnHasNull;

                /// <summary>列要素数ばらつきあり（列数2以上の場合に有効）</summary>
                public static ListType ColumnSizeDifference;

                static ListType()
                {
                    Normal = new ListType(nameof(Normal));
                    SelfNull = new ListType(nameof(SelfNull));
                    RowHasNull = new ListType(nameof(RowHasNull));
                    ColumnHasNull = new ListType(nameof(ColumnHasNull));
                    ColumnSizeDifference = new ListType(nameof(ColumnSizeDifference));
                }

                private ListType(string id) : base(id)
                {
                }

                public static ListType FromId(string id)
                {
                    return AllItems.First(x => x.Id.Equals(id));
                }

                internal TwoDimItem[] GetLine(int itemCount)
                {
                    if (this == SelfNull) return null;

                    var funcMakeItem = (Func<int, TwoDimItem>)(i =>
                        this == ColumnHasNull && i % 2 == 0
                            ? (string)null
                            : MakeDefaultValueItemForAddColumn(0, i));

                    return Enumerable.Range(0, itemCount)
                        .Select(funcMakeItem)
                        .ToArray();
                }

                internal TwoDimItem[][] GetMultiLine(int rowCount, int columnCount)
                {
                    if (this == SelfNull) return null;

                    var funcMakeItem = (Func<int, int, TwoDimItem>)((i, j) =>
                        this == ColumnHasNull && j % 2 == 1
                            ? null
                            : MakeDefaultValueItemForAddColumn(i, j));
                    var funcMakeRow = (Func<int, int, TwoDimItem[]>)((i, colCount) =>
                        this == RowHasNull && i % 2 == 1
                            ? null
                            : Enumerable.Range(0, this == ColumnSizeDifference && i == 1
                                    ? colCount - 1
                                    : colCount)
                                .Select(j => funcMakeItem(i, j))
                                .ToArray());

                    return Enumerable.Range(0, rowCount)
                        .Select(i => funcMakeRow(i, columnCount))
                        .ToArray();
                }

                public override string ToString()
                {
                    return Id;
                }
            }
        }
    }
}
