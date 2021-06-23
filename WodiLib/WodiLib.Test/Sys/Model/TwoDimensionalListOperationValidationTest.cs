using System;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using TestTools = WodiLib.Test.Sys.TwoDimensionalListTest_Tools;
using TestRecord = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecord;
using TestSingleEnumerableInstanceType = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestSingleEnumerableInstanceType;
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

        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), true, false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), false, false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Empty), true, false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.EmptyRows), false, false)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.HasNullRow), true, true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.HasNullColumn), false, true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Jagged), false, true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Null), true, true)]
        [TestCase(nameof(TestDoubleEnumerableInstanceType.Null), false, true)]
        public static void ConstructorTest_A(string initValueType,
            bool validatorIsNull, bool isError)
        {
            var initItems = TestTools.MakeTestRecordList(initValueType, false, TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = new TwoDimensionalList<TestRecord>(initItems,
                    target => validatorIsNull ? null : new CommonTwoDimensionalListValidator<TestRecord>(target),
                    TestTools.MakeListDefaultItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        /* makeFuncIsNull == true のテストは initRowLength / initColumnLength の値不問 */
        [TestCase(-1, 0, true, true, true)]
        [TestCase(-1, 0, false, true, true)]
        [TestCase(-1, 0, true, false, true)]
        [TestCase(0, 0, true, false, true)]
        [TestCase(0, 0, false, true, false)]
        [TestCase(0, 0, false, false, false)]
        [TestCase(TestTools.InitRowLength, -1, false, true, true)]
        [TestCase(TestTools.InitRowLength, 0, false, false, false)]
        [TestCase(TestTools.InitRowLength, TestTools.InitRowLength, true, true, true)]
        public static void ConstructorTest_B(int initRowLength, int initColumnLength,
            bool makeFuncIsNull, bool validatorIsNull, bool isError)
        {
            Func<int, int, TestRecord> func;
            if (makeFuncIsNull)
            {
                func = null;
            }
            else
            {
                func = TestTools.MakeListDefaultItem;
            }

            var errorOccured = false;
            try
            {
                _ = new TwoDimensionalList<TestRecord>(initRowLength, initColumnLength,
                    target => new CommonTwoDimensionalListValidator<TestRecord>(target),
                    // ReSharper disable once AssignNullToNotNullAttribute
                    func);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, true, true)]
        [TestCase(true, false, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public static void ConstructorTest_C(bool makeFuncIsNull,
            bool validatorIsNull, bool isError)
        {
            Func<int, int, TestRecord> func;
            if (makeFuncIsNull)
            {
                func = null;
            }
            else
            {
                func = TestTools.MakeListDefaultItem;
            }

            var errorOccured = false;
            try
            {
                _ = new TwoDimensionalList<TestRecord>(
                    target => new CommonTwoDimensionalListValidator<TestRecord>(target),
                    // ReSharper disable once AssignNullToNotNullAttribute
                    func);
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

        #region Accessor

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(TestTools.InitRowLength - 1, false)]
        [TestCase(TestTools.InitRowLength, true)]
        public static void AccessorTest_Line_Get(int index, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance[index];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(TestTools.InitRowLength - 1, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(TestTools.InitRowLength, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        public static void AccessorTest_Line_Set(int index, string itemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var setItems = TestTools.MakeTestRecords(itemType, false, TestTools.MakeInsertItem);

            var errorOccured = false;
            try
            {
                instance[index] = setItems;
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

        #region CopyTo

        private static readonly object[] CopyToTest_ToArrayWithDirectionTestCaseSource =
        {
            new object[] {-1, true, Direction.Column, true},
            new object[] {-1, true, Direction.None, true},
            new object[] {-1, true, null, true},
            new object[] {-1, false, Direction.Row, true},
            new object[] {0, true, null, true},
            new object[] {0, true, Direction.Column, true},
            new object[] {0, true, Direction.None, true},
            new object[] {0, false, Direction.Row, false},
            new object[] {TestTools.OneArrayBuffer, true, null, true},
            new object[] {TestTools.OneArrayBuffer, true, Direction.Column, true},
            new object[] {TestTools.OneArrayBuffer, false, Direction.None, true},
            new object[] {TestTools.OneArrayBuffer, false, Direction.Row, false},
            new object[] {TestTools.OneArrayBuffer, false, Direction.Column, false},
            new object[] {TestTools.OneArrayBuffer + 1, true, Direction.Row, true},
            new object[] {TestTools.OneArrayBuffer + 1, false, null, true},
            new object[] {TestTools.OneArrayBuffer + 1, false, Direction.Column, true},
            new object[] {TestTools.OneArrayBuffer + 1, false, Direction.None, true},
        };

        [TestCaseSource(nameof(CopyToTest_ToArrayWithDirectionTestCaseSource))]
        public static void CopyToTest_ToArrayWithDirection(int index, bool dstArrayIsNull, Direction direction,
            bool isError)
        {
            var array = TestTools.MakeSingleArray(dstArrayIsNull);

            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                instance.CopyTo(array, index, direction);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, 0, false, true)]
        [TestCase(-1, TestTools.TwoDimArrayColumnBuffer, false, true)]
        [TestCase(0, -1, false, true)]
        [TestCase(0, 0, false, false)]
        [TestCase(0, 0, true, true)]
        [TestCase(0, TestTools.TwoDimArrayColumnBuffer, false, false)]
        [TestCase(0, TestTools.TwoDimArrayColumnBuffer + 1, false, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, -1, false, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, 0, false, false)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, TestTools.TwoDimArrayColumnBuffer, true, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, TestTools.TwoDimArrayColumnBuffer, false, false)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, TestTools.TwoDimArrayColumnBuffer + 1, false, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer + 1, 0, false, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer + 1, TestTools.TwoDimArrayColumnBuffer, false, true)]
        public static void CopyToTest_ToArray_RectangularArray(int row, int column,
            bool dstArrayIsNull, bool isError)
        {
            var array = TestTools.MakeDoubleRegularArray(dstArrayIsNull);

            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                instance.CopyTo(array, row, column);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, 0, false, true)]
        [TestCase(-1, TestTools.TwoDimArrayColumnBuffer, false, true)]
        [TestCase(0, -1, false, true)]
        [TestCase(0, 0, false, false)]
        [TestCase(0, 0, true, true)]
        [TestCase(0, TestTools.TwoDimArrayColumnBuffer, false, false)]
        [TestCase(0, TestTools.TwoDimArrayColumnBuffer + 1, false, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, -1, false, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, 0, false, false)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, TestTools.TwoDimArrayColumnBuffer, true, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, TestTools.TwoDimArrayColumnBuffer, false, false)]
        [TestCase(TestTools.TwoDimArrayRowBuffer, TestTools.TwoDimArrayColumnBuffer + 1, false, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer + 1, 0, false, true)]
        [TestCase(TestTools.TwoDimArrayRowBuffer + 1, TestTools.TwoDimArrayColumnBuffer, false, true)]
        public static void CopyToTest_ToArray_JaggedArray(int row, int column,
            bool dstArrayIsNull, bool isError)
        {
            var array = TestTools.MakeDoubleJaggedArray(dstArrayIsNull);

            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                instance.CopyTo(array, row, column);
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
        public static void GetRange_Row_Test(int index, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance.GetRange(index, count);
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
        [TestCase(TestTools.InitColumnLength - 1, false)]
        [TestCase(TestTools.InitColumnLength, true)]
        public static void GetItem_Test(int index, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance.GetItem(index);
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
        public static void GetItemRange_Test(int index, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance.GetItemRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        private static readonly object[] GetRange_Direction_TestCaseSource =
        {
            new object[] {-1, -1, 0, TestTools.InitRowLength, Direction.Row, true},
            new object[] {0, -1, 0, TestTools.InitRowLength, Direction.Row, true},
            new object[] {0, 0, 0, 0, null, true},
            new object[] {0, 0, 0, 0, Direction.Row, false},
            new object[] {0, 0, 0, 0, Direction.Column, false},
            new object[] {0, 0, 0, TestTools.InitRowLength, Direction.Column, false},
            new object[] {0, 0, TestTools.InitRowLength - 1, 1, Direction.Row, false},
            new object[] {0, TestTools.InitRowLength - 1, 0, TestTools.InitColumnLength + 1, Direction.Column, true},
            new object[] {0, TestTools.InitRowLength, -1, -1, Direction.Row, true},
            new object[] {0, TestTools.InitRowLength, 0, -1, Direction.Row, true},
            new object[] {0, TestTools.InitRowLength, 0, 0, Direction.Row, false},
            new object[] {0, TestTools.InitRowLength, 0, TestTools.InitColumnLength, Direction.Column, false},
            new object[] {0, TestTools.InitRowLength, 0, TestTools.InitColumnLength + 1, Direction.Column, true},
            new object[] {0, TestTools.InitRowLength, TestTools.InitColumnLength - 1, -1, Direction.Row, true},
            new object[] {0, TestTools.InitRowLength, TestTools.InitColumnLength - 1, 0, Direction.Column, false},
            new object[] {0, TestTools.InitRowLength, TestTools.InitColumnLength - 1, 1, Direction.Column, false},
            new object[] {0, TestTools.InitRowLength, TestTools.InitColumnLength - 1, 2, Direction.Row, true},
            new object[] {0, TestTools.InitRowLength, TestTools.InitColumnLength, -1, Direction.Row, true},
            new object[] {0, TestTools.InitRowLength, TestTools.InitColumnLength, 0, Direction.Row, true},
            new object[] {TestTools.InitRowLength - 1, -1, 0, TestTools.InitColumnLength, Direction.Row, true},
            new object[] {TestTools.InitRowLength - 1, 0, 0, 0, Direction.Column, false},
            new object[] {TestTools.InitRowLength - 1, 1, 0, 0, Direction.Column, false},
            new object[] {TestTools.InitRowLength - 1, 2, 0, TestTools.InitColumnLength, Direction.Column, true},
            new object[] {TestTools.InitRowLength, -1, 0, TestTools.InitColumnLength, Direction.Row, true},
            new object[] {TestTools.InitRowLength, 0, 0, TestTools.InitColumnLength, Direction.Row, true},
        };

        [TestCaseSource(nameof(GetRange_Direction_TestCaseSource))]
        public static void GetRange_Direction_Test(int row, int count, int column,
            int itemCount, Direction direction, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                _ = instance.GetRange(row, count, column, itemCount, direction);
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
        public static void SetRangeTest(int row, string setItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var setItems = TestTools.MakeTestRecordList(setItemType, false, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.SetRange(row, setItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.Empty), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(0, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestSingleEnumerableInstanceType.Empty), true)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(TestTools.InitColumnLength - 1, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(TestTools.InitColumnLength, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        public static void SetItemTest(int index, string setItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var setItems = TestTools.MakeTestRecords(setItemType, true, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.SetItem(index, setItems);
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
        public static void SetItemRangeTest(int index, string setItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var setItems = TestTools.MakeTestRecordList(setItemType, true, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.SetItemRange(index, setItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(-1, 0, nameof(TestDoubleEnumerableInstanceType.Empty), nameof(Direction.Row), true)]
        [TestCase(0, -1, nameof(TestDoubleEnumerableInstanceType.Empty), nameof(Direction.Row), true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), nameof(Direction.Row),
            false)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), nameof(Direction.None),
            true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic), null, true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic), nameof(Direction.Row),
            true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong), nameof(Direction.Row),
            true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong), nameof(Direction.None),
            true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.Empty), nameof(Direction.Row), false)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.Empty), nameof(Direction.Column), false)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.Empty), null, true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), nameof(Direction.Row), false)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), nameof(Direction.Column), false)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.EmptyRows), null, true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.HasNullRow), nameof(Direction.Row), true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.HasNullColumn), nameof(Direction.Row), true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.Jagged), nameof(Direction.Row), true)]
        [TestCase(0, 0, nameof(TestDoubleEnumerableInstanceType.Null), nameof(Direction.Row), true)]
        [TestCase(0, TestTools.InitColumnLength - 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne), nameof(Direction.Row), false)]
        [TestCase(0, TestTools.InitColumnLength - 1, nameof(TestDoubleEnumerableInstanceType.Empty),
            nameof(Direction.Row), false)]
        [TestCase(0, TestTools.InitColumnLength - 1, nameof(TestDoubleEnumerableInstanceType.EmptyRows),
            nameof(Direction.Row), false)]
        [TestCase(0, TestTools.InitColumnLength, nameof(TestDoubleEnumerableInstanceType.Empty), nameof(Direction.Row),
            true)]
        [TestCase(TestTools.InitRowLength - 1, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic),
            nameof(Direction.Row), false)]
        [TestCase(TestTools.InitRowLength - 1, 0, nameof(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic),
            nameof(Direction.None), true)]
        [TestCase(TestTools.InitRowLength - 1, 0, nameof(TestDoubleEnumerableInstanceType.Empty), nameof(Direction.Row),
            false)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 2,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo), nameof(Direction.Row), false)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 2,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo), nameof(Direction.Column), true)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 2,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne), nameof(Direction.Row), true)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 2,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne), nameof(Direction.Column), false)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), nameof(Direction.Row), false)]
        [TestCase(TestTools.InitRowLength - 1, TestTools.InitColumnLength - 1,
            nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne), nameof(Direction.Column), false)]
        [TestCase(TestTools.InitRowLength, 0, nameof(TestDoubleEnumerableInstanceType.Empty), nameof(Direction.Row),
            true)]
        public static void SetRange_Wide_Test(int row, int column,
            string setItemType, string directionType, bool isError)
        {
            var direction = TestTools.TestDirectionFrom(directionType);
            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var setItems = TestTools.MakeTestRecordList(setItemType, false, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.SetRange(row, column, setItems, direction);
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

        [TestCase(true, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.NotNull_Short), false)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.NotNull_Long), false)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.Empty), false)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.Empty), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.Null), true)]
        public static void AddTest(bool targetIsEmpty, string addItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var addItems = TestTools.MakeTestRecords(addItemType, false, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.Add(addItems);
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
        public static void AddRangeTest(bool targetIsEmpty, string addItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var addItems = TestTools.MakeTestRecordList(addItemType, false, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.AddRange(addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.NotNull_Short), false)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.NotNull_Long), false)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.Empty), false)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(true, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.Empty), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(false, nameof(TestSingleEnumerableInstanceType.Null), true)]
        public static void AddItemTest(bool targetIsEmpty, string addItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var addItems = TestTools.MakeTestRecords(addItemType, true, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.AddItem(addItems);
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
        public static void AddItemRangeTest(bool targetIsEmpty, string addItemType, bool isError)
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
                instance.AddItemRange(addItems);
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

        [TestCase(true, -1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        [TestCase(true, -1, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(true, -1, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Short), false)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Long), false)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.Empty), false)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(true, 1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        [TestCase(true, 1, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(true, 1, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(false, -1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.Empty), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestSingleEnumerableInstanceType.Empty), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(false, TestTools.InitRowLength, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitRowLength + 1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        public static void InsertTest(bool targetIsEmpty, int index, string insertItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var insertItems = TestTools.MakeTestRecords(insertItemType, false, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.Insert(index, insertItems);
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
        public static void InsertRangeTest(bool targetIsEmpty, int index, string insertItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var insertItems = TestTools.MakeTestRecordList(insertItemType, false, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.InsertRange(index, insertItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, -1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        [TestCase(true, -1, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(true, -1, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Short), false)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Long), false)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.Empty), false)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(true, 0, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(true, 1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        [TestCase(true, 1, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(true, 1, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(false, -1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.Empty), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(false, 0, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), false)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestSingleEnumerableInstanceType.NotNull_Short), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestSingleEnumerableInstanceType.NotNull_Long), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestSingleEnumerableInstanceType.Empty), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestSingleEnumerableInstanceType.HasNullItem), true)]
        [TestCase(false, TestTools.InitColumnLength, nameof(TestSingleEnumerableInstanceType.Null), true)]
        [TestCase(false, TestTools.InitColumnLength + 1, nameof(TestSingleEnumerableInstanceType.NotNull_Basic), true)]
        public static void InsertItemTest(bool targetIsEmpty, int index, string insertItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var insertItems = TestTools.MakeTestRecords(insertItemType, true, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.InsertItem(index, insertItems);
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
        public static void InsertItemRangeTest(bool targetIsEmpty, int index, string insertItemType, bool isError)
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
                instance.InsertItemRange(index, insertItems);
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
        public static void OverwriteTest(bool targetIsEmpty, int index, string overwriteItemType, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var overwriteItems = TestTools.MakeTestRecordList(overwriteItemType, false, TestTools.MakeInsertItem);
            var errorOccured = false;
            try
            {
                instance.Overwrite(index, overwriteItems);
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
        public static void OverwriteItemTest(bool targetIsEmpty, int index, string overwriteItemType, bool isError)
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
                instance.OverwriteItem(index, overwriteItems);
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

        [TestCase(true, -1, -1, true)]
        [TestCase(true, 0, -1, true)]
        [TestCase(true, -1, 0, true)]
        [TestCase(true, 0, 0, true)]
        [TestCase(false, -1, -1, true)]
        [TestCase(false, -1, 0, true)]
        [TestCase(false, -1, TestTools.InitRowLength - 1, true)]
        [TestCase(false, -1, TestTools.InitRowLength, true)]
        [TestCase(false, 0, -1, true)]
        [TestCase(false, 0, 0, false)]
        [TestCase(false, 0, TestTools.InitRowLength - 1, false)]
        [TestCase(false, 0, TestTools.InitRowLength, true)]
        [TestCase(false, TestTools.InitRowLength - 1, -1, true)]
        [TestCase(false, TestTools.InitRowLength - 1, 0, false)]
        [TestCase(false, TestTools.InitRowLength - 1, TestTools.InitRowLength - 1, false)]
        [TestCase(false, TestTools.InitRowLength - 1, TestTools.InitRowLength, true)]
        [TestCase(false, TestTools.InitRowLength, -1, true)]
        [TestCase(false, TestTools.InitRowLength, 0, true)]
        [TestCase(false, TestTools.InitRowLength, TestTools.InitRowLength - 1, true)]
        [TestCase(false, TestTools.InitRowLength, TestTools.InitRowLength, true)]
        public static void MoveTest(bool targetIsEmpty, int oldIndex, int newIndex, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                instance.Move(oldIndex, newIndex);
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
        public static void MoveRangeTest(bool targetIsEmpty, int oldIndex, int newIndex,
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
                instance.MoveRange(oldIndex, newIndex, count);
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
        [TestCase(true, 0, -1, true)]
        [TestCase(true, -1, 0, true)]
        [TestCase(true, 0, 0, true)]
        [TestCase(false, -1, -1, true)]
        [TestCase(false, -1, 0, true)]
        [TestCase(false, -1, TestTools.InitColumnLength - 1, true)]
        [TestCase(false, -1, TestTools.InitColumnLength, true)]
        [TestCase(false, 0, -1, true)]
        [TestCase(false, 0, 0, false)]
        [TestCase(false, 0, TestTools.InitColumnLength - 1, false)]
        [TestCase(false, 0, TestTools.InitColumnLength, true)]
        [TestCase(false, TestTools.InitColumnLength - 1, -1, true)]
        [TestCase(false, TestTools.InitColumnLength - 1, 0, false)]
        [TestCase(false, TestTools.InitColumnLength - 1, TestTools.InitColumnLength - 1, false)]
        [TestCase(false, TestTools.InitColumnLength - 1, TestTools.InitColumnLength, true)]
        [TestCase(false, TestTools.InitColumnLength, -1, true)]
        [TestCase(false, TestTools.InitColumnLength, 0, true)]
        [TestCase(false, TestTools.InitColumnLength, TestTools.InitColumnLength - 1, true)]
        [TestCase(false, TestTools.InitColumnLength, TestTools.InitColumnLength, true)]
        public static void MoveItemTest(bool targetIsEmpty, int oldIndex, int newIndex, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);

            var errorOccured = false;
            try
            {
                instance.MoveItem(oldIndex, newIndex);
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
        public static void MoveItemRangeTest(bool targetIsEmpty, int oldIndex, int newIndex, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var errorOccured = false;
            try
            {
                instance.MoveItemRange(oldIndex, newIndex, count);
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

        [TestCase(-1, -1, false)]
        [TestCase(1, TestTools.InitColumnLength - 2, false)]
        [TestCase(2, TestTools.InitColumnLength, true)]
        public static void RemoveTest(int itemRow, int itemColumnLength, bool isRemoved)
        {
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                funcMakeDefaultItem);
            var removeItems = itemRow >= 0
                ? Enumerable.Range(0, itemColumnLength).Select(c => funcMakeDefaultItem(itemRow, c))
                : null;
            var comparer = EqualityComparerFactory.Create<TestRecord>();

            var result = false;
            try
            {
                result = instance.Remove(removeItems, comparer);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 結果が意図した値であること
            Assert.AreEqual(isRemoved, result);
        }

        [TestCase(true, -1, true)]
        [TestCase(true, 0, true)]
        [TestCase(false, -1, true)]
        [TestCase(false, 0, false)]
        [TestCase(false, TestTools.InitRowLength - 1, false)]
        [TestCase(false, TestTools.InitRowLength, true)]
        public static void RemoveAtTest(bool targetIsEmpty, int index, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var errorOccured = false;
            try
            {
                instance.RemoveAt(index);
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
        public static void RemoveRangeTest(bool targetIsEmpty, int index, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var errorOccured = false;
            try
            {
                instance.RemoveRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);
        }

        [TestCase(true, -1, true)]
        [TestCase(true, 0, true)]
        [TestCase(false, -1, true)]
        [TestCase(false, 0, false)]
        [TestCase(false, TestTools.InitColumnLength - 1, false)]
        [TestCase(false, TestTools.InitColumnLength, true)]
        public static void RemoveItemTest(bool targetIsEmpty, int index, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var errorOccured = false;
            try
            {
                instance.RemoveItem(index);
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
        public static void RemoveItemRangeTest(bool targetIsEmpty, int index, int count, bool isError)
        {
            var instance = TestTools.MakeTwoDimensionalList(
                targetIsEmpty
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeInitItem);
            var errorOccured = false;
            try
            {
                instance.RemoveItemRange(index, count);
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
                instance.AdjustLength(rowLength);
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
                instance.AdjustLengthIfShort(rowLength);
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
                instance.AdjustLengthIfLong(rowLength);
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
                instance.AdjustItemLength(columnLength);
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
                instance.AdjustItemLengthIfShort(columnLength);
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
                instance.AdjustItemLengthIfLong(columnLength);
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

            var resetItems = TestTools.MakeTestRecordList(resetItemType, false, TestTools.MakeInsertItem);
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
