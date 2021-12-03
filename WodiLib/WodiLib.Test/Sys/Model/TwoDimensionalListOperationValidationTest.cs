using System;
using Commons;
using NUnit.Framework;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using TestTools = WodiLib.Test.Sys.TwoDimensionalListTest_Tools;
using TestRecord = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecord;
using TestRecordList = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecordList;
using TestDoubleEnumerableInstanceType = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestDoubleEnumerableInstanceType;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class TwoDimensionalListOperationValidateTest
    {
        /*
         * 操作の引数検証が正しく行われることをテストする。
         * CommonTwoDimensionalListValidator のテストを兼ねる。
         *
         * 処理前後の通知の正しさや処理そのものの結果の正しさはテストしない。
         * (TwoDimensionalListTest, TwoDimensionalListOperationValidateTest、TwoDimensionalListNotifyTestで確認)
         */

        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        #region Constructor

        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Null), true)]
        public static void ConstructorTest_A(string initValueType, bool isError)
        {
            var initItems = TestTools.MakeTestRecordList(initValueType, false, TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = new TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord>(initItems,
                    TestTools.CreateCommonConfig(TestTools.MakeListDefaultItem));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, 0, true)]
        [TestCase(0, 0, false)]
        [TestCase(TestTools.InitRowLength, -1, true)]
        [TestCase(TestTools.InitRowLength, 0, false)]
        [TestCase(TestTools.InitRowLength, TestTools.InitColumnLength, false)]
        public static void ConstructorTest_B(int initRowLength, int initColumnLength, bool isError)
        {
            var errorOccured = false;
            try
            {
                _ = new TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord>(initRowLength,
                    initColumnLength,
                    TestTools.CreateCommonConfig(TestTools.MakeListDefaultItem));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        public static void ConstructorTest_C()
        {
            var errorOccured = false;
            try
            {
                _ = new TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord>(
                    TestTools.CreateCommonConfig(TestTools.MakeListDefaultItem));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        #endregion

        #region Accessor

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(-1, TestTools.InitColumnLength - 1, true)]
        [TestCase(-1, TestTools.InitColumnLength, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, -1, true)]
        [TestCase(0, TestTools.InitColumnLength - 1, false)]
        [TestCase(0, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength - 1, -1, true)]
        [TestCase(TestTools.InitRowLength - 1, 0, false)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 1, false)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength, -1, true)]
        [TestCase(TestTools.InitRowLength, 0, true)]
        [TestCase(TestTools.InitRowLength, TestTools.InitColumnLength - 1, true)]
        [TestCase(TestTools.InitRowLength, TestTools.InitColumnLength, true)]
        public static void AccessorTest_Item_Get(int rowIndex, int columnIndex, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance[rowIndex, columnIndex];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, -1, false, true)]
        [TestCase(-1, 0, false, true)]
        [TestCase(-1, TestTools.InitColumnLength - 1, false, true)]
        [TestCase(-1, TestTools.InitColumnLength, false, true)]
        [TestCase(0, -1, false, true)]
        [TestCase(0, 0, true, true)]
        [TestCase(0, 0, false, false)]
        [TestCase(0, TestTools.InitColumnLength - 1, true, true)]
        [TestCase(0, TestTools.InitColumnLength - 1, false, false)]
        [TestCase(0, TestTools.InitColumnLength, false, true)]
        [TestCase(TestTools.InitRowLength - 1, -1, false, true)]
        [TestCase(TestTools.InitRowLength - 1, 0, true, true)]
        [TestCase(TestTools.InitRowLength - 1, 0, false, false)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 1, true, true)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 1, false, false)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength, false, true)]
        [TestCase(TestTools.InitRowLength, -1, false, true)]
        [TestCase(TestTools.InitRowLength, 0, false, true)]
        [TestCase(TestTools.InitRowLength, TestTools.InitColumnLength - 1, false, true)]
        [TestCase(TestTools.InitRowLength, TestTools.InitColumnLength, false, true)]
        public static void AccessorTest_Item_Set(int rowIndex, int columnIndex, bool setItemIsNull, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var setItem = setItemIsNull ? null : TestTools.MakeInsertItem(rowIndex, columnIndex);

            var errorOccured = false;
            try
            {
                instance[rowIndex, columnIndex] = setItem;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region Get

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(-1, TestTools.InitRowLength, true)]
        [TestCase(-1, TestTools.InitRowLength + 1, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, TestTools.InitRowLength, false)]
        [TestCase(0, TestTools.InitRowLength + 1, true)]
        [TestCase(TestTools.InitRowLength - 1, -1, true)]
        [TestCase(TestTools.InitRowLength - 1, 0, false)]
        [TestCase(TestTools.InitRowLength - 1, 1, false)]
        [TestCase(TestTools.InitRowLength - 1, 2, true)]
        [TestCase(TestTools.InitRowLength, -1, true)]
        [TestCase(TestTools.InitRowLength, 0, true)]
        [TestCase(TestTools.InitRowLength, 1, true)]
        public static void GetRowTest(int index, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance.GetRow(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(-1, TestTools.InitColumnLength, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, TestTools.InitColumnLength, false)]
        [TestCase(0, TestTools.InitColumnLength + 1, true)]
        [TestCase(TestTools.InitColumnLength - 1, -1, true)]
        [TestCase(TestTools.InitColumnLength - 1, 0, false)]
        [TestCase(TestTools.InitColumnLength - 1, 1, false)]
        [TestCase(TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(TestTools.InitColumnLength, -1, true)]
        [TestCase(TestTools.InitColumnLength, 0, true)]
        [TestCase(TestTools.InitColumnLength, 1, true)]
        public static void GetColumnTest(int index, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance.GetColumn(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, 0, 0, TestTools.InitColumnLength, true)]
        [TestCase(-1, 1, TestTools.InitColumnLength - 1, 1, true)]
        [TestCase(-1, 2, TestTools.InitColumnLength - 1, 0, true)]
        [TestCase(-1, TestTools.InitRowLength, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(0, -1, 0, 0, true)]
        [TestCase(0, -1, TestTools.InitColumnLength - 1, 1, true)]
        [TestCase(0, 0, 0, 0, false)]
        [TestCase(0, 0, 0, TestTools.InitColumnLength, false)]
        [TestCase(0, 0, 0, TestTools.InitColumnLength + 1, true)]
        [TestCase(0, 0, TestTools.InitColumnLength - 1, -1, true)]
        [TestCase(0, 0, TestTools.InitColumnLength - 1, 1, false)]
        [TestCase(0, 0, TestTools.InitColumnLength, 2, true)]
        [TestCase(0, TestTools.InitRowLength, -1, 0, true)]
        [TestCase(0, TestTools.InitRowLength, 0, 0, false)]
        [TestCase(0, TestTools.InitRowLength, 0, TestTools.InitColumnLength, false)]
        [TestCase(0, TestTools.InitRowLength, 0, TestTools.InitColumnLength + 1, true)]
        [TestCase(0, TestTools.InitRowLength, TestTools.InitColumnLength - 1, -1, true)]
        [TestCase(0, TestTools.InitRowLength, TestTools.InitColumnLength - 1, 1, false)]
        [TestCase(0, TestTools.InitRowLength, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(0, TestTools.InitRowLength, TestTools.InitColumnLength, 1, true)]
        [TestCase(0, TestTools.InitRowLength + 1, 0, TestTools.InitColumnLength, true)]
        [TestCase(0, TestTools.InitRowLength + 1, TestTools.InitColumnLength - 1, 0, true)]
        [TestCase(0, TestTools.InitRowLength + 1, TestTools.InitColumnLength - 1, 1, true)]
        [TestCase(0, TestTools.InitRowLength + 1, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(TestTools.InitRowLength - 1, -1, 0, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength - 1, -1, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(TestTools.InitRowLength - 1, 0, -1, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength - 1, 0, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(TestTools.InitRowLength - 1, 1, -1, 2, true)]
        [TestCase(TestTools.InitRowLength - 1, 1, 0, -1, true)]
        [TestCase(TestTools.InitRowLength - 1, 1, 0, 0, false)]
        [TestCase(TestTools.InitRowLength - 1, 1, 0, TestTools.InitColumnLength + 1, true)]
        [TestCase(TestTools.InitRowLength - 1, 1, 0, TestTools.InitColumnLength, false)]
        [TestCase(TestTools.InitRowLength - 1, 1, TestTools.InitColumnLength - 1, 1, false)]
        [TestCase(TestTools.InitRowLength - 1, 1, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(TestTools.InitRowLength - 1, 1, TestTools.InitColumnLength, 0, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, -1, 1, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, 0, -1, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, 0, TestTools.InitColumnLength + 1, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, 0, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, TestTools.InitColumnLength - 1, 0, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, TestTools.InitColumnLength - 1, 1, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, TestTools.InitColumnLength, 2, true)]
        [TestCase(TestTools.InitRowLength - 1, 2, TestTools.InitColumnLength, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength, 0, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(TestTools.InitRowLength, 1, TestTools.InitColumnLength - 1, 1, true)]
        [TestCase(TestTools.InitRowLength, 2, 0, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength, TestTools.InitRowLength, TestTools.InitColumnLength - 1, 0, true)]
        public static void GetItemTest(int row, int rowCount, int column, int columnCount, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance.GetItem(row, rowCount, column, columnCount);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region Set

        [TestCase(-1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.Empty), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic),
            false)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort),
            true)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong),
            true)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic),
            true)]
        [TestCase(TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), true)]
        public static void SetRowTest(int row, string setItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var setItems =
                TestTools.MakeRowList(TestTools.MakeTestRecordList(setItemType, false, TestTools.MakeInsertItem));
            var errorOccured = false;
            try
            {
                instance.SetRow(row, setItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.Empty), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne),
            false)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne),
            true)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne),
            true)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo),
            true)]
        [TestCase(TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne),
            true)]
        public static void SetColumnTest(int index, string setItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var setItems = TestTools.MakeTestRecordList(setItemType, true, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.SetColumn(index, setItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region Add

        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), false)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        public static void AddRowTest(bool targetIsEmpty, string addItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var addItems =
                TestTools.MakeRowList(TestTools.MakeTestRecordList(addItemType, false, TestTools.MakeInsertItem));
            var errorOccured = false;
            try
            {
                instance.AddRow(addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(true, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), false)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        public static void AddColumnTest(bool targetIsEmpty, string addItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var addItems = TestTools.MakeTestRecordList(addItemType, true, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.AddColumn(addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region Insert

        [TestCase(true, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(true, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(true, 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(true, 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), true)]
        [TestCase(false, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(false, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitRowLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, TestTools.InitRowLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), false)]
        [TestCase(false, TestTools.InitRowLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong),
            true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitRowLength + 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(false, TestTools.InitRowLength + 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), true)]
        public static void InsertRowRangeTest(bool targetIsEmpty, int index, string insertItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var insertItems =
                TestTools.MakeRowList(TestTools.MakeTestRecordList(insertItemType, false, TestTools.MakeInsertItem));
            var errorOccured = false;
            try
            {
                instance.InsertRow(index, insertItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(true, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(true, 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(true, 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), true)]
        [TestCase(false, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(false, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitColumnLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, TestTools.InitColumnLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), false)]
        [TestCase(false, TestTools.InitColumnLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne),
            true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitColumnLength + 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(false, TestTools.InitColumnLength + 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), true)]
        public static void InsertColumnRangeTest(bool targetIsEmpty, int index, string insertItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var insertItems = TestTools.MakeTestRecordList(insertItemType, true, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.InsertColumn(index, insertItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region Overwrite

        [TestCase(true, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(true, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(true, 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(true, 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), true)]
        [TestCase(false, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(false, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitRowLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, TestTools.InitRowLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), false)]
        [TestCase(false, TestTools.InitRowLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong),
            true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitRowLength + 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(false, TestTools.InitRowLength + 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic), true)]
        public static void OverwriteRowTest(bool targetIsEmpty, int index, string overwriteItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var overwriteItems =
                TestTools.MakeRowList(TestTools.MakeTestRecordList(overwriteItemType, false, TestTools.MakeInsertItem));
            var errorOccured = false;
            try
            {
                instance.OverwriteRow(index, overwriteItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(true, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(true, 0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(true, 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(true, 1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), true)]
        [TestCase(false, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(false, -1, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, 0, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitColumnLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(false, TestTools.InitColumnLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), false)]
        [TestCase(false, TestTools.InitColumnLength,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne),
            true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.EmptyRows), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitColumnLength + 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true)]
        [TestCase(false, TestTools.InitColumnLength + 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), true)]
        public static void OverwriteColumnTest(bool targetIsEmpty, int index, string overwriteItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var overwriteItems = TestTools.MakeTestRecordList(overwriteItemType, true, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.OverwriteColumn(index, overwriteItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region Move

        [TestCase(true, -1, -1, 0, true)]
        [TestCase(true, 0, -1, 0, true)]
        [TestCase(true, -1, 0, 0, true)]
        [TestCase(true, 0, 0, 0, true)]
        [TestCase(false, -1, -1, TestTools.InitRowLength, true)]
        [TestCase(false, -1, 0, TestTools.InitRowLength, true)]
        [TestCase(false, -1, TestTools.InitRowLength - 1, TestTools.InitRowLength, true)]
        [TestCase(false, -1, TestTools.InitRowLength, TestTools.InitRowLength, true)]
        [TestCase(false, 0, -1, TestTools.InitRowLength, true)]
        [TestCase(false, 0, 0, TestTools.InitRowLength, false)]
        [TestCase(false, 0, 0, TestTools.InitRowLength + 1, true)]
        [TestCase(false, 0, TestTools.InitRowLength - 1, 1, false)]
        [TestCase(false, 0, TestTools.InitRowLength - 1, 2, true)]
        [TestCase(false, 0, TestTools.InitRowLength, 0, false)]
        [TestCase(false, 0, TestTools.InitRowLength, 1, true)]
        [TestCase(false, TestTools.InitRowLength - 1, -1, 1, true)]
        [TestCase(false, TestTools.InitRowLength - 1, 0, 1, false)]
        [TestCase(false, TestTools.InitRowLength - 1, 0, 2, true)]
        [TestCase(false, TestTools.InitRowLength - 1, TestTools.InitRowLength - 1, 1, false)]
        [TestCase(false, TestTools.InitRowLength - 1, TestTools.InitRowLength - 1, 2, true)]
        [TestCase(false, TestTools.InitRowLength - 1, TestTools.InitRowLength, 0, false)]
        [TestCase(false, TestTools.InitRowLength - 1, TestTools.InitRowLength, 1, true)]
        [TestCase(false, TestTools.InitRowLength, -1, 0, true)]
        [TestCase(false, TestTools.InitRowLength, 0, 0, true)]
        [TestCase(false, TestTools.InitRowLength, TestTools.InitRowLength - 1, 0, true)]
        [TestCase(false, TestTools.InitRowLength, TestTools.InitRowLength, 0, true)]
        public static void MoveRowTest(bool targetIsEmpty, int oldIndex, int newIndex,
            int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var errorOccured = false;
            try
            {
                instance.MoveRow(oldIndex, newIndex, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, -1, -1, 0, true)]
        [TestCase(true, 0, -1, 0, true)]
        [TestCase(true, -1, 0, 0, true)]
        [TestCase(true, 0, 0, 0, true)]
        [TestCase(false, -1, -1, TestTools.InitColumnLength, true)]
        [TestCase(false, -1, 0, TestTools.InitColumnLength, true)]
        [TestCase(false, -1, TestTools.InitColumnLength - 1, TestTools.InitColumnLength, true)]
        [TestCase(false, -1, TestTools.InitColumnLength, TestTools.InitColumnLength, true)]
        [TestCase(false, 0, -1, TestTools.InitColumnLength, true)]
        [TestCase(false, 0, 0, TestTools.InitColumnLength, false)]
        [TestCase(false, 0, 0, TestTools.InitColumnLength + 1, true)]
        [TestCase(false, 0, TestTools.InitColumnLength - 1, 1, false)]
        [TestCase(false, 0, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(false, 0, TestTools.InitColumnLength, 0, false)]
        [TestCase(false, 0, TestTools.InitColumnLength, 1, true)]
        [TestCase(false, TestTools.InitColumnLength - 1, -1, 1, true)]
        [TestCase(false, TestTools.InitColumnLength - 1, 0, 1, false)]
        [TestCase(false, TestTools.InitColumnLength - 1, 0, 2, true)]
        [TestCase(false, TestTools.InitColumnLength - 1, TestTools.InitColumnLength - 1, 1, false)]
        [TestCase(false, TestTools.InitColumnLength - 1, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(false, TestTools.InitColumnLength - 1, TestTools.InitColumnLength, 0, false)]
        [TestCase(false, TestTools.InitColumnLength - 1, TestTools.InitColumnLength, 1, true)]
        [TestCase(false, TestTools.InitColumnLength, -1, 0, true)]
        [TestCase(false, TestTools.InitColumnLength, 0, 0, true)]
        [TestCase(false, TestTools.InitColumnLength, TestTools.InitColumnLength - 1, 0, true)]
        [TestCase(false, TestTools.InitColumnLength, TestTools.InitColumnLength, 0, true)]
        public static void MoveColumnTest(bool targetIsEmpty, int oldIndex, int newIndex, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var errorOccured = false;
            try
            {
                instance.MoveColumn(oldIndex, newIndex, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region Remove

        [TestCase(true, -1, -1, true)]
        [TestCase(true, -1, 0, true)]
        [TestCase(true, 0, -1, true)]
        [TestCase(true, 0, 0, true)]
        [TestCase(false, -1, 0, true)]
        [TestCase(false, -1, TestTools.InitRowLength, true)]
        [TestCase(false, 0, -1, true)]
        [TestCase(false, 0, 0, false)]
        [TestCase(false, 0, TestTools.InitRowLength, false)]
        [TestCase(false, 0, TestTools.InitRowLength + 1, true)]
        [TestCase(false, TestTools.InitRowLength - 1, -1, true)]
        [TestCase(false, TestTools.InitRowLength - 1, 0, false)]
        [TestCase(false, TestTools.InitRowLength - 1, 1, false)]
        [TestCase(false, TestTools.InitRowLength - 1, 2, true)]
        [TestCase(false, TestTools.InitRowLength, 0, true)]
        [TestCase(false, TestTools.InitRowLength, TestTools.InitRowLength, true)]
        public static void RemoveRowTest(bool targetIsEmpty, int index, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, -1, -1, true)]
        [TestCase(true, -1, 0, true)]
        [TestCase(true, 0, -1, true)]
        [TestCase(true, 0, 0, true)]
        [TestCase(false, -1, 0, true)]
        [TestCase(false, -1, TestTools.InitColumnLength, true)]
        [TestCase(false, 0, -1, true)]
        [TestCase(false, 0, 0, false)]
        [TestCase(false, 0, TestTools.InitColumnLength, false)]
        [TestCase(false, 0, TestTools.InitColumnLength + 1, true)]
        [TestCase(false, TestTools.InitColumnLength - 1, -1, true)]
        [TestCase(false, TestTools.InitColumnLength - 1, 0, false)]
        [TestCase(false, TestTools.InitColumnLength - 1, 1, false)]
        [TestCase(false, TestTools.InitColumnLength - 1, 2, true)]
        [TestCase(false, TestTools.InitColumnLength, 0, true)]
        [TestCase(false, TestTools.InitColumnLength, TestTools.InitColumnLength, true)]
        public static void RemoveColumnTest(bool targetIsEmpty, int index, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var errorOccured = false;
            try
            {
                instance.RemoveColumn(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region AdjustLength

        [TestCase(-1, 0, true)]
        [TestCase(-1, TestTools.InitColumnLength, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength, -1, true)]
        [TestCase(TestTools.InitRowLength, 0, false)]
        [TestCase(TestTools.InitRowLength, TestTools.InitColumnLength, false)]
        public static void AdjustLengthTest(int rowLength, int columnLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, TestTools.InitColumnLength, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength, -1, true)]
        [TestCase(TestTools.InitRowLength, 0, false)]
        [TestCase(TestTools.InitRowLength, TestTools.InitColumnLength, false)]
        public static void AdjustLengthIfShortTest(int rowLength, int columnLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, TestTools.InitColumnLength, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, TestTools.InitColumnLength, true)]
        [TestCase(TestTools.InitRowLength, -1, true)]
        [TestCase(TestTools.InitRowLength, 0, false)]
        [TestCase(TestTools.InitRowLength, TestTools.InitColumnLength, false)]
        public static void AdjustLengthIfLongTest(int rowLength, int columnLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(false, -1, true)]
        [TestCase(false, 0, true)]
        [TestCase(false, TestTools.InitRowLength + 1, false)]
        [TestCase(true, -1, true)]
        [TestCase(true, 0, false)]
        [TestCase(true, TestTools.InitRowLength + 1, false)]
        public static void AdjustRowLengthTest(bool targetIsEmptyColumn, int rowLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmptyColumn
                    ? TestDoubleEnumerableInstanceType.EmptyRows
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(false, -1, true)]
        [TestCase(false, 0, true)]
        [TestCase(false, TestTools.InitRowLength + 1, false)]
        [TestCase(true, -1, true)]
        [TestCase(true, 0, false)]
        [TestCase(true, TestTools.InitRowLength + 1, false)]
        public static void AdjustRowLengthIfShortTest(bool targetIsEmptyColumn, int rowLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmptyColumn
                    ? TestDoubleEnumerableInstanceType.EmptyRows
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(false, -1, true)]
        [TestCase(false, 0, true)]
        [TestCase(false, TestTools.InitRowLength + 1, false)]
        [TestCase(true, -1, true)]
        [TestCase(true, 0, false)]
        [TestCase(true, TestTools.InitRowLength + 1, false)]
        public static void AdjustRowLengthIfLongTest(bool targetIsEmptyColumn, int rowLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmptyColumn
                    ? TestDoubleEnumerableInstanceType.EmptyRows
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(TestTools.InitColumnLength, false)]
        public static void AdjustColumnLengthTest(int columnLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(TestTools.InitColumnLength, false)]
        public static void AdjustColumnLengthIfShortTest(int columnLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(TestTools.InitColumnLength, false)]
        public static void AdjustColumnLengthIfLongTest(int columnLength, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
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
            Assert.AreEqual(isError, errorOccured);
        }

        #endregion

        #region Reset

        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Empty), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.EmptyRows), false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.HasNullRow), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.HasNullColumn), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Jagged), true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Null), true)]
        public static void ResetTest(string resetItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var resetItems =
                TestTools.MakeRowList(TestTools.MakeTestRecordList(resetItemType, false, TestTools.MakeInsertItem));
            var errorOccured = false;
            try
            {
                instance.Reset(resetItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [Test]
        public static void ClearTest()
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

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
            Assert.AreEqual(false, errorOccured);
        }

        #endregion
    }
}
