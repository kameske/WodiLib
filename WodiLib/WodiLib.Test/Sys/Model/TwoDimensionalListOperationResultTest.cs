using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TwoDimensionalListOperationResultTest
    {
        /*
         * 操作の引数検証クラスが正しく呼ばれること、
         * 各操作メソッドの実行結果が正しいことをテストする。
         *
         * 各メソッドの引数が正しいことを事前条件とする。
         * 引数が誤っている場合の動作や処理前後の通知の正しさは検証しない。
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

        [Test]
        public static void ConstructorTest_Require()
        {
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var initItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                false, funcMakeDefaultItem);

            const int maxRowCapacity = TestTools.InitRowLength + 4;
            const int minRowCapacity = 1;
            const int maxColumnCapacity = TestTools.InitColumnLength + 12;
            const int minColumnCapacity = 3;

            var validatorMock = new TwoDimensionalListValidatorMock();

            TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance = null;

            try
            {
                instance = new TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord>(initItems,
                    TestTools.CreateCommonConfig(funcMakeDefaultItem, validatorMock,
                        maxRowCapacity,
                        minRowCapacity,
                        maxColumnCapacity,
                        minColumnCapacity));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckConstructor(initItems, true);

            // 正しく初期化されていること
            AssertElementsConstructor(instance, initItems);

            // 容量制限が正しいこと
            Assert.AreEqual(maxRowCapacity, instance.GetMaxRowCapacity());
            Assert.AreEqual(minRowCapacity, instance.GetMinRowCapacity());
            Assert.AreEqual(maxColumnCapacity, instance.GetMaxColumnCapacity());
            Assert.AreEqual(minColumnCapacity, instance.GetMinColumnCapacity());
        }

        [Test]
        public static void ConstructorTest_A()
        {
            var validatorMock = new TwoDimensionalListValidatorMock();
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance = null;

            try
            {
                instance = new TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord>(
                    TestTools.InitRowLength, TestTools.InitColumnLength,
                    TestTools.CreateCommonConfig(funcMakeDefaultItem, validatorMock)
                    );
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            var expectedInitItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                false, funcMakeDefaultItem);

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckConstructor(expectedInitItems, false);

            // 正しく初期化されていること
            AssertElementsConstructor(instance, expectedInitItems);
        }

        [Test]
        public static void ConstructorTest_B()
        {
            var validatorMock = new TwoDimensionalListValidatorMock();
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance = null;

            try
            {
                instance = new TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord>(
                    TestTools.CreateCommonConfig(funcMakeDefaultItem, validatorMock));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            var expectedInitItems = Array.Empty<TestRecord[]>();

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckConstructor(expectedInitItems, false);

            // 正しく初期化されていること
            AssertElementsConstructor(instance, expectedInitItems);
        }

        private static void AssertElementsConstructor(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance, TestRecord[][] items)
        {
            var assumedCount = items.Length;
            var assumedItemCount = items.GetInnerArrayLength();
            Assert.IsTrue(instance.RowCount == assumedCount);
            Assert.IsTrue(instance.ColumnCount == assumedItemCount);
            for (var i = 0; i < assumedCount; i++)
            for (var j = 0; j < assumedItemCount; j++)
            {
                Assert.IsTrue(instance[i, j].ItemEquals(items[i][j]),
                    $"instance[{i}, {j}] == items{i}][{j}] ({instance[i, j]}.ItemEquals({items[i][j]}))");
            }
        }

        #endregion

        #region Accessor

        [Test]
        public static void AccessorTest_Item_Get()
        {
            const int rowIndex = 1;
            const int columnIndex = 3;

            TestRecord result = null;

            var instance = MakeInstance(out var validatorMock);

            try
            {
                result = instance[rowIndex, columnIndex];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGetItem(rowIndex, columnIndex);

            // 取得結果が正しいこと
            {
                var expectedItem = TestTools.MakeListDefaultItem(rowIndex, columnIndex);

                Assert.IsTrue(result.ItemEquals(expectedItem),
                    $"result.ItemEquals(expectedItem) ({result}.ItemEquals({expectedItem}))");
            }
        }

        [Test]
        public static void AccessorTest_Item_Set()
        {
            const int rowIndex = 1;
            const int columnIndex = 3;

            var setItem = TestTools.MakeItem(rowIndex, columnIndex, "Set", "Value");

            var instance = MakeInstance(out var validatorMock);

            try
            {
                instance[rowIndex, columnIndex] = setItem;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSetItem(rowIndex, columnIndex, setItem);

            // 取得結果が正しいこと
            Assert.IsTrue(instance[rowIndex, columnIndex].ItemEquals(setItem),
                $"instance[{rowIndex}, {columnIndex}].ItemEquals(expectedItem) ({instance[rowIndex, columnIndex]}.ItemEquals({setItem}))");
        }

        #endregion

        #region Get

        [Test]
        public static void GetRowTest()
        {
            const int index = 1;
            const int count = 3;

            var instance = MakeInstance(out var validatorMock);

            IEnumerable<IEnumerable<TestRecord>> result = null;

            try
            {
                result = instance.GetRow(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGetRow(index, count);

            // 取得結果が正しいこと
            var resultArray = result.ToTwoDimensionalArray();
            AssertElementsGet(instance, index, count, 0, TestTools.InitColumnLength, Direction.Row, resultArray);
        }

        [Test]
        public static void GetColumnTest()
        {
            const int index = 1;
            const int count = 2;

            var instance = MakeInstance(out var validatorMock);

            IEnumerable<IEnumerable<TestRecord>> result = null;

            try
            {
                result = instance.GetColumn(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGetColumn(index, count);

            // 取得結果が正しいこと
            var resultArray = result.ToTwoDimensionalArray();
            AssertElementsGet(instance, 0, TestTools.InitRowLength, index, count, Direction.Column, resultArray);
        }

        [Test]
        public static void GetItemTest()
        {
            const int rowIndex = 1;
            const int rowCount = 2;
            const int columnIndex = 0;
            const int columnCount = 3;

            var instance = MakeInstance(out var validatorMock);

            IEnumerable<IEnumerable<TestRecord>> result = null;

            try
            {
                result = instance.GetItem(rowIndex, rowCount, columnIndex, columnCount);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGetItem(rowIndex, rowCount, columnIndex, columnCount);

            // 取得結果が正しいこと
            var resultArray = result.ToTwoDimensionalArray();
            AssertElementsGet(instance, rowIndex, rowCount, columnIndex, columnCount, Direction.Row, resultArray);
        }

        private static void AssertElementsGet(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            int row, int count, int column, int itemCount, Direction direction,
            TestRecord[][] resultArray)
        {
            if (direction == Direction.Row)
            {
                Assert.AreEqual(count, resultArray.Length);
                Assert.AreEqual(itemCount, resultArray.GetInnerArrayLength());
                for (var rOffset = 0; rOffset < count; rOffset++)
                for (var cOffset = 0; cOffset < itemCount; cOffset++)
                {
                    Assert.IsTrue(instance[rOffset + row, cOffset + column].ItemEquals(resultArray[rOffset][cOffset]));
                }
            }
            else // direction == Direction.Column
            {
                Assert.AreEqual(itemCount, resultArray.Length);
                Assert.AreEqual(count, resultArray.GetInnerArrayLength());
                for (var cOffset = 0; cOffset < itemCount; cOffset++)
                for (var rOffset = 0; rOffset < count; rOffset++)
                {
                    Assert.IsTrue(instance[rOffset + row, cOffset + column].ItemEquals(resultArray[cOffset][rOffset]));
                }
            }
        }

        [Test]
        public static void GetEnumeratorTest()
        {
            var instance = MakeInstance(out var validatorMock);

            try
            {
                _ = instance.GetEnumerator();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が呼ばれていないこと
            validatorMock.CheckNotWork();

            // foreach で結果が正しく取得されること
            var r = 0;
            foreach (var line in instance)
            {
                var lineArray = line.ToArray();
                Assert.AreEqual(TestTools.InitColumnLength, lineArray.Length);
                for (var c = 0; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(lineArray[c]));
                }

                r++;
            }
        }

        #endregion

        #region Set

        [Test]
        public static void SetRowTest()
        {
            const int row = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            var setItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.SetRow(row, TestTools.MakeRowList(setItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSetRow(row, TestTools.ConvertIEnumerableArray(setItems));

            // 編集結果が正しいこと
            AssertElementsSetRow(row, 0, setItems, Direction.Row, instance, oldItems);
        }

        [Test]
        public static void SetColumnTest()
        {
            const int index = 1;

            var setItems = TestTools.MakeTestRecordArrays(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                true, TestTools.MakeInitItem);

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.SetColumn(index, TestTools.ConvertIEnumerableArray(setItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSetColumn(index, TestTools.ConvertIEnumerableArray(setItems));

            // 編集結果が正しいこと
            AssertElementsSetRow(0, index, setItems, Direction.Column, instance, oldItems);
        }

        private static void AssertElementsSetRow(
            int row, int column, TestRecord[][] setItems, Direction setDirection,
            TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance, TestRecord[][] oldItems)
        {
            Assert.AreEqual(TestTools.InitRowLength, instance.RowCount);
            Assert.AreEqual(TestTools.InitColumnLength, instance.ColumnCount);

            var setItemRowLength =
                setDirection == Direction.Row
                    ? setItems.Length
                    : setItems.GetInnerArrayLength();
            var setItemColumnLength =
                setDirection == Direction.Row
                    ? setItems.GetInnerArrayLength()
                    : setItems.Length;

            var r = 0;
            // 編集行より前：要素が変化していないこと
            for (; r < row; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }

            // 編集行：編集した列が編集した内容に変化していること
            for (var rOffset = 0; rOffset < setItemRowLength; rOffset++)
            {
                // 編集列より前：要素が変化していないこと
                var c = 0;
                for (; c < column; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                for (var cOffset = 0; cOffset < setItemColumnLength; cOffset++)
                {
                    // 編集列：編集した内容に変化していること
                    if (setDirection == Direction.Row)
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(setItems[rOffset][cOffset]),
                            $"instance[{r}, {c}].ItemEquals(setItems[{rOffset}][{cOffset}]) ({instance[r, c]}.ItemEquals({setItems[rOffset][cOffset]}))");
                    }
                    else
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(setItems[cOffset][rOffset]),
                            $"instance[{r}, {c}].ItemEquals(setItems[{rOffset}][{cOffset}]) ({instance[r, c]}.ItemEquals({setItems[cOffset][rOffset]}))");
                    }

                    c++;
                }

                // 編集列より後：要素が変化していないこと
                for (; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                r++;
            }

            // 編集行より後：要素が変化していないこと
            for (; r < TestTools.InitRowLength; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }
        }

        #endregion

        #region Add

        [Test]
        public static void AddRowTest()
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArrays(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.AddRow(TestTools.MakeRowList(addItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsertRow(TestTools.InitRowLength, TestTools.ConvertIEnumerableArray(addItems));

            // 挿入結果が正しいこと
            AssertElementsAddRow(instance, oldItems, addItems);
        }

        private static void AssertElementsAddRow(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            TestRecord[][] oldItems, TestRecord[][] addItems)
        {
            var addRowLength = addItems.Length;

            Assert.IsTrue(instance.RowCount == TestTools.InitRowLength + addRowLength);
            Assert.IsTrue(instance.ColumnCount == TestTools.InitColumnLength);

            var r = 0;

            // 追加行より前：値が変化していないこと
            for (; r < oldItems.Length; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }

            // 追加行：追加した値と一致すること
            var rOffset = 0;
            for (; rOffset < addRowLength; rOffset++)
            {
                for (var c = 0; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(addItems[rOffset][c]),
                        $"instance[{r}, {c}].ItemEquals(addItems[{rOffset}][{c}]) ({instance[r, c]}.ItemEquals({addItems[rOffset][c]}))");
                }

                r++;
            }
        }

        [Test]
        public static void AddColumnTest()
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArrays(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                true, TestTools.MakeInitItem);

            try
            {
                instance.AddColumn(TestTools.ConvertIEnumerableArray(addItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsertColumn(TestTools.InitColumnLength, TestTools.ConvertIEnumerableArray(addItems));

            // 挿入結果が正しいこと
            AssertElementsAddColumn(instance, oldItems, addItems);
        }

        private static void AssertElementsAddColumn(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            TestRecord[][] oldItems, TestRecord[][] addItems)
        {
            var addColumnLength = addItems.Length;

            Assert.IsTrue(instance.RowCount == TestTools.InitRowLength);
            Assert.IsTrue(instance.ColumnCount == TestTools.InitColumnLength + addColumnLength);

            for (var r = 0; r < TestTools.InitRowLength; r++)
            {
                var c = 0;
                // 追加列より前：値が変化していないこと
                for (; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                // 追加行：追加した値と一致すること
                var cOffset = 0;
                for (; cOffset < addColumnLength; cOffset++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(addItems[cOffset][r]),
                        $"instance[{r}, {c}].ItemEquals(addItems[{cOffset}][{r}]) ({instance[r, c]}.ItemEquals({addItems[cOffset][r]}))");
                    c++;
                }
            }
        }

        #endregion

        #region Insert

        [Test]
        public static void InsertRowTest()
        {
            const int index = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.InsertRow(index, TestTools.MakeRowList(addItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsertRow(index, TestTools.ConvertIEnumerableArray(addItems));

            // 挿入結果が正しいこと
            AssertElementsInsertRow(index, instance, oldItems, addItems);
        }

        private static void AssertElementsInsertRow(int index, TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            TestRecord[][] oldItems, TestRecord[][] addItems)
        {
            var addRowLength = addItems.Length;

            Assert.IsTrue(instance.RowCount == TestTools.InitRowLength + addRowLength);
            Assert.IsTrue(instance.ColumnCount == TestTools.InitColumnLength);

            var r = 0;

            // 挿入行より前：値が変化していないこと
            for (; r < index; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }

            // 挿入行：挿入した値と一致すること
            var rOffset = 0;
            for (; rOffset < addRowLength; rOffset++)
            {
                for (var c = 0; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(addItems[rOffset][c]),
                        $"instance[{r}, {c}].ItemEquals(addItems[{rOffset}][{c}]) ({instance[r, c]}.ItemEquals({addItems[rOffset][c]}))");
                }

                r++;
            }

            // 挿入列より後：値が変化していないこと
            for (; r < TestTools.InitRowLength + addRowLength; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r - rOffset][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r - rOffset][c]}))");
            }
        }

        [Test]
        public static void InsertColumnTest()
        {
            const int index = 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArrays(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                true, TestTools.MakeInitItem);

            try
            {
                instance.InsertColumn(index, TestTools.ConvertIEnumerableArray(addItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsertColumn(index, TestTools.ConvertIEnumerableArray(addItems));

            // 挿入結果が正しいこと
            AssertElementsInsertColumn(index, instance, oldItems, addItems);
        }

        private static void AssertElementsInsertColumn(int index, TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            TestRecord[][] oldItems, TestRecord[][] addItems)
        {
            var addColumnLength = addItems.Length;

            Assert.IsTrue(instance.RowCount == TestTools.InitRowLength);
            Assert.IsTrue(instance.ColumnCount == TestTools.InitColumnLength + addColumnLength);

            for (var r = 0; r < TestTools.InitRowLength; r++)
            {
                var c = 0;
                // 挿入列より前：値が変化していないこと
                for (; c < index; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                // 挿入行：挿入した値と一致すること
                var cOffset = 0;
                for (; cOffset < addColumnLength; cOffset++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(addItems[cOffset][r]),
                        $"instance[{r}, {c}].ItemEquals(addItems[{cOffset}][{r}]) ({instance[r, c]}.ItemEquals({addItems[cOffset][r]}))");
                    c++;
                }

                // 挿入列より後：値が変化していないこと
                for (; c < TestTools.InitColumnLength + addColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c - cOffset]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c - cOffset]}))");
                }
            }
        }

        #endregion

        #region Overwrite

        private const string OverwriteTestCase_ReplaceOnly = "ReplaceOnly";
        private const string OverwriteTestCase_ReplaceAdd = "ReplaceAdd";
        private const string OverwriteTestCase_AddOnly = "AddOnly";

        [TestCase(OverwriteTestCase_ReplaceOnly)]
        [TestCase(OverwriteTestCase_ReplaceAdd)]
        [TestCase(OverwriteTestCase_AddOnly)]
        public static void OverwriteTest(string testType)
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var overwriteItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                false, TestTools.MakeInitItem);

            var addRowLength = overwriteItems.Length;

            var index = testType switch
            {
                OverwriteTestCase_ReplaceOnly => TestTools.InitRowLength - addRowLength,
                OverwriteTestCase_ReplaceAdd => TestTools.InitRowLength - addRowLength / 2,
                OverwriteTestCase_AddOnly => TestTools.InitRowLength,
                _ => throw new Exception()
            };

            var answerAfterRowLength =
                TestTools.InitRowLength + Math.Max(index - TestTools.InitRowLength + addRowLength, 0);

            try
            {
                instance.OverwriteRow(index, TestTools.MakeRowList(overwriteItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckOverwriteRow(index, TestTools.ConvertIEnumerableArray(overwriteItems));

            // 上書き結果が正しいこと
            {
                Assert.AreEqual(instance.RowCount, answerAfterRowLength);
                Assert.AreEqual(instance.ColumnCount, TestTools.InitColumnLength);

                var r = 0;

                // 上書き行より前：値が変化していないこと
                for (; r < index; r++)
                for (var c = 0; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                // 上書き行：上書きした値と一致すること
                for (var rOffset = 0; rOffset < addRowLength; rOffset++)
                {
                    for (var c = 0; c < TestTools.InitColumnLength; c++)
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(overwriteItems[rOffset][c]),
                            $"instance[{r}, {c}].ItemEquals(overwriteItems[{rOffset}][{c}]) ({instance[r, c]}.ItemEquals({overwriteItems[rOffset][c]}))");
                    }

                    r++;
                }
            }
        }

        [TestCase(OverwriteTestCase_ReplaceOnly)]
        [TestCase(OverwriteTestCase_ReplaceAdd)]
        [TestCase(OverwriteTestCase_AddOnly)]
        public static void OverwriteColumnTest(string testType)
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var overwriteItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                true, TestTools.MakeInitItem);

            var addColumnLength = overwriteItems.Length;

            var index = testType switch
            {
                OverwriteTestCase_ReplaceOnly => TestTools.InitColumnLength - addColumnLength,
                OverwriteTestCase_ReplaceAdd => TestTools.InitColumnLength - addColumnLength / 2,
                OverwriteTestCase_AddOnly => TestTools.InitColumnLength,
                _ => throw new Exception()
            };

            var answerAfterColumnLength =
                TestTools.InitColumnLength + Math.Max(index - TestTools.InitColumnLength + addColumnLength, 0);

            try
            {
                instance.OverwriteColumn(index, TestTools.ConvertIEnumerableArray(overwriteItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckOverwriteColumn(index, TestTools.ConvertIEnumerableArray(overwriteItems));

            // 上書き結果が正しいこと
            {
                Assert.IsTrue(instance.RowCount == TestTools.InitRowLength);
                Assert.IsTrue(instance.ColumnCount == answerAfterColumnLength);

                for (var r = 0; r < TestTools.InitRowLength; r++)
                {
                    var c = 0;

                    // 上書き列より前：値が変化していないこと
                    for (; c < index; c++)
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                            $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                    }

                    // 上書き列：上書きした値と一致すること
                    for (var cOffset = 0; cOffset < addColumnLength; cOffset++)
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(overwriteItems[cOffset][r]),
                            $"instance[{r}, {c}].ItemEquals(overwriteItems[{cOffset}][{r}]) ({instance[r, c]}.ItemEquals({overwriteItems[cOffset][r]}))");
                        c++;
                    }
                }
            }
        }

        #endregion

        #region Move

        private const string MoveTestCase_OldGreaterThanNew = "OldIndex > NewIndex";
        private const string MoveTestCase_OldEqualsNew = "OldIndex == NewIndex";
        private const string MoveTestCase_OldLessThanNew = "OldIndex < NewIndex";

        [TestCase(MoveTestCase_OldGreaterThanNew)]
        [TestCase(MoveTestCase_OldEqualsNew)]
        [TestCase(MoveTestCase_OldLessThanNew)]
        public static void MoveRowTest(string testType)
        {
            var (oldIndex, newIndex) = testType switch
            {
                MoveTestCase_OldGreaterThanNew => (TestTools.InitRowLength / 2 - 1, 0),
                MoveTestCase_OldEqualsNew => (TestTools.InitRowLength / 2 - 1, TestTools.InitRowLength / 2 - 1),
                MoveTestCase_OldLessThanNew => (0, TestTools.InitRowLength / 2 - 1),
                _ => throw new Exception(),
            };
            const int count = TestTools.InitRowLength / 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.MoveRow(oldIndex, newIndex, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckMoveRow(oldIndex, newIndex, count);

            // 移動結果が正しいこと
            AssertElementsMove(instance, oldIndex, newIndex, count, Direction.Row, oldItems);
        }

        [TestCase(MoveTestCase_OldGreaterThanNew)]
        [TestCase(MoveTestCase_OldEqualsNew)]
        [TestCase(MoveTestCase_OldLessThanNew)]
        public static void MoveColumnTest(string testType)
        {
            var (oldIndex, newIndex) = testType switch
            {
                MoveTestCase_OldGreaterThanNew => (TestTools.InitRowLength - 1, 1),
                MoveTestCase_OldEqualsNew => (TestTools.InitRowLength / 2, TestTools.InitRowLength / 2),
                MoveTestCase_OldLessThanNew => (1, TestTools.InitRowLength - 1),
                _ => throw new Exception(),
            };
            const int count = TestTools.InitRowLength / 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.MoveColumn(oldIndex, newIndex, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckMoveColumn(oldIndex, newIndex, count);

            // 移動結果が正しいこと
            AssertElementsMove(instance, oldIndex, newIndex, count, Direction.Column, oldItems);
        }

        private static void AssertElementsMove(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            int oldIndex, int newIndex, int count, Direction direction, TestRecord[][] oldItems)
        {
            Assert.IsTrue(instance.RowCount == TestTools.InitRowLength);
            Assert.IsTrue(instance.ColumnCount == TestTools.InitColumnLength);

            // 移動前後のインデックスMap
            var outer = Enumerable.Range(0, TestTools.InitRowLength).ToList();
            if (direction == Direction.Row)
            {
                var moveItems = outer.GetRange(oldIndex, count);
                outer.RemoveRange(oldIndex, count);
                outer.InsertRange(newIndex, moveItems);
            }

            var inner = Enumerable.Range(0, TestTools.InitColumnLength).ToList();
            if (direction == Direction.Column)
            {
                var moveItems = inner.GetRange(oldIndex, count);
                inner.RemoveRange(oldIndex, count);
                inner.InsertRange(newIndex, moveItems);
            }

            outer.ForEach((beforeRow, afterRow) =>
                inner.ForEach((beforeColumn, afterColumn) =>
                {
                    Assert.IsTrue(instance[afterRow, afterColumn].ItemEquals(oldItems[beforeRow][beforeColumn]),
                        $"instance[{afterRow}, {afterColumn}].ItemEquals(oldItems[{beforeRow}][{beforeColumn}]) " +
                        $"({instance[afterRow, afterColumn]}.ItemEquals({oldItems[beforeRow][beforeColumn]}))");
                })
            );
        }

        #endregion

        #region Remove

        [TestCase(0)]
        [TestCase(2)]
        public static void RemoveRowTest(int removeCount)
        {
            const int index = 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.RemoveRow(index, removeCount);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckRemoveRow(index, removeCount);

            // 除去結果が正しいこと
            AssertElementsRemoveRow(instance, index, removeCount, oldItems);
        }

        private static void AssertElementsRemoveRow(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            int index, int count, TestRecord[][] oldItems)
        {
            var answerRowLength = TestTools.InitRowLength - count;

            Assert.IsTrue(instance.RowCount == answerRowLength);
            Assert.IsTrue(instance.ColumnCount == TestTools.InitColumnLength);

            // 初期要素（削除位置より前）が変化していないこと
            var r = 0;
            for (; r < index; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }

            // 初期要素（削除位置より後）が変化していないこと
            for (; r < answerRowLength; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r + count][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r + count}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r + count][c]}))");
            }
        }

        [TestCase(0)]
        [TestCase(2)]
        public static void RemoveColumnTest(int removeCount)
        {
            const int index = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.RemoveColumn(index, removeCount);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckRemoveColumn(index, removeCount);

            // 移動結果が正しいこと
            AssertElementsRemoveColumn(instance, index, removeCount, oldItems);
        }

        private static void AssertElementsRemoveColumn(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            int index, int count, TestRecord[][] oldItems)
        {
            var answerColumnLength = TestTools.InitColumnLength - count;

            Assert.IsTrue(instance.RowCount == TestTools.InitRowLength);
            Assert.IsTrue(instance.ColumnCount == answerColumnLength);

            for (var r = 0; r < TestTools.InitRowLength; r++)
            {
                var c = 0;

                // 初期要素（削除位置より前）が変化していないこと
                for (; c < index; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                // 初期要素（削除位置より後）が変化していないこと
                for (; c < answerColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c + count]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c + count}]) ({instance[r, c]}.ItemEquals({oldItems[r][c + count]}))");
                }
            }
        }

        #endregion

        #region AdjustLength

        private const string AdjustLengthTestCase_LongLength = "LongLength";
        private const string AdjustLengthTestCase_SameLength = "SameLength";
        private const string AdjustLengthTestCase_ShortLength = "ShortLength";

        #region Both

        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_ShortLength)]
        public static void AdjustLengthTest(string rowLengthType, string columnLengthType)
        {
            var rowLength = rowLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };
            var columnLength = columnLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLength(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, rowLength, columnLength, oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_ShortLength)]
        public static void AdjustLengthIfShortTest(string rowLengthType, string columnLengthType)
        {
            var rowLength = rowLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };
            var columnLength = columnLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLengthIfShort(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, Math.Max(TestTools.InitRowLength, rowLength),
                Math.Max(TestTools.InitColumnLength, columnLength), oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_ShortLength)]
        public static void AdjustLengthIfLongTest(string rowLengthType, string columnLengthType)
        {
            var rowLength = rowLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };
            var columnLength = columnLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLengthIfLong(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, Math.Min(TestTools.InitRowLength, rowLength),
                Math.Min(TestTools.InitColumnLength, columnLength), oldItems, funcMakeDefaultItem);
        }

        #endregion

        #region Row

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustRowLengthTest(string lengthType)
        {
            var rowLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustRowLength(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, TestTools.InitColumnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, rowLength, TestTools.InitColumnLength, oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustRowLengthIfShortTest(string lengthType)
        {
            var rowLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustRowLengthIfShort(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, TestTools.InitColumnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, Math.Max(TestTools.InitRowLength, rowLength),
                TestTools.InitColumnLength, oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustRowLengthIfLongTest(string lengthType)
        {
            var rowLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustRowLengthIfLong(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, TestTools.InitColumnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, Math.Min(TestTools.InitRowLength, rowLength),
                TestTools.InitColumnLength, oldItems, funcMakeDefaultItem);
        }

        #endregion

        #region Column

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustColumnLengthTest(string lengthType)
        {
            var columnLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitColumnLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitColumnLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitColumnLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustColumnLength(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(TestTools.InitRowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, TestTools.InitRowLength, columnLength, oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustColumnLengthIfShortTest(string lengthType)
        {
            var columnLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitColumnLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitColumnLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitColumnLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustColumnLengthIfShort(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(TestTools.InitRowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, TestTools.InitRowLength,
                Math.Max(TestTools.InitColumnLength, columnLength), oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustColumnLengthIfLongTest(string lengthType)
        {
            var columnLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitColumnLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitColumnLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitColumnLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(funcMakeDefaultItem, out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustColumnLengthIfLong(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(TestTools.InitRowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, TestTools.InitRowLength,
                Math.Min(TestTools.InitColumnLength, columnLength), oldItems, funcMakeDefaultItem);
        }

        #endregion

        private static void AssertElementsAdjustLength(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance, int adjustRowLength,
            int adjustColumnLength, TestRecord[][] oldItems, Func<int, int, TestRecord> funcMakeDefaultItem)
        {
            var oldRowLength = oldItems.Length;
            var oldColumnLength = oldItems.GetInnerArrayLength();

            var notChangedRowLength = Math.Min(adjustRowLength, oldRowLength);
            var notChangedColumnLength = Math.Min(adjustColumnLength, oldColumnLength);

            var isRowAdded = adjustRowLength > oldRowLength;
            var isColumnAdded = adjustColumnLength > oldColumnLength;

            Assert.IsTrue(instance.RowCount == adjustRowLength);
            Assert.IsTrue(instance.ColumnCount == adjustColumnLength);

            var r = 0;

            for (; r < notChangedRowLength; r++)
            {
                var c = 0;

                // 初期要素（調整位置より前）が変化していないこと
                for (; c < notChangedColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                if (isColumnAdded)
                {
                    // 列に対して「追加」の場合、追加要素が正しいこと
                    for (; c < oldColumnLength; c++)
                    {
                        var answerItem = funcMakeDefaultItem(r, c);

                        Assert.IsTrue(instance[r, c].ItemEquals(answerItem),
                            $"instance[{r}, {c}].ItemEquals(answerItem) ({instance[r, c]}.ItemEquals({answerItem}))");
                    }
                }
                else
                {
                    // 列に対して「追加」以外の場合、すべての列要素に対して探索完了していること
                    Assert.AreEqual(notChangedColumnLength, c);
                }
            }

            if (isRowAdded)
            {
                // 行に対して「追加」の場合、追加要素が正しいこと
                for (var c = 0; c < adjustColumnLength; c++)
                {
                    var answerItem = funcMakeDefaultItem(r, c);

                    Assert.IsTrue(instance[r, c].ItemEquals(answerItem),
                        $"instance[{r}, {c}] == defaultItem({r}][{c}) ({instance[r, c]} == {answerItem})");
                }
            }
            else
            {
                // 行に対して「追加」以外の場合、すべての列要素に対して探索完了していること
                Assert.AreEqual(notChangedRowLength, r);
            }
        }

        #endregion

        #region Reset

        [Test]
        public static void ResetTest()
        {
            var instance = MakeInstance(out var validatorMock);

            var resetItems = TestTools.MakeTestRecordList(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                    false, TestTools.MakeInsertItem)
                .ToTwoDimensionalArray();

            try
            {
                instance.Reset(TestTools.MakeRowList(resetItems));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckReset(resetItems);

            // 結果が正しいこと
            AssertElementsReset(instance, resetItems);
        }

        [Test]
        public static void ClearTest()
        {
            var instance = MakeInstance(out var validatorMock);

            // 最小行数、列数ともに0であるため、期待される結果は0行0列
            var expectedResetItems = Array.Empty<TestRecord[]>();

            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が呼ばれていないこと
            validatorMock.CheckNotWork();

            // 結果が正しいこと
            AssertElementsReset(instance, expectedResetItems);
        }

        private static void AssertElementsReset(TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> instance,
            TestRecord[][] resetItems)
        {
            var resetItemRowLength = resetItems.Length;
            var resetItemColumnLength = resetItems.GetInnerArrayLength();

            Assert.IsTrue(instance.RowCount == resetItemRowLength);
            Assert.IsTrue(instance.ColumnCount == resetItemColumnLength);

            // すべての要素が初期化要素に置換されていること
            for (var r = 0; r < resetItemRowLength; r++)
            for (var c = 0; c < resetItemColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(resetItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(resetItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({resetItems[r][c]}))");
            }
        }

        #endregion

        #region TestTools


        private static TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> MakeInstance(
            out TwoDimensionalListValidatorMock validatorMock)
            => MakeInstance(null, out validatorMock);

        private static TwoDimensionalList<IFixedLengthList<TestRecord>, TestRecordList, TestRecord> MakeInstance(
            Func<int, int, TestRecord> funcMakeDefaultItem,
            out TwoDimensionalListValidatorMock validatorMock)
        {
            validatorMock = new TwoDimensionalListValidatorMock(false);

            var result = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                validatorMock, funcMakeDefaultItem ?? TestTools.MakeListDefaultItem);

            return result;
        }

        #endregion

        #region TestClass

        private class TwoDimensionalListValidatorMock : ITwoDimensionalListValidator<IFixedLengthList<TestRecord>, TestRecord>
        {
            private List<string> CalledMethodNames { get; } = new();
            private TestRecord[][] Items { get; set; }
            private TestRecord[][] InitItems { get; set; }
            private int? Index { get; set; }
            private int? Row { get; set; }
            private int? Column { get; set; }
            private int? RowCount { get; set; }
            private int? ColumnCount { get; set; }
            private int? Count { get; set; }
            private int? OldIndex { get; set; }
            private int? NewIndex { get; set; }
            private int? RowLength { get; set; }
            private int? ColumnLength { get; set; }

            private bool IsRecordConstructor { get; }

            public TwoDimensionalListValidatorMock(bool isRecordConstructor = true)
            {
                IsRecordConstructor = isRecordConstructor;
            }

            #region ValidateHandler

            public void Constructor(IFixedLengthList<TestRecord>[] initItems)
            {
                if (!IsRecordConstructor) return;

                CalledMethodNames.Add(nameof(Constructor));
                InitItems = initItems.ToTwoDimensionalArray();
            }

            public void GetRow(int row, int rowCount)
            {
                CalledMethodNames.Add(nameof(GetRow));
                Row = row;
                RowCount = rowCount;
            }

            public void GetColumn(int columnIndex, int columnCount)
            {
                CalledMethodNames.Add(nameof(GetColumn));
                Column = columnIndex;
                ColumnCount = columnCount;
            }

            public void GetItem(int rowIndex, int columnIndex)
            {
                CalledMethodNames.Add(nameof(GetItem));
                Row = rowIndex;
                Column = columnIndex;
            }

            public void GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount)
            {
                CalledMethodNames.Add(nameof(GetItem));
                Row = rowIndex;
                RowCount = rowCount;
                Column = columnIndex;
                ColumnCount = columnCount;
            }

            public void SetRow(int rowIndex, params IFixedLengthList<TestRecord>[] rows)
            {
                CalledMethodNames.Add(nameof(SetRow));
                Row = rowIndex;
                Items = rows.ToTwoDimensionalArray();
            }

            public void SetColumn(int columnIndex, params IEnumerable<TestRecord>[] items)
            {
                CalledMethodNames.Add(nameof(SetColumn));
                Column = columnIndex;
                Items = items.ToTwoDimensionalArray();
            }

            public void SetItem(int rowIndex, int columnIndex, TestRecord item)
            {
                CalledMethodNames.Add(nameof(SetItem));
                Row = rowIndex;
                Column = columnIndex;
                Items = new[] {new[] {item}};
            }

            public void InsertRow(int rowIndex, params IFixedLengthList<TestRecord>[] items)
            {
                CalledMethodNames.Add(nameof(InsertRow));
                Index = rowIndex;
                Items = items.ToTwoDimensionalArray();
            }

            public void InsertColumn(int columnIndex, params IEnumerable<TestRecord>[] items)
            {
                CalledMethodNames.Add(nameof(InsertColumn));
                Index = columnIndex;
                Items = items.ToTwoDimensionalArray();
            }

            public void OverwriteRow(int rowIndex, params IFixedLengthList<TestRecord>[] items)
            {
                CalledMethodNames.Add(nameof(OverwriteRow));
                Index = rowIndex;
                Items = items.ToTwoDimensionalArray();
            }

            public void OverwriteColumn(int columnIndex, params IEnumerable<TestRecord>[] items)
            {
                CalledMethodNames.Add(nameof(OverwriteColumn));
                Index = columnIndex;
                Items = items.ToTwoDimensionalArray();
            }

            public void MoveRow(int oldRowIndex, int newRowIndex, int count)
            {
                CalledMethodNames.Add(nameof(MoveRow));
                OldIndex = oldRowIndex;
                NewIndex = newRowIndex;
                Count = count;
            }

            public void MoveColumn(int oldColumnIndex, int newColumnIndex, int count)
            {
                CalledMethodNames.Add(nameof(MoveColumn));
                OldIndex = oldColumnIndex;
                NewIndex = newColumnIndex;
                Count = count;
            }

            public void RemoveRow(int rowIndex, int count)
            {
                CalledMethodNames.Add(nameof(RemoveRow));
                Index = rowIndex;
                Count = count;
            }

            public void RemoveColumn(int columnIndex, int count)
            {
                CalledMethodNames.Add(nameof(RemoveColumn));
                Index = columnIndex;
                Count = count;
            }

            public void AdjustLength(int rowLength, int columnLength)
            {
                CalledMethodNames.Add(nameof(AdjustLength));
                RowLength = rowLength;
                ColumnLength = columnLength;
            }

            public void Reset(IEnumerable<IFixedLengthList<TestRecord>> initItems)
            {
                CalledMethodNames.Add(nameof(Reset));
                Items = initItems.ToTwoDimensionalArray();
            }

            #endregion

            #region Assersion

            // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

            public void CheckConstructor(IEnumerable<IEnumerable<TestRecord>> values, bool isValuesUseConstructor)
            {
                Assert.IsTrue(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Constructor)));

                var valuesArray = values.ToTwoDimensionalArray();

                Assert.IsTrue(isValuesUseConstructor
                    ? TestTools.IsAllItemReferenceEquals(InitItems, valuesArray)
                    : TestTools.IsAllItemEquals(InitItems, valuesArray));
                CheckNull(nameof(InitItems));
            }

            public void CheckGetRow(int row, int rowCount)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(GetRow)));

                Assert.AreEqual(row, Row);
                Assert.AreEqual(rowCount, RowCount);
                CheckNull(
                    nameof(Row),
                    nameof(RowCount)
                );
            }

            public void CheckGetColumn(int columnIndex, int columnCount)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(GetColumn)));

                Assert.AreEqual(columnIndex, Column);
                Assert.AreEqual(columnCount, ColumnCount);
                CheckNull(
                    nameof(Column),
                    nameof(ColumnCount)
                );
            }

            public void CheckGetItem(int rowIndex, int columnIndex)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(GetItem)));

                Assert.AreEqual(rowIndex, Row);
                Assert.AreEqual(columnIndex, Column);
                CheckNull(
                    nameof(Row),
                    nameof(Column)
                );
            }

            public void CheckGetItem(int rowIndex, int rowCount, int columnIndex, int columnCount)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(GetItem)));

                Assert.AreEqual(rowIndex, Row);
                Assert.AreEqual(rowCount, RowCount);
                Assert.AreEqual(columnIndex, Column);
                Assert.AreEqual(columnCount, ColumnCount);
                CheckNull(
                    nameof(Row),
                    nameof(RowCount),
                    nameof(Column),
                    nameof(ColumnCount)
                );
            }

            public void CheckSetRow(int row, params IEnumerable<TestRecord>[] rows)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(SetRow)));

                var itemArray = rows.ToTwoDimensionalArray();

                Assert.AreEqual(row, Row);
                Assert.IsTrue(TestTools.IsAllItemReferenceEquals(Items, itemArray));
                CheckNull(
                    nameof(Row),
                    nameof(Items)
                );
            }

            public void CheckSetColumn(int columnIndex, params IEnumerable<TestRecord>[] items)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(SetColumn)));

                var itemArray = items.ToTwoDimensionalArray();

                Assert.AreEqual(columnIndex, Column);
                Assert.IsTrue(TestTools.IsAllItemReferenceEquals(Items, itemArray));
                CheckNull(
                    nameof(Column),
                    nameof(Items)
                );
            }

            public void CheckSetItem(int rowIndex, int columnIndex, TestRecord item)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(SetItem)));

                var itemArray = new[] {new[] {item}};

                Assert.AreEqual(rowIndex, Row);
                Assert.AreEqual(columnIndex, Column);
                Assert.IsTrue(TestTools.IsAllItemReferenceEquals(Items, itemArray));
                CheckNull(
                    nameof(Row),
                    nameof(Column),
                    nameof(Items)
                );
            }

            public void CheckInsertRow(int rowIndex, params IEnumerable<TestRecord>[] items)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(InsertRow)));
                Assert.AreEqual(rowIndex, Index);
                CheckItemEquals(items);
                CheckNull(
                    nameof(Index),
                    nameof(Items)
                );
            }

            public void CheckInsertColumn(int columnIndex, params IEnumerable<TestRecord>[] items)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(InsertColumn)));
                Assert.AreEqual(columnIndex, Index);
                CheckItemEquals(items);
                CheckNull(
                    nameof(Index),
                    nameof(Items)
                );
            }

            public void CheckOverwriteRow(int index, params IEnumerable<TestRecord>[] items)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(OverwriteRow)));

                Assert.AreEqual(index, Index);
                CheckItemEquals(items);
                CheckNull(
                    nameof(Index),
                    nameof(Items),
                    nameof(Direction)
                );
            }

            public void CheckOverwriteColumn(int index, params IEnumerable<TestRecord>[] items)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(OverwriteColumn)));

                Assert.AreEqual(index, Index);
                CheckItemEquals(items);
                CheckNull(
                    nameof(Index),
                    nameof(Items),
                    nameof(Direction)
                );
            }

            public void CheckMoveRow(int oldIndex, int newIndex, int count)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(MoveRow)));

                Assert.AreEqual(oldIndex, OldIndex);
                Assert.AreEqual(newIndex, NewIndex);
                Assert.AreEqual(count, Count);
                CheckNull(
                    nameof(OldIndex),
                    nameof(NewIndex),
                    nameof(Count)
                );
            }

            public void CheckMoveColumn(int oldIndex, int newIndex, int count)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(MoveColumn)));

                Assert.AreEqual(oldIndex, OldIndex);
                Assert.AreEqual(newIndex, NewIndex);
                Assert.AreEqual(count, Count);
                CheckNull(
                    nameof(OldIndex),
                    nameof(NewIndex),
                    nameof(Count)
                );
            }

            public void CheckRemoveRow(int index, int count)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(RemoveRow)));

                Assert.AreEqual(index, Index);
                Assert.AreEqual(count, Count);
                CheckNull(
                    nameof(Index),
                    nameof(Count)
                );
            }

            public void CheckRemoveColumn(int index, int count)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(RemoveColumn)));

                Assert.AreEqual(index, Index);
                Assert.AreEqual(count, Count);
                CheckNull(
                    nameof(Index),
                    nameof(Count)
                );
            }

            public void CheckAdjustLength(int rowLength, int columnLength)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(AdjustLength)));

                Assert.AreEqual(rowLength, RowLength);
                Assert.AreEqual(columnLength, ColumnLength);
                CheckNull(nameof(RowLength), nameof(ColumnLength));
            }

            public void CheckReset(TestRecord[][] items)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Reset)));

                CheckItemEquals(items);
                CheckNull(nameof(Items));
            }

            public void CheckNotWork()
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(0, CalledMethodNames.Count);
                CheckNull();
            }

            private void CheckNull(params string[] ignores)
            {
                var targets = new List<string>
                {
                    nameof(Items),
                    nameof(InitItems),
                    nameof(Row),
                    nameof(Column),
                    nameof(RowCount),
                    nameof(ColumnCount),
                    nameof(Count),
                    nameof(OldIndex),
                    nameof(NewIndex),
                    nameof(RowLength),
                    nameof(ColumnLength),
                };

                targets.RemoveAll(ignores.Contains);

                foreach (var target in targets)
                {
                    switch (target)
                    {
                        case nameof(Items):
                            Assert.IsNull(Items);
                            break;
                        case nameof(InitItems):
                            Assert.IsNull(InitItems);
                            break;
                        case nameof(Row):
                            Assert.IsNull(Row);
                            break;
                        case nameof(Column):
                            Assert.IsNull(Column);
                            break;
                        case nameof(RowCount):
                            Assert.IsNull(RowCount);
                            break;
                        case nameof(ColumnCount):
                            Assert.IsNull(ColumnCount);
                            break;
                        case nameof(Count):
                            Assert.IsNull(Count);
                            break;
                        case nameof(OldIndex):
                            Assert.IsNull(OldIndex);
                            break;
                        case nameof(NewIndex):
                            Assert.IsNull(NewIndex);
                            break;
                        case nameof(RowLength):
                            Assert.IsNull(RowLength);
                            break;
                        case nameof(ColumnLength):
                            Assert.IsNull(ColumnLength);
                            break;
                        default:
                            Assert.Fail(target);
                            break;
                    }
                }
            }

            private void CheckItemEquals(IEnumerable<IEnumerable<TestRecord>> items)
            {
                var itemArrays = items.ToTwoDimensionalArray();
                var insertItemLength = itemArrays.Length;
                var insertItemInnerLength = itemArrays.GetInnerArrayLength();

                Assert.AreEqual(Items.Length, insertItemLength);
                Assert.AreEqual(Items.GetInnerArrayLength(), insertItemInnerLength);

                for (var r = 0; r < insertItemLength; r++)
                for (var c = 0; c < insertItemInnerLength; c++)
                {
                    Assert.IsTrue(itemArrays[r][c].ItemEquals(Items[r][c]));
                }
            }

            // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Local

            #endregion
        }

        #endregion
    }
}
